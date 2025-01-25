using UnityEngine;

namespace BubbleWand {
    sealed class FragileRigidbody : ComponentFeature<Rigidbody> {
        [SerializeField]
        float killImpulse = 1;
        [SerializeField]
        GameObject killPrefab;

        void OnCollisionEnter(Collision collision) {
            if (collision.impulse.magnitude > killImpulse) {
                if (killPrefab) {
                    var instance = Instantiate(killPrefab, transform.position, transform.rotation);
                    instance.transform.localScale = transform.localScale;
                }

                Destroy(gameObject);
            }
        }
    }
}
