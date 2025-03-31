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

        public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f) {
            float u, v, S;

            do {
                u = 2.0f * Random.value - 1.0f;
                v = 2.0f * Random.value - 1.0f;
                S = u * u + v * v;
            } while (S >= 1.0f);

            // Standard Normal Distribution
            float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

            // Normal Distribution centered between the min and max value
            // and clamped following the "three-sigma rule"
            float mean = (minValue + maxValue) / 2.0f;
            float sigma = (maxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
        }

        public static Color Lerpish(Color a, Color b, float t, float eps = 0.01f) {
            var x = Color.Lerp(a, b, t);
            return Vector4.Distance(x, b) < eps ? b : x;
        }
    }
}
