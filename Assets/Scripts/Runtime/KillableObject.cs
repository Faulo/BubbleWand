using UnityEngine;

namespace BubbleWand {
    sealed class KillableObject : MonoBehaviour, IKillMessages {
        [SerializeField]
        GameObject killPrefab;

        public void Die() {
            if (killPrefab) {
                var instance = Instantiate(killPrefab, transform.position, transform.rotation);
                instance.transform.localScale = transform.localScale;
            }

            Destroy(gameObject);
        }
    }
}
