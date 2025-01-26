using BubbleWand.Player;
using MyBox;
using UnityEngine;

namespace BubbleWand.Environment {
    sealed class LoadSceneByAvatar : MonoBehaviour {
        [SerializeField]
        SceneReference sceneToLoad = new();

        void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<AvatarComponent>(out var avatar)) {
                sceneToLoad.LoadScene();
            }
        }
    }
}
