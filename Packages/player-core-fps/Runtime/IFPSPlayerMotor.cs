using UnityEngine;

namespace PlayerScriptsNS
{
    public interface IFPSPlayerMotor
    {
        public float SpeedCoef { get; set; }
        public bool IsGrounded { get; set; }
        public float DefaultSpeed { get; }
        public float CurrentSpeed { get; }
        public Vector3 Velocity { get; }
        public Vector3 VelocityScaleByTime { get; }
        public void EnableMovement();
        public void DisableMovement();
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation);
        public void ProcessMove(Vector2 input);
        public void OnPerformJump();
        public void OnPerformCrouch();
        public void OnStartSprint();
        public void OnCancelSprint();
    }
}