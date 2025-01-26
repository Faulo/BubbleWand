namespace BubbleWand.Player {
    public interface ICharacterMessages {
        void OnJumpFromPlatform(IAvatar avatar);
        void OnLandOnPlatform(IAvatar avatar);
    }
}