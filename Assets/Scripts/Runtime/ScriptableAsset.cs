using MyBox;
using UnityEngine;

namespace BubbleWand {
    public class ScriptableAsset : ScriptableObject {
        [SerializeField, ReadOnly]
        ScriptableAsset asset = default;
        protected virtual void OnValidate() {
            if (asset != this) {
                asset = this;
            }
        }
    }
}