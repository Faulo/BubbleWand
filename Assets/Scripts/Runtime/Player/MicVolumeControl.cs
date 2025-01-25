using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace BubbleWand.Player {
    [InputControlLayout(stateType = typeof(MicVolumeState))]
    public class MicVolumeControl : AxisControl {
        float maxVolume = 1.0f; // Maximal erwarteter Dezibel-Wert

        AudioClip micClip;

        protected override void FinishSetup() {
            base.FinishSetup();

            micClip = Microphone.Start(null, true, 1, 44100);
            Debug.Log(micClip);
        }

        public void Update() {
            float volume = GetMicrophoneVolume();
            Debug.Log(volume);
            //WriteValueIntoState(volume);
        }

        float GetMicrophoneVolume() {
            float[] data = new float[256];
            if (micClip == null) {
                return 0f;
            }

            micClip.GetData(data, 0);
            float sum = 0f;
            foreach (float sample in data) {
                sum += sample * sample;
            }

            float rmsValue = Mathf.Sqrt(sum / data.Length);
            return Mathf.Clamp01(rmsValue / maxVolume);
        }
    }
}