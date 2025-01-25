using UnityEngine;
using UnityEngine.VFX;

namespace BubbleWand.Environment {
    sealed class BlowableVisualEffect : ComponentFeature<VisualEffect>, IAirReceiver {
        [SerializeField]
        float airToActivate = 1;
        [SerializeField]
        string eventToSet = "TriggerBlowEvent";
        [SerializeField]
        string directionToSet = "BlowDirection";

        float air;

        public void Receive(Vector3 direction, float magnitude) {
            air += magnitude;

            if (air > airToActivate) {
                observedCompopnent.SetBool(eventToSet, true);
                observedCompopnent.SetVector3(directionToSet, direction);
                Destroy(this);
            }
        }
    }
}
