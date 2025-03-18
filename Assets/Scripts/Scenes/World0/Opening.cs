using System;
using System.Collections;
using Background;
using TMPro;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Opening : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;
        [SerializeField] private PanBackground panBackground;
        [SerializeField] private Transform panTarget;

        [SerializeField] private TextMeshPro Text1;
        [SerializeField] private TextMeshPro Text2;

        private string text1;
        private string text2;
        public void Start() {
            text1 = Text1.text;
            text2 = Text2.text;
            Text1.text = "";
            Text2.text = "";
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            panBackground.PanTo(panTarget, 20f);
            yield return Typewriter.TypewriterEffect(Text1, text1, 0.05f);
        }



        public void Update() {
            Debug.Log(Text1.text);
        }
    }
}
