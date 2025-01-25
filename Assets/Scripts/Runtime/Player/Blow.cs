using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleWand.Player {
    [Serializable]
    public class Blow : IUpdatable, IDisposable {
        public static float value;

        readonly AvatarComponent avatarComponent;
        readonly AvatarSettings settings;
        readonly InputActionMap input;
        readonly Transform mouth;
        readonly Transform eyes;

        public Blow(AvatarComponent avatarComponent, AvatarSettings settings, InputActionMap input, Transform mouth) {
            this.avatarComponent = avatarComponent;
            this.settings = settings;
            this.input = input;
            this.mouth = mouth;

            RegisterInput();
        }

        public void Dispose() {
            UnregisterInput();
        }

        void RegisterInput() {
            input["Blow"].performed += HandleBlow;
        }

        void UnregisterInput() {
            input["Blow"].performed -= HandleBlow;
        }

        void HandleBlow(InputAction.CallbackContext context) {
            value = context.ReadValue<float>();
        }

        public void Update(float deltaTime) {
        }
    }
}