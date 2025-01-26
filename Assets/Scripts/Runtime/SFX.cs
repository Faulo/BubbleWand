using UnityEngine;

namespace BubbleWand {
    sealed class SFX : ComponentFeature<AudioSource> {
        [SerializeField]
        float minPitch = 0.8f;
        [SerializeField]
        float maxPitch = 1.2f;
        [SerializeField]
        float offset = 0f;

        public void InstantiateAt(GameObject target) {
            Instantiate(this, target.transform.position, target.transform.rotation);
        }

        void Start() {
            observedCompopnent.time = offset;
            observedCompopnent.pitch *= Random.Range(minPitch, maxPitch);
        }

        void Update() {
            if (!observedCompopnent.isPlaying) {
                Destroy(gameObject);
            }
        }
    }
}
