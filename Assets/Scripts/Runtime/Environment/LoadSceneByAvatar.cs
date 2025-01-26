using BubbleWand.Player;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace BubbleWand.Environment {
    sealed class LoadSceneByAvatar : MonoBehaviour {
        [SerializeField, Expandable]
        GameManager manager;

        void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<AvatarComponent>(out var avatar)) {
                manager.LoadMainMenu();
            }
        }
    }
}
