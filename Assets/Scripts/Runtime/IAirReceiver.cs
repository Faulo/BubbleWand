using UnityEngine;

namespace BubbleWand {
    public interface IAirReceiver {
        void Receive(Vector3 direction, float magnitude);
    }
}
