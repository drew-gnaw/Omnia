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

        public static float RoundX(float value) {
            if (value >= 0.5) return 1;
            if (value <= -0.5) return -1;
            return 0;
        }
    }
}
