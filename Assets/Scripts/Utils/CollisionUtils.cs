using UnityEngine;

namespace Omnia.Utils
{
    public static class CollisionUtils {
        public static bool isLayerInMask(int layer, LayerMask mask) {
            return ((1 << layer) & mask) != 0;
        }
    }
}