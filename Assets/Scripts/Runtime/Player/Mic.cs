using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace BubbleWand.Player {
    [InputControlLayout(displayName = "Mic", stateType = typeof(MicVolumeState))]
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class Mic : InputDevice, IInputStateCallbackReceiver {

        static Mic() {
            InputSystem.RegisterLayout<Mic>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void InitializeInPlayer() { }

        public AxisControl volume { get; private set; }

        const float CLIP_GAIN = 1;

        protected override void OnRemoved() {
            base.OnRemoved();

            Cleanup();
        }

#if UNITY_WEBGL
        uMicrophoneWebGL.MicrophoneWebGL mic;

        void UpdateVolume() {
            if (mic) {
                return;
            }

            mic = new GameObject(nameof(uMicrophoneWebGL.MicrophoneWebGL)).AddComponent<uMicrophoneWebGL.MicrophoneWebGL>();

            mic.dataEvent.AddListener(HandleData);
        }

        void Cleanup() {
            if (mic) {
                UnityEngine.Object.Destroy(mic.gameObject);
                mic = null;
            }
        }
#else
        AudioClip clip;
        float[] clipData;

        void UpdateVolume() {
            if (!clip) {
                clip = Microphone.Start(null, true, 1, 44100);
                clipData = new float[clip.samples * clip.channels];
            }

            if (clip.GetData(clipData, 0)) {
                HandleData(clipData);
            }
        }

        void Cleanup() {
            if (clip) {
                Microphone.End(null);
                UnityEngine.Object.Destroy(clip);
                clip = null;
            }
        }
#endif

        void HandleData(float[] clipData) {
            float max = 0f;
            for (int i = 0; i < clipData.Length; i++) {
                float sample = Mathf.Abs(clipData[i]);
                sample *= CLIP_GAIN;
                max = Mathf.Max(max, sample);
            }

            currentVolume = Mathf.Clamp01(max);
        }

        float currentVolume = 0;

        protected override void FinishSetup() {
            base.FinishSetup();

            volume = GetChildControl<AxisControl>(nameof(volume));
        }

        public void OnNextUpdate() {
            UpdateVolume();

            InputState.Change(volume, currentVolume, InputState.currentUpdateType);
        }

        public unsafe void OnStateEvent(InputEventPtr eventPtr) {
        }

        public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset) {
            return false;
        }
    }
}