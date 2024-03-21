using UnityEngine;

namespace PlayerScriptsNS
{
    public interface IFPSPlayerMotor
    {
        public float SpeedCoef { get; set; }
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