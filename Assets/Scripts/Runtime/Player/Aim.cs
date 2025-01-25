using System;
using UnityEngine.InputSystem;

namespace BubbleWand.Player {
    [Serializable]
    sealed class Aim : IUpdatable, IDisposable {
        readonly IAvatar avatar;
        readonly AvatarSettings settings;
        readonly InputActionMap input;

        public bool isAiming { get; private set; }

        public Aim(IAvatar avatar, AvatarSettings settings, InputActionMap input) {
            this.avatar = avatar;
            this.settings = settings;
            this.input = input;

            RegisterInput();
        }

        public void Dispose() {
            UnregisterInput();
        }

        void RegisterInput() {
            input["Aim"].started += HandleAimStart;
            input["Aim"].canceled += HandleAimCancel;
        }

        void UnregisterInput() {
            input["Aim"].started -= HandleAimStart;
            input["Aim"].canceled -= HandleAimCancel;
        }

        void HandleAimStart(InputAction.CallbackContext context) {
            isAiming = true;
        }

        void HandleAimCancel(InputAction.CallbackContext context) {
            isAiming = false;
        }

        public void Update(float deltaTime) {
        }
    }
}