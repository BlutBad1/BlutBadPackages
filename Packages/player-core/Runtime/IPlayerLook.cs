using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScriptsNS
{
    public interface IPlayerLook
    {
        public Vector3 CameraCurrentRotation { get; }
    }
}
