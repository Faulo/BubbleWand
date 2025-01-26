using UnityEngine;

namespace BubbleWand.Environment {
    sealed class RotatingRigidbody : ComponentFeature<Rigidbody> {
        [SerializeField]
        Vector3 impulse = Vector3.up;

        void FixedUpdate() {
            observedCompopnent.AddTorque(impulse * Random.value, ForceMode.VelocityChange);
        }
    }
}
