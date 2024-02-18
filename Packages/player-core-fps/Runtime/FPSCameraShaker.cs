using EZCameraShake;
using UnityEngine;

namespace PlayerScriptsNS
{
	public class FPSCameraShaker : CameraShaker
	{
		[SerializeField]
		private PlayerLook playerLook;

		protected override void SetLocalRotation()
		{
			if (playerLook != null)
				transform.localEulerAngles = rotAddShake + RestRotationOffset + playerLook.PlayerCameraCurRotation;
			else
				transform.localEulerAngles = rotAddShake + RestRotationOffset;
		}
	}
}