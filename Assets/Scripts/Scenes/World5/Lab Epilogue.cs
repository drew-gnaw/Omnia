using System.Collections;
using Players;
using Players.Mixin;
using UnityEngine;

namespace Scenes.World5 {
    public class Lab_Epilogue : MonoBehaviour {
        [SerializeField] private GameObject dinky;
        [SerializeField] private GameObject boss;
        [SerializeField] private Animator bossAnimator;

        [SerializeField] DialogueWrapper beginDialogue;
        public void Start() {
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return new WaitForSeconds(0.5f);


        }


    }

}
