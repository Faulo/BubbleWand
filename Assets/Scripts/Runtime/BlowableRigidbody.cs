using UnityEngine;

namespace BubbleWand {
    sealed class BlowableRigidbody : ComponentFeature<Rigidbody>, IAirReceiver {
        [SerializeField]
        float multiplier = 1;

        public void Receive(Vector3 direction, float magnitude) => observedCompopnent.AddForce(multiplier * magnitude * direction, ForceMode.Acceleration);
    }
}
