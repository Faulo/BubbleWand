using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleWand.Player {
    [Serializable]
    sealed class Movement : IUpdatable, IDisposable {
        enum JumpState {
            NotJumping,
            ShortJump,
            MediumJump,
            LongJump,
        }

        IAvatar avatar;
        AvatarSettings settings;
        InputActionMap input;
        CharacterController character;

        Vector2 intendedMovement;
        Vector3 targetVelocity;
        Vector2 targetMovement;
        public Vector3 currentVelocity;
        Vector3 acceleration;

        bool intendsToJump;
        JumpState jumpState;
        float jumpTimer;
        int jumpCount;
        float currentSpeed => isRunning
            ? settings.runningSpeed
            : settings.walkingSpeed;

        float stepDistance;
        float airDistance;

        public Rigidbody platform => character.isGrounded
            ? lastPlatform
            : null;

        Rigidbody lastPlatform;

        public bool isRunning => input["Sprint"].phase == InputActionPhase.Performed && intendedMovement != Vector2.zero;

        public Movement(IAvatar avatar, AvatarSettings settings, InputActionMap input, CharacterController character) {
            this.avatar = avatar;
            this.settings = settings;
            this.character = character;
            this.input = input;

            avatar.onControllerColliderHit += HandleHit;

            RegisterInput();
        }

        public void Dispose() {
            avatar.onControllerColliderHit -= HandleHit;

            UnregisterInput();
        }

        void RegisterInput() {
            input["Jump"].started += HandleJumpStart;
            input["Jump"].canceled += HandleJumpCancel;
        }
        void UnregisterInput() {
            input["Jump"].started -= HandleJumpStart;
            input["Jump"].canceled -= HandleJumpCancel;
        }

        public void Update(float deltaTime) {
            CalculateTargetVelocity();
            ProcessJump();

            targetVelocity.y = currentVelocity.y;

            if (avatar.platform) {
                targetVelocity += avatar.platform.velocity;
            }

            currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref acceleration, settings.smoothingTime);

            float gravity = Physics.gravity.y * deltaTime;
            if (jumpState != JumpState.NotJumping) {
                gravity *= settings.jumpGravityMultiplier;
            }

            if (character.isGrounded && currentVelocity.y <= gravity) {
                currentVelocity.y = gravity;
            } else {
                currentVelocity.y += gravity;
            }

            var position = avatar.position;
            character.Move(currentVelocity * deltaTime);
            ProcessStep(Vector3.Distance(position, avatar.position));
        }

        void HandleHit(ControllerColliderHit hit) {
            if (Vector3.Dot(hit.normal, Vector3.up) > Mathf.Cos(character.slopeLimit * Mathf.Deg2Rad)) {
                lastPlatform = hit.rigidbody;
                if (hit.gameObject) {
                    hit.gameObject.SendMessage(nameof(ICharacterMessages.OnLandOnPlatform), avatar, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        void ProcessStep(float deltaStep) {
            if (!character.isGrounded) {
                airDistance += deltaStep;
                return;
            }

            stepDistance += airDistance;
            stepDistance += deltaStep;
            airDistance = 0;
            if (stepDistance >= settings.metersPerStep) {
                stepDistance = 0;
                settings.onStep.Invoke(avatar.gameObject);
            }
        }

        void CalculateTargetVelocity() {
            intendedMovement = input["Move"].ReadValue<Vector2>();
            targetVelocity = character.transform.rotation * new Vector3(intendedMovement.x, 0, intendedMovement.y);
            targetVelocity *= currentSpeed;
            if (targetVelocity != Vector3.zero) {
                float forward = Vector3.Dot(targetVelocity.normalized, character.transform.forward);
                targetVelocity *= settings.speedOverForward.Evaluate(forward);
            }

            targetMovement = new Vector2(targetVelocity.x, targetVelocity.z);
            if (targetMovement != Vector2.zero) {
                targetMovement.Normalize();
            }
        }
        void ProcessJump() {
            switch (jumpState) {
                case JumpState.NotJumping:
                    if (character.isGrounded) {
                        jumpCount = 0;
                    } else {
                        jumpCount = Mathf.Max(jumpCount, 1);
                    }

                    if (jumpCount < avatar.jumpCount && intendsToJump) {
                        jumpCount++;
                        jumpTimer = 0;
                        jumpState = JumpState.ShortJump;
                        currentVelocity.x += targetMovement.x * settings.jumpStartSpeed.x;
                        currentVelocity.y = settings.jumpStartSpeed.y;
                        currentVelocity.z += targetMovement.y * settings.jumpStartSpeed.x;

                        if (lastPlatform) {
                            lastPlatform.SendMessage(nameof(ICharacterMessages.OnJumpFromPlatform), avatar, SendMessageOptions.DontRequireReceiver);
                        }

                        settings.onJumpStart.Invoke(avatar.gameObject);
                    }

                    break;
                case JumpState.ShortJump:
                    jumpTimer += Time.deltaTime;
                    if (jumpTimer >= settings.shortJumpInputDuration && intendsToJump) {
                        jumpState = JumpState.MediumJump;
                    }

                    break;
                case JumpState.MediumJump:
                    jumpTimer += Time.deltaTime;
                    if (jumpTimer >= settings.mediumJumpInputDuration && intendsToJump) {
                        jumpState = JumpState.LongJump;
                    }

                    break;
                case JumpState.LongJump:
                    jumpTimer += Time.deltaTime;
                    break;
                default:
                    break;
            }

            if (jumpState != JumpState.NotJumping) {
                float duration = jumpState switch {
                    JumpState.ShortJump => settings.shortJumpExecutionDuration,
                    JumpState.MediumJump => settings.mediumJumpExecutionDuration,
                    JumpState.LongJump => settings.longJumpExecutionDuration,
                    _ => throw new NotImplementedException(),
                };
                var stopSpeed = jumpState switch {
                    JumpState.ShortJump => settings.jumpShortStopSpeed,
                    JumpState.MediumJump => settings.jumpMediumStopSpeed,
                    JumpState.LongJump => settings.jumpLongStopSpeed,
                    _ => throw new NotImplementedException(),
                };
                if (jumpTimer >= duration) {
                    intendsToJump = false;
                    jumpState = JumpState.NotJumping;
                    currentVelocity.x += targetMovement.x * stopSpeed.x;
                    currentVelocity.y = Math.Min(currentVelocity.y, stopSpeed.y);
                    currentVelocity.z += targetMovement.y * stopSpeed.x;
                }
            }
        }

        void HandleJumpStart(InputAction.CallbackContext context) {
            intendsToJump = true;
        }
        void HandleJumpCancel(InputAction.CallbackContext context) {
            intendsToJump = false;
        }
    }
}