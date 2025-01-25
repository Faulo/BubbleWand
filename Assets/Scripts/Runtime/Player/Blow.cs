using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityObject = UnityEngine.Object;

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

        GameObject bubble;

        void UpdateBlowing(float blowing, float deltaTime) {
            bool isBlowing = blowing > settings.minBlowVolume;

            if (isBlowing) {
                if (!bubble) {
                    bubble = UnityObject.Instantiate(settings.bubblePrefab, mouth);
                    bubble.transform.localScale = Vector3.zero;
                }

                bubble.transform.localScale += random * deltaTime;
                bubble.transform.SetPositionAndRotation(mouth.position + (mouth.forward * bubble.transform.localScale.z), mouth.rotation);
            } else {
                if (bubble) {
                    bubble.transform.parent = null;

                    if (bubble.TryGetComponent<Rigidbody>(out var rigidbody)) {
                        rigidbody.AddForce(settings.bubbleEjectScaling.Evaluate(blowing) * settings.bubbleEjectSpeed * mouth.forward, ForceMode.VelocityChange);
                    }

                    bubble = default;
                }
            }
        }

        Vector3 random => new(
            UnityEngine.Random.Range(0.8f, 1),
            UnityEngine.Random.Range(0.8f, 1),
            UnityEngine.Random.Range(0.8f, 1)
        );

        public void Update(float deltaTime) {
            UpdateBlowing(value, deltaTime);
        }
    }
}