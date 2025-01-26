using BubbleWand.Player;
using UnityEngine;

namespace BubbleWand.Environment {
    sealed class FallOnLand : ComponentFeature<Rigidbody>, ICharacterMessages {
        [SerializeField]
        float gravityMultiplier = 1;

        public void OnJumpFromPlatform(IAvatar avatar) { }
        public void OnLandOnPlatform(IAvatar avatar) => observedCompopnent.velocity += gravityMultiplier * Time.deltaTime * Physics.gravity;
    }
}
