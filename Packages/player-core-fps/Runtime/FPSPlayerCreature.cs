using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScriptsNS
{
	public class FPSPlayerCreature : PlayerCreature
	{
		[SerializeField, FormerlySerializedAs("PlayerMotor")]
		protected PlayerMotor playerMotor;

		public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
		{
			BlockMovement();
			transform.position = position;
			transform.rotation = rotation;
			Task.Delay(5).GetAwaiter();
			UnblockMovement();
		}
		public override void BlockMovement() =>
			playerMotor.GetCharacterController().enabled = false;
		public override void UnblockMovement() =>
			playerMotor.GetCharacterController().enabled = true;
		public override void SetSpeedCoef(float speedCoef) =>
			playerMotor.SetSpeedCoef(speedCoef);
	}
}