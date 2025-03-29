using System.Collections;
using UnityEngine;

namespace NPC {
    public class Uncle : MonoBehaviour {
        [SerializeField] private Animator animator;

        [Tooltip("We require this field because the animation is messed up, and I MANUALLY shift the transform to keep it consistent :3")]
        [SerializeField] private GameObject graphics;

        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private Vector3 initialPosition;

        // Start is called before the first frame update
        public void Start() {
            Idle();
            initialPosition = graphics.transform.position;  // Store the initial position for later use.
        }

        public void StartWalking() {
            animator.SetBool(IsWalking, true);
            graphics.transform.position += new Vector3(0, 0.25f, 0);  // Adjust position for animation consistency
        }

        public void Idle() {
            animator.SetBool(IsWalking, false);
            graphics.transform.position -= new Vector3(0, 0.25f, 0);  // Return position to idle state
        }

        // Walk method that moves the uncle along the X axis
        public void Walk(float x, float speed) {
            StartCoroutine(WalkCoroutine(x, speed));
        }

        private IEnumerator WalkCoroutine(float x, float speed) {
            StartWalking();
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + new Vector3(x, 0, 0);  // Calculate the target position

            float distance = Mathf.Abs(targetPosition.x - startPosition.x);  // Get the total distance to walk
            float duration = distance / speed;  // Calculate the time required to walk the distance at the specified speed

            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                // Smoothly move the object towards the target position based on elapsed time and the duration
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure that we set the final position exactly at the target position to avoid small inaccuracies
            transform.position = targetPosition;
            Idle();
        }
    }
}
