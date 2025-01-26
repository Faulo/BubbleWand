using BubbleWand.Player;
using UnityEngine;

namespace BubbleWand.Environment {
    sealed class CollectByAvatar : MonoBehaviour {
        void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<AvatarComponent>(out var avatar)) {
                avatar.canAim = true;

                SendMessage(nameof(IKillMessages.Die), SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
