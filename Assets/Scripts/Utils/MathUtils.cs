using UnityEngine;

namespace Utils {
    public class MathUtils {
        public static float Lerpish(float a, float b, float t, float eps = 0.01f) {
            var x = Mathf.Lerp(a, b, t);
            return Mathf.Abs(x - a) < eps ? a : Mathf.Abs(x - b) < eps ? b : x;
        }
    }
}
