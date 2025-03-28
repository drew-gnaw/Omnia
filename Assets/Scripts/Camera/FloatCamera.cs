using UnityEngine;

public class FloatCamera : MonoBehaviour {
    [SerializeField] private float moveSpeed = 2f;  // Speed of movement
    [SerializeField] private float moveDistance = 5f;  // How far to move

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = true;

    void Start() {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * moveDistance;
    }

    void Update() {
        if (isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f) {
                transform.position = targetPosition; // Snap to the exact position
                isMoving = false; // Stop moving
            }
        }
    }
}
