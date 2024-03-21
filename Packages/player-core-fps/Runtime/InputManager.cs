using AYellowpaper;
using InputNS;
using UnityEngine;

namespace PlayerScriptsNS
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput.OnFootActions onFoot;
        [SerializeField]
        private PlayerInput.UIActions uIActions;
        [SerializeField, RequireInterface(typeof(IFPSPlayerMotor))]
        protected MonoBehaviour playerMotor;
        [SerializeField, RequireInterface(typeof(IFPSPlayerLook))]
        protected MonoBehaviour playerLook;

        private PlayerInput playerInput;

        public IFPSPlayerMotor PlayerMotor { get { return (IFPSPlayerMotor)playerMotor; } }
        public IFPSPlayerLook PlayerLook { get { return (IFPSPlayerLook)playerLook; } }
        public PlayerInput.OnFootActions OnFoot { get => onFoot; }
        public PlayerInput.UIActions UIActions { get => uIActions; }

        private void Awake()
        {
            InitializeFields();
        }
        private void OnEnable()
        {
            SubscribeEvents();
            onFoot.Enable();
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
            onFoot.Disable();
        }
        private void FixedUpdate()
        {
            PlayerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
        private void LateUpdate()
        {
            PlayerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
        private void PerformJump(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            PlayerMotor.OnPerformJump();
        private void PerformCrounch(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            PlayerMotor.OnPerformCrouch();
        private void StartSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            PlayerMotor.OnStartSprint();
        private void CancelSprint(UnityEngine.InputSystem.InputAction.CallbackContext obj) =>
            PlayerMotor.OnCancelSprint();
        private void InitializeFields()
        {
            playerInput = GetPlayerInput.GetInput();
            onFoot = playerInput.OnFoot;
            uIActions = playerInput.UI;
            if (playerMotor == null)
                playerMotor = (MonoBehaviour)GetComponent<IFPSPlayerMotor>();
            if (playerLook == null)
                playerLook = (MonoBehaviour)GetComponent<IFPSPlayerLook>();
        }
        private void SubscribeEvents()
        {
            onFoot.Jump.performed += PerformJump;
            onFoot.Crouch.performed += PerformCrounch;
            onFoot.Sprint.started += StartSprint;
            onFoot.Sprint.canceled += CancelSprint;
        }
        private void UnsubscribeEvents()
        {
            onFoot.Jump.performed -= PerformJump;
            onFoot.Crouch.performed -= PerformCrounch;
            onFoot.Sprint.started -= StartSprint;
            onFoot.Sprint.canceled -= CancelSprint;
        }
    }
}