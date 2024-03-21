using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSPlayerMotor : MonoBehaviour, IFPSPlayerMotor
    {
        [SerializeField]
        protected float gravity = -9.8f;
        [SerializeField]
        protected float groundCheckDistance;
        [SerializeField]
        protected LayerMask slopeLayer;
        [SerializeField]
        protected float jumpHeight = 3f;
        [SerializeField, Range(0f, 1f)]
        protected float inertiaWeight = 0.65f;
        [Header("Speed")]
        [SerializeField]
        private float defaultSpeed = 5;
        [SerializeField]
        private float sprintingSpeed = 8;
        [SerializeField]
        private float crounchingSpeed = 2.5f;

        protected bool lerpCrouch;
        protected float crounchTimer;
        private float speed;
        private bool isGrounded;
        private Vector3 yVelocity;

        public event Action OnSprintStartEvent;
        public event Action OnSprintCanceltEvent;

        public virtual float CurrentSpeed
        {
            get { return speed * SpeedCoef; }
            set { speed = value; }
        }
        public bool IsCrounching { get; protected set; }
        public bool IsSprinting { get; protected set; }
        public virtual bool IsGrounded
        {
            get => isGrounded;
            set
            {
                isGrounded = value;
                if (!IsGrounded && IsSprinting)
                    OnCancelSprint();
            }
        }
        public CharacterController Character { get; protected set; }
        public float SpeedCoef { get; set; }
        public float CrounchingSpeed { get => crounchingSpeed; set => crounchingSpeed = value; }
        public float SprintingSpeed { get => sprintingSpeed; set => sprintingSpeed = value; }
        public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }
        public Vector3 Velocity { get; protected set; }
        public Vector3 VelocityScaleByTime { get; protected set; }
        public virtual Vector3 YVelocity { get => yVelocity; protected set => yVelocity = value; }

        private void Awake()
        {
            Character = GetComponent<CharacterController>();
            CurrentSpeed = DefaultSpeed;
            IsSprinting = false;
        }
        protected virtual void Update()
        {
            IsGrounded = Character.isGrounded;
            LerpCrounch();
        }
        public virtual void EnableMovement()
        {
            Character.enabled = true;
        }
        public virtual void DisableMovement()
        {
            Character.enabled = false;
        }
        public virtual void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            DisableMovement();
            transform.position = position;
            transform.rotation = rotation;
            Task.Delay(5).GetAwaiter();
            EnableMovement();
        }
        public virtual void OnPerformJump()
        {
            if (IsGrounded && !IsCrounching)
                yVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        public virtual void OnPerformCrouch()
        {
            IsCrounching = !IsCrounching;
            crounchTimer = 0;
            lerpCrouch = true;
        }
        protected virtual void LerpCrounch()
        {
            if (lerpCrouch)
            {
                crounchTimer += Time.deltaTime;
                float p = crounchTimer / 0.5f;
                p *= 2 * p;
                if (IsCrounching)
                {
                    Character.height = Mathf.Lerp(Character.height, 1, p);
                    CurrentSpeed = CrounchingSpeed;
                }
                else
                {
                    Character.height = Mathf.Lerp(Character.height, 2, p);
                    CurrentSpeed = DefaultSpeed;
                }
                if (p > 1)
                {
                    lerpCrouch = false;
                    crounchTimer = 0f;
                }
            }
        }
        public virtual void OnStartSprint()
        {
            if (!IsCrounching && IsGrounded && DefaultSpeed != SprintingSpeed)
            {
                IsSprinting = true;
                CurrentSpeed = SprintingSpeed;
                OnSprintStartEvent?.Invoke();
            }
        }
        public virtual void OnCancelSprint()
        {
            OnSprintCanceltEvent?.Invoke();
            IsSprinting = false;
            CurrentSpeed = DefaultSpeed;
        }
        public virtual void ProcessMove(Vector2 input)
        {
            // Calculate target movement
            Vector3 horizontalMovement = new Vector3(input.x, 0, input.y);
            Vector3 targetVelocity = horizontalMovement * CurrentSpeed;
            // Apply inertia by combining the target velocity with last frame's velocity
            Vector3 velocityWithInertia = Vector3.Lerp(targetVelocity, Velocity, inertiaWeight);
            Vector3 velocity = SlopeCalculation(velocityWithInertia);
            Velocity = velocity;
            // Set Y component to 0
            velocity.y = 0;
            // Gravity application
            if (IsGrounded && yVelocity.y < 0)
                yVelocity.y = -2f;
            else
                yVelocity.y += gravity * Time.deltaTime;
            velocity += yVelocity;
            velocity = transform.TransformDirection(velocity);
            VelocityScaleByTime = velocity * Time.deltaTime;
            Character.Move(VelocityScaleByTime);
        }
        protected virtual Vector3 SlopeCalculation(Vector3 calculatedMovement)
        {
            if (IsGrounded)
            {
                float maxDistance = Character.height / 2 - Character.radius + groundCheckDistance;
                Physics.SphereCast(transform.position, Character.radius, Vector3.down, out RaycastHit groundCheckHit, maxDistance, slopeLayer);
                Vector3 localGroundCheckHitNormal = transform.InverseTransformDirection(groundCheckHit.normal);
                float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, transform.up);
                if (groundSlopeAngle > Character.slopeLimit)
                {
                    Quaternion slopeAngleRotation = Quaternion.FromToRotation(transform.up, localGroundCheckHitNormal);
                    calculatedMovement = slopeAngleRotation * calculatedMovement;
                    float relativeSlopeAngle = Vector3.Angle(calculatedMovement, transform.up) - 90.0f;
                    calculatedMovement += calculatedMovement * (relativeSlopeAngle / Character.slopeLimit);
                }
            }
            return calculatedMovement;
        }
    }
}