using System;
using UnityEngine;

namespace BubbleWand.Player {
    public interface IAvatar {
        event Action<ControllerColliderHit> onControllerColliderHit;
        GameObject gameObject { get; }
        Vector3 forward { get; }
        Vector3 position { get; }
        Quaternion rotation { get; }
        Vector3 velocity { get; }
        int jumpCount { get; set; }
        bool isRunning { get; }
        bool isBlowing { get; }
        bool isAiming { get; }
        bool canAim { get; set; }

        Rigidbody platform { get; }
    }
}