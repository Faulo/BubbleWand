using UnityEngine;

namespace BubbleWand {
    sealed class FragileRigidbody : ComponentFeature<Rigidbody> {
        [SerializeField]
        float killImpulse = 1;

        void OnCollisionEnter(Collision collision) {
            if (collision.impulse.magnitude > killImpulse) {
                SendMessage(nameof(IKillMessages.Die), SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
