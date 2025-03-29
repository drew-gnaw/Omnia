using UnityEngine;
using System.Collections;

public class FloatCamera : MonoBehaviour {
    [SerializeField] private float moveSpeed;  // Speed of movement
    [SerializeField] private float moveDistance;  // How far to move

    private bool isMoving = false;

    public void StartFloating() {
        if (!isMoving) {
            Vector3 targetPosition = transform.position + Vector3.up * moveDistance; // Calculate dynamically
            StartCoroutine(FloatUp(targetPosition));
        }
    }

    private IEnumerator FloatUp(Vector3 targetPosition) {
        isMoving = true;

        yield return StartCoroutine(MoveToPosition(targetPosition));

        isMoving = false;
    }

    private IEnumerator MoveToPosition(Vector3 destination) {
        float elapsedTime = 0f;
        float totalDistance = Mathf.Abs(destination.y - transform.position.y);
        float duration = totalDistance / moveSpeed;

        Vector3 start = transform.position;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            float newY = Mathf.Lerp(start.y, destination.y, t);
            transform.position = new Vector3(start.x, newY, start.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(start.x, destination.y, start.z);
    }
}
