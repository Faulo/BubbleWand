using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace BubbleWand.Player {
    public struct MicVolumeState : IInputStateTypeInfo {
        public FourCC format => new('M', 'I', 'C', 'V');

        [InputControl(layout = "Axis")]
        public float volume;
    }
}