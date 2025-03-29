using System.Collections;
using UnityEngine;

namespace NPC {
    public class Uncle : MonoBehaviour {
        [SerializeField] private Animator animator;

        [Tooltip("We require this field because the animation is fucked up and I MANUALLY shift the transform to keep it consistent :3")]
        [SerializeField] private GameObject graphics;


        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        public void Start() {
            Idle();
            StartCoroutine(foo());
        }

        private IEnumerator foo() {
            yield return new WaitForSeconds(3f);
            StartWalking();
            yield return new WaitForSeconds(3f);
            Idle();
            yield return new WaitForSeconds(3f);
            StartWalking();
            yield return new WaitForSeconds(3f);
            Idle();
            yield return new WaitForSeconds(3f);
            StartWalking();
            yield return new WaitForSeconds(3f);
            Idle();
            yield return new WaitForSeconds(3f);
            StartWalking();
            yield return new WaitForSeconds(3f);
            Idle();
            yield return new WaitForSeconds(3f);
            StartWalking();
            yield return new WaitForSeconds(3f);
            Idle();
        }

        public void StartWalking() {
            animator.SetBool(IsWalking, true);
            graphics.transform.position += new Vector3(0, 0.25f, 0);
        }

        public void Idle() {
            animator.SetBool(IsWalking, false);
            graphics.transform.position -= new Vector3(0, 0.25f, 0);
        }
    }
}
