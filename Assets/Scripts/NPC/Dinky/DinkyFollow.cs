using System.Collections;
using UnityEngine;

namespace NPC.Dinky {

    [RequireComponent(typeof(Dinky))]
    public class DinkyFollow : MonoBehaviour
    {
        [SerializeField] private Transform player; // Reference to the player
        [SerializeField] private float followDistance = 2f; // Distance to maintain behind the player
        [SerializeField] private float moveSpeed = 2f; // Speed at which Dinky follows

        private Dinky dinky;
        private Coroutine followCoroutine;

        private void Awake()
        {
            dinky = GetComponent<Dinky>();

            // If player reference isn't assigned, try to find the player automatically
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
        }

        private void Start()
        {
            if (player != null)
            {
                followCoroutine = StartCoroutine(FollowPlayer());
            }
        }

        private IEnumerator FollowPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f); // Small delay to optimize performance

                float distanceToPlayer = player.position.x - transform.position.x;

                // Only move if the distance is greater than the desired follow distance
                if (Mathf.Abs(distanceToPlayer) > followDistance)
                {
                    float moveDistance = distanceToPlayer - (followDistance * Mathf.Sign(distanceToPlayer));
                    dinky.Walk(moveDistance, moveSpeed);
                }
            }
        }
    }

}
