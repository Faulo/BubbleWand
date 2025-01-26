using BubbleWand.Player;
using UnityEngine;

namespace BubbleWand.Environment {
    sealed class KillOnJump : MonoBehaviour, ICharacterMessages {
        public void OnJumpFromPlatform(IAvatar avatar) => SendMessage(nameof(IKillMessages.Die), SendMessageOptions.DontRequireReceiver);
        public void OnLandOnPlatform(IAvatar avatar) { }
    }
}
