using UnityEngine;

public class InverseParallaxBackground : MonoBehaviour {
    private Vector3 lastCameraPosition;
    private Transform cameraTransform;

    [SerializeField, Range(0f, 1f)]
    private float parallaxFactor = 0.5f; // Controls how much it moves against the camera

    private void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position -= new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
