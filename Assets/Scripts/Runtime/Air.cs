using UnityEngine;

namespace BubbleWand {
    sealed class Air : ComponentFeature<Rigidbody> {
        [SerializeField]
        float killSpeed = 1;

        void FixedUpdate() {
            if (observedCompopnent.velocity.magnitude < killSpeed) {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<IAirReceiver>(out var receiver)) {
                receiver.Receive(transform.forward, observedCompopnent.velocity.magnitude);
            }
        }
    }
}
