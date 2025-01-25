using BubbleWand.Player;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleWand.UI {
    sealed class BlowDisplay : MonoBehaviour {
        [SerializeField]
        Slider target;

        void Update() {
            if (target) {
                target.value = Blow.value;
            }
        }
    }
}
