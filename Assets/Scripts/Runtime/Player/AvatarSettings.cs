using UnityEngine;
using UnityEngine.Events;

namespace BubbleWand.Player {
    [CreateAssetMenu]
    public class AvatarSettings : ScriptableAsset {
        [Header("Movement")]
        [SerializeField, Range(0, 100)]
        public float walkingSpeed = 5;
        [SerializeField, Range(0, 100)]
        public float runningSpeed = 12;
        [SerializeField]
        public AnimationCurve speedOverForward = AnimationCurve.Constant(-1, 1, 1);
        [SerializeField, Range(0, 10)]
        public float smoothingTime = 1;
        [SerializeField, Range(0, 10)]
        public float metersPerStep = 1;

        [Header("Jumping")]
        [SerializeField, Range(-10, 10)]
        public float jumpGravityMultiplier = 1;
        [SerializeField]
        public Vector2 jumpStartSpeed = Vector2.one;
        [SerializeField]
        public Vector2 jumpShortStopSpeed = Vector2.one;
        [SerializeField]
        public Vector2 jumpMediumStopSpeed = Vector2.one;
        [SerializeField]
        public Vector2 jumpLongStopSpeed = Vector2.one;
        [SerializeField, Range(0, 1)]
        public float shortJumpInputDuration = 0.1f;
        [SerializeField, Range(0, 1)]
        public float mediumJumpInputDuration = 0.2f;
        [SerializeField, Range(0, 1)]
        public float shortJumpExecutionDuration = 0.25f;
        [SerializeField, Range(0, 1)]
        public float mediumJumpExecutionDuration = 0.5f;
        [SerializeField, Range(0, 1)]
        public float longJumpExecutionDuration = 0.75f;

        [Header("Look")]
        [SerializeField]
        public Vector2 cameraMouseSpeed = Vector2.one;
        [SerializeField]
        public Vector2 cameraStickSpeed = Vector2.one;
        [SerializeField, Range(0, 10)]
        public float cameraSmoothing = 1;
        [SerializeField, Range(-180, 180)]
        public float cameraMinX = -90;
        [SerializeField, Range(-180, 180)]
        public float cameraMaxX = 90;
        [SerializeField, Range(0, 180)]
        public float defaultFieldOfView = 75;
        [SerializeField, Range(0, 180)]
        public float runningFieldOfView = 90;
        [SerializeField, Range(0, 10)]
        public float fieldOfViewSmoothingTime = 1;

        [Header("Blow")]
        [SerializeField]
        public GameObject bubblePrefab;
        [SerializeField]
        public float minBlowVolume = 0.5f;
        [SerializeField]
        public AnimationCurve bubbleEjectScaling = AnimationCurve.Constant(0, 1, 1);
        [SerializeField]
        public float bubbleEjectSpeed = 10;
        [SerializeField]
        public float bubbleVelocityMultiplier = 1;

        [Header("Events")]
        [SerializeField]
        public UnityEvent<GameObject> onJumpCountChanged = new();
        [SerializeField]
        public UnityEvent<GameObject> onStep = new();
    }
}