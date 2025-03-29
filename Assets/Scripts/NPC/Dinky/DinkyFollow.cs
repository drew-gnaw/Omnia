using System;
using UnityEngine;

namespace NPC.Dinky {

    [RequireComponent(typeof(Dinky))]
    public class DinkyFollow : MonoBehaviour
    {
        [SerializeField] private Transform player; // Reference to the player
        [SerializeField] private float followDistance = 2f; // Distance to maintain behind the player
        [SerializeField] private float moveSpeed = 2f; // Speed at which Dinky follows

        private Dinky dinky;
        private bool isWalking = false;
        private Vector3 targetPosition;

        private float initialY;

        private void Awake()
        {
            dinky = GetComponent<Dinky>();

            // If player reference isn't assigned, try to find the player automatically
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
        }

        private void Start() {
            initialY = transform.position.y;
        }

        private void Update()
        {
            if (player != null)
            {
                FollowPlayer();
            }
        }

        private void FollowPlayer()
        {
            // Calculate the distance from Dinky to the player
            float distanceToPlayer = player.position.x - transform.position.x;

            // If Dinky is too far from the player, start moving
            if (Mathf.Abs(distanceToPlayer) > followDistance)
            {
                // Set target position
                targetPosition = player.position - new Vector3(followDistance * Mathf.Sign(distanceToPlayer), player.position.y - initialY, 0);

                // Flip Dinky to face the correct direction based on movement
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Sign(targetPosition.x - transform.position.x) * Mathf.Abs(scale.x);
                transform.localScale = scale;

                // Move Dinky towards the target position smoothly
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Trigger walking animation if Dinky is moving
                if (!isWalking)
                {
                    dinky.animator.SetTrigger(Dinky.WalkTrigger);  // Set walk animation trigger
                    isWalking = true;
                }
            }
            else
            {
                // Stop walking and trigger idle animation if Dinky is close enough
                if (isWalking)
                {
                    dinky.animator.SetTrigger(Dinky.IdleTrigger);  // Set idle animation trigger
                    isWalking = false;
                }
            }
        }
    }

}
