using UnityEngine;

namespace BubbleWand.Environment {
    sealed class AirBlower : MonoBehaviour {
        [SerializeField]
        GameObject airPrefab;
        [SerializeField]
        Transform pivot;
        [SerializeField]
        float ejectSpeed = 10;
        [SerializeField]
        int ticksPerAir = 2;

        int ticks;
        void FixedUpdate() {
            ticks++;
            if (ticks >= ticksPerAir) {
                ticks = 0;

                var air = Instantiate(airPrefab, pivot.transform.position, pivot.transform.rotation);

                if (air.TryGetComponent<Rigidbody>(out var rigidbody)) {
                    rigidbody.AddForce(ejectSpeed * pivot.forward, ForceMode.VelocityChange);
                }
            }
        }
    }
}
