using System;
using Cinemachine;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleWand.Player {
    sealed class AvatarComponent : MonoBehaviour, IAvatar {
        [Header("MonoBehaviour Configuration")]
        [SerializeField, Expandable]
        InputActionAsset controls = default;
        [SerializeField, Expandable]
        AvatarSettings settings = default;
        [SerializeField, Expandable]
        CharacterController character = default;
        [SerializeField, Expandable]
        Transform body = default;
        [SerializeField, Expandable]
        Transform eyes = default;
        [SerializeField, Expandable]
        Transform mouth = default;
        [SerializeField, Expandable]
        ParticleSystem blowParticles = default;
        [SerializeField, Expandable]
        CinemachineVirtualCamera cinemachineCamera = default;

        [Header("Unity Configuration")]
        [SerializeField]
        UpdateMethod updateMethod = UpdateMethod.FixedUpdate;

        Movement movement;
        Look look;
        Blow blow;
        Aim aim;

        public event Action<ControllerColliderHit> onControllerColliderHit;
        public event Action onJumpCountChanged;

        public Vector3 forward => eyes.forward;
        public Vector3 velocity => movement.currentVelocity;
        public Vector3 position => eyes.position;
        public Quaternion rotation => eyes.rotation;
        public bool isRunning => movement.isRunning;
        public bool isBlowing => blow.isBlowing;
        public bool isAiming => aim.isAiming;

        [SerializeField]
        int m_jumpCount = 1;
        public int jumpCount {
            get => m_jumpCount;
            set {
                if (m_jumpCount != value) {
                    m_jumpCount = value;
                    onJumpCountChanged?.Invoke();
                }
            }
        }

        Mic mic;

        void Awake() {
            controls = Instantiate(controls);
            movement = new Movement(this, settings, controls.FindActionMap("Player"), character);
            look = new Look(this, settings, controls.FindActionMap("Player"), body, eyes, cinemachineCamera);
            blow = new Blow(this, settings, controls.FindActionMap("Player"), mouth, blowParticles);
            aim = new Aim(this, settings, controls.FindActionMap("Player"));

            onJumpCountChanged += () => settings.onJumpCountChanged.Invoke(gameObject);
        }

        void OnEnable() {
            mic = InputSystem.AddDevice<Mic>();
            controls.Enable();
        }

        void OnDisable() {
            controls.Disable();
            InputSystem.RemoveDevice(mic);
        }

        void OnDestroy() {
            movement.Dispose();
            look.Dispose();
            blow.Dispose();
            aim.Dispose();
        }
        void Update() {
            if (updateMethod == UpdateMethod.Update) {
                UpdateAvatar(Time.deltaTime);
            }
        }
        void FixedUpdate() {
            if (updateMethod == UpdateMethod.FixedUpdate) {
                UpdateAvatar(Time.deltaTime);
            }
        }
        void LateUpdate() {
            if (updateMethod == UpdateMethod.LateUpdate) {
                UpdateAvatar(Time.deltaTime);
            }
        }
        void UpdateAvatar(float deltaTime) {
            look.Update(deltaTime);
            movement.Update(deltaTime);
            blow.Update(deltaTime);
        }
        void OnControllerColliderHit(ControllerColliderHit hit) {
            onControllerColliderHit?.Invoke(hit);
        }

        public Rigidbody platform => movement.platform;
    }
}