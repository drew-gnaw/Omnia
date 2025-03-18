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

        public static Vector2 Lerpish(Vector2 a, Vector2 b, float t, float eps = 0.01f) {
            var x = Vector2.Lerp(a, b, t);
            return Vector2.Distance(x, b) < eps ? b : x;
        }
    }
}
