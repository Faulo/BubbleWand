using TMPro;
using UnityEngine;

namespace BubbleWand.UI {
    [ExecuteAlways]
    sealed class ProductDisplay : MonoBehaviour {
        [SerializeField]
        TMP_Text target;

        void Update() {
            if (target) {
                target.text = Application.productName;
            }
        }
    }
}
