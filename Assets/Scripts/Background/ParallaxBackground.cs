using UnityEngine;

public class ParallaxBackground : MonoBehaviour {
    private Vector3 lastCameraPosition;
    private Transform cameraTransform;

    [SerializeField, Range(0f, 1f)]
    private float parallaxFactor = 0.5f; // 0 = Static, 1 = Moves with Camera

    private void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxFactor;
        lastCameraPosition = cameraTransform.position;
    }
}
