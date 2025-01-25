using UnityEngine;

namespace BubbleWand {
    sealed class FallableRigidbody : ComponentFeature<Rigidbody> {
        [SerializeField]
        float gravityMultiplier = 1;

        void FixedUpdate() {
            observedCompopnent.velocity += gravityMultiplier * Time.deltaTime * Physics.gravity;
        }
    }
}
