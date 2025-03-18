using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Scenes {
    public class DinkyWasted : MonoBehaviour {
        [SerializeField] private DialogueWrapper beginDialogue;
        public void Start() {
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));
        }

    }
}
