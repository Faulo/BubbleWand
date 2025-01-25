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

        const int CLIP_DURATION = 1;
        const int CLIP_SAMPLE_DIVIDER = 8;
        const int CLIP_FREQUENCY = 44100;
        const float CLIP_MAX_VOLUME = 1f;

        AudioClip clip;
        float[] clipData;

        float GetMicrophoneVolume() {
            if (!clip) {
                return 0;
            }

            clipData ??= new float[clip.samples * clip.channels];

            if (!clip.GetData(clipData, 0)) {
                return 0;
            }

            float max = 0f;
            for (int i = 0; i < clipData.Length / CLIP_SAMPLE_DIVIDER; i++) {
                float sample = clipData[i] * clipData[i];
                max = Mathf.Max(max, sample / CLIP_MAX_VOLUME);
            }

            return Mathf.Clamp01(max);
        }

        protected override void FinishSetup() {
            base.FinishSetup();

            volume = GetChildControl<AxisControl>(nameof(volume));

            clip = Microphone.Start(null, true, CLIP_DURATION, CLIP_FREQUENCY);
        }

        public void OnNextUpdate() {
            InputState.Change(volume, GetMicrophoneVolume(), InputState.currentUpdateType);
        }

        public unsafe void OnStateEvent(InputEventPtr eventPtr) {
        }

        public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset) {
            return false;
        }
    }
}