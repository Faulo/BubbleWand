using UnityEngine;
using UnityEngine.Events;

namespace BubbleWand {
    sealed class KillableObject : MonoBehaviour, IKillMessages {
        [SerializeField]
        GameObject killPrefab;
        [SerializeField]
        UnityEvent<GameObject> onKill = new();

        public void Die() {
            onKill.Invoke(gameObject);

            if (killPrefab) {
                var instance = Instantiate(killPrefab, transform.position, transform.rotation);
                instance.transform.localScale = transform.localScale;
            }

            Destroy(gameObject);
        }
    }
}
