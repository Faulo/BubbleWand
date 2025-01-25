using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace BubbleWand.Player {
    [Serializable]
    public class Look : IUpdatable, IDisposable {
        readonly IAvatar avatar;
        readonly AvatarSettings settings;
        readonly InputActionMap input;
        readonly Transform body;
        readonly Transform eyes;
        readonly CinemachineVirtualCamera cinemachine;

        float horizontalCurrentAngle;
        float horizontalTargetAngle;
        float horizontalSpeed;
        void UpdateHorizontalAngle(float deltaTime) {
            if (isMouseLook) {
                horizontalCurrentAngle = Mathf.SmoothDampAngle(
                    horizontalCurrentAngle,
                    horizontalTargetAngle,
                    ref horizontalSpeed,
                    settings.cameraSmoothing,
                    float.PositiveInfinity,
                    deltaTime
                );
            } else {
                horizontalCurrentAngle += horizontalSpeed * deltaTime;
                horizontalTargetAngle = horizontalCurrentAngle;
            }

            body.localRotation = Quaternion.Euler(0, horizontalCurrentAngle, 0);
        }

        float verticalCurrentAngle;
        float verticalTargetAngle;
        float verticalSpeed;
        void UpdateVerticalAngle(float deltaTime) {
            if (isMouseLook) {
                verticalTargetAngle = Mathf.Clamp(verticalTargetAngle, settings.cameraMinX, settings.cameraMaxX);

                verticalCurrentAngle = Mathf.SmoothDampAngle(
                    verticalCurrentAngle,
                    verticalTargetAngle,
                    ref verticalSpeed,
                    settings.cameraSmoothing,
                    float.PositiveInfinity,
                    deltaTime
                );
            } else {
                verticalCurrentAngle += verticalSpeed * deltaTime;
                verticalCurrentAngle = Mathf.Clamp(verticalCurrentAngle, settings.cameraMinX, settings.cameraMaxX);
                verticalTargetAngle = verticalCurrentAngle;
            }

            eyes.localRotation = Quaternion.Euler(verticalCurrentAngle, 0, 0);
        }

        float fieldOfViewAngle;
        float fieldOfViewSpeed;
        void UpdateFOV(float deltaTime) {
            fieldOfViewAngle = Mathf.SmoothDamp(
                fieldOfViewAngle,
                avatar.isRunning ? settings.runningFieldOfView : settings.defaultFieldOfView,
                ref fieldOfViewSpeed,
                settings.fieldOfViewSmoothingTime,
                float.PositiveInfinity,
                deltaTime
            );
            cinemachine.m_Lens.FieldOfView = fieldOfViewAngle;
        }

        bool isLocked {
            get => Cursor.lockState == CursorLockMode.Locked;
            set => Cursor.lockState = value
                ? CursorLockMode.Locked
                : CursorLockMode.None;
        }

        public Look(IAvatar avatar, AvatarSettings settings, InputActionMap input, Transform body, Transform eyes, CinemachineVirtualCamera cinemachine) {
            this.avatar = avatar;
            this.settings = settings;
            this.input = input;
            this.body = body;
            this.eyes = eyes;
            this.cinemachine = cinemachine;

            RegisterInput();

            isLocked = true;
        }

        public void Dispose() {
            isLocked = false;
            UnregisterInput();
        }

        void RegisterInput() {
            input["Look"].performed += HandleLook;
            input["Menu"].performed += HandleMenu;
        }
        void UnregisterInput() {
            input["Look"].performed -= HandleLook;
            input["Menu"].performed -= HandleMenu;
        }

        bool isMouseLook;

        void HandleLook(InputAction.CallbackContext context) {
            isMouseLook = context.control is DeltaControl;

            if (isMouseLook) {
                if (isLocked) {
                    var deltaLook = context.ReadValue<Vector2>() * settings.cameraMouseSpeed;

                    horizontalTargetAngle += deltaLook.x;
                    verticalTargetAngle -= deltaLook.y;
                }
            } else {
                var targetSpeed = context.ReadValue<Vector2>() * settings.cameraStickSpeed;

                horizontalSpeed = targetSpeed.x;
                verticalSpeed = targetSpeed.y;
            }
        }

        void HandleMenu(InputAction.CallbackContext context) {
            isLocked = !isLocked;
        }

        public void Update(float deltaTime) {
            UpdateHorizontalAngle(deltaTime);
            UpdateVerticalAngle(deltaTime);
            UpdateFOV(deltaTime);
        }
    }
}