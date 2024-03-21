using AYellowpaper;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
    public class FPSPlayerCreature : PlayerCreature
    {
        [SerializeField, RequireInterface(typeof(IFPSPlayerMotor))]
        protected MonoBehaviour playerMotor;

        public IFPSPlayerMotor PlayerMotor { get { return (IFPSPlayerMotor)playerMotor; } }

        public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            BlockMovement();
            transform.position = position;
            transform.rotation = rotation;
            Task.Delay(5).GetAwaiter();
            UnblockMovement();
        }
        public override void BlockMovement() =>
            PlayerMotor.DisableMovement();
        public override void UnblockMovement() =>
            PlayerMotor.EnableMovement();
        protected override void SetCurrentSpeed() =>
            PlayerMotor.SpeedCoef = CurrentSpeedCoefficient;
    }
}