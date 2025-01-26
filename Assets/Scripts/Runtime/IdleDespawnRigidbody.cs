using UnityEngine;

namespace BubbleWand {
    sealed class IdleDespawnRigidbody : ComponentFeature<Rigidbody> {
        [SerializeField]
        float killSpeed = 1;

        void FixedUpdate() {
            if (observedCompopnent.velocity.magnitude < killSpeed) {
                SendMessage(nameof(IKillMessages.Die), SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
