using UnityEngine;

namespace Utils {
    public static class MathUtils {
        public static float Lerpish(float a, float b, float t, float eps = 0.01f) {
            var x = Mathf.Lerp(a, b, t);
            return Mathf.Abs(x - b) < eps ? b : x;
        }

        public static int LayerIndexOf(LayerMask it) {
            return Mathf.FloorToInt(Mathf.Log(it, 2));
        }
    }
}
