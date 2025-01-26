using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityObject = UnityEngine.Object;

namespace BubbleWand.Player {
    [Serializable]
    sealed class Blow : IUpdatable, IDisposable {
        public static float value;

        readonly IAvatar avatar;
        readonly AvatarSettings settings;
        readonly InputActionMap input;
        readonly Transform mouth;
        readonly Transform bubblePivot;
        readonly ParticleSystem blowParticles;

        public Blow(IAvatar avatar, AvatarSettings settings, InputActionMap input, Transform mouth, Transform bubblePivot, ParticleSystem blowParticles) {
            this.avatar = avatar;
            this.settings = settings;
            this.input = input;
            this.mouth = mouth;
            this.bubblePivot = bubblePivot;
            this.blowParticles = blowParticles;

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

        public bool isBlowing { get; private set; }

        void UpdateBlowing(float blowing, float deltaTime) {
            isBlowing = blowing > settings.minBlowVolume;

            var emission = blowParticles.emission;
            emission.rateOverTime = 0;

            if (isBlowing && avatar.isAiming && (!bubble || bubble.transform.localScale.z < settings.maxBlowSize)) {
                if (!bubble) {
                    bubble = UnityObject.Instantiate(settings.bubblePrefab, bubblePivot);
                    bubble.transform.localScale = Vector3.one * settings.startBlowSize;

                    if (bubble.TryGetComponent<Rigidbody>(out var rigidbody)) {
                        rigidbody.detectCollisions = false;
                    }
                }

                bubble.transform.localScale += deltaTime * settings.blowGain * random;
                bubble.transform.SetPositionAndRotation(bubblePivot.position + (0.5f * bubble.transform.localScale.z * bubblePivot.forward), bubblePivot.rotation);
            } else {
                if (bubble) {
                    bubble.transform.parent = null;

                    if (bubble.TryGetComponent<Rigidbody>(out var rigidbody)) {
                        rigidbody.detectCollisions = true;
                        rigidbody.AddForce(avatar.velocity * settings.bubbleVelocityMultiplier, ForceMode.VelocityChange);
                        rigidbody.AddForce(settings.bubbleEjectScaling.Evaluate(blowing) * settings.bubbleEjectSpeed * bubblePivot.forward, ForceMode.VelocityChange);

                        if (bubble.transform.localScale.z < settings.minBlowSize) {
                            rigidbody.excludeLayers = 1 << LayerMask.NameToLayer("Player");
                        }
                    }

                    bubble = default;
                }

                if (isBlowing) {
                    emission.rateOverTime = blowing * settings.airParticleMultiplier;
                    var main = blowParticles.main;
                    main.startSpeedMultiplier = blowing * settings.airParticleSpeed;

                    var air = UnityObject.Instantiate(settings.airPrefab, mouth.transform.position, mouth.transform.rotation);

                    if (air.TryGetComponent<Rigidbody>(out var rigidbody)) {
                        rigidbody.AddForce(avatar.velocity * settings.bubbleVelocityMultiplier, ForceMode.VelocityChange);
                        rigidbody.AddForce(settings.bubbleEjectScaling.Evaluate(blowing) * settings.airEjectSpeed * mouth.forward, ForceMode.VelocityChange);
                    }
                }
            }
        }

        Vector3 random => new(
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            UnityEngine.Random.value
        );

        public void Update(float deltaTime) {
            UpdateBlowing(value, deltaTime);
        }
    }
}