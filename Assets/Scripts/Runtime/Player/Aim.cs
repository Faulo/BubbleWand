using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleWand.Player {
    [Serializable]
    sealed class Aim : IUpdatable, IDisposable {
        readonly IAvatar avatar;
        readonly AvatarSettings settings;
        readonly InputActionMap input;
        readonly Transform bubbleWand;

        bool _isAiming;
        public bool isAiming {
            get => _isAiming;
            private set {
                _isAiming = value;
                bubbleWand.gameObject.SetActive(value);
            }
        }

        public Aim(IAvatar avatar, AvatarSettings settings, InputActionMap input, Transform bubbleWand) {
            this.avatar = avatar;
            this.settings = settings;
            this.input = input;
            this.bubbleWand = bubbleWand;

            isAiming = false;

            RegisterInput();
        }

        public void Dispose() {
            UnregisterInput();
        }

        void RegisterInput() {
            input["Aim"].started += HandleAimStart;
            input["Aim"].canceled += HandleAimCancel;
            input["Cheat"].performed += HandleCheat;
        }

        void UnregisterInput() {
            input["Aim"].started -= HandleAimStart;
            input["Aim"].canceled -= HandleAimCancel;
            input["Cheat"].performed -= HandleCheat;
        }

        void HandleCheat(InputAction.CallbackContext context) {
            avatar.canAim = true;
        }

        void HandleAimStart(InputAction.CallbackContext context) {
            isAiming = avatar.canAim;
        }

        void HandleAimCancel(InputAction.CallbackContext context) {
            isAiming = false;
        }

        public void Update(float deltaTime) {
        }
    }
}