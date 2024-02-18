using UnityEngine;

namespace PlayerScriptsNS
{
	public abstract class PlayerLook : MonoBehaviour
	{
		public abstract Vector3 PlayerCameraCurRotation { get; protected set; }
	}
}