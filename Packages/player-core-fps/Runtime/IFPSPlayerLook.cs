using UnityEngine;

namespace PlayerScriptsNS
{
    public interface IFPSPlayerLook : IPlayerLook
    {
        public void ProcessLook(Vector2 input);
    }
}