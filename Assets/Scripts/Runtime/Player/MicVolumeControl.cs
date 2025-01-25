using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace BubbleWand.Player {
    [InputControlLayout(stateType = typeof(MicVolumeState))]
    public class MicVolumeControl : AxisControl {

        protected override void FinishSetup() {
            base.FinishSetup();
        }
    }
}