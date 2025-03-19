using System.Collections;
using TMPro;
using UnityEngine;

namespace Utils {
    public class Typewriter {
        public static IEnumerator TypewriterEffect(TextMeshPro textMesh, string fullText, float delay) {
            textMesh.text = "";
            foreach (char c in fullText) {
                textMesh.text += c;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
