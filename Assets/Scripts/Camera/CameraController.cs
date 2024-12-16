using Cinemachine;
using Players;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine camera
    [SerializeField] private Player player;                         // Reference to the player
    [SerializeField] private float aimOffsetDistance = 3f;          // Distance to offset the camera
    [SerializeField] private float smoothSpeed = 5f;                // Smooth transition speed for the offset

    private CinemachineFramingTransposer framingTransposer;         // Framing Transposer component
    private Vector3 defaultOffset;                                  // The default camera offset
    private Vector3 currentOffset;

    void Start()
    {
        // Get the Framing Transposer component from the virtual camera
        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if (framingTransposer == null)
        {
            Debug.LogError("Framing Transposer is missing on the Cinemachine Virtual Camera.");
        }
        else
        {
            // Store the default offset
            defaultOffset = framingTransposer.m_TrackedObjectOffset;
        }
    }

    void LateUpdate()
    {
        if (player == null || framingTransposer == null) return;

        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            // Calculate the desired offset based on the player's facing direction
            Vector3 desiredOffset = (Vector3)player.facing.normalized * aimOffsetDistance;

            // Smoothly interpolate to the new offset
            currentOffset = Vector3.Lerp(currentOffset, desiredOffset, smoothSpeed * Time.deltaTime);

            // Apply the offset to the Framing Transposer's tracked object offset
            framingTransposer.m_TrackedObjectOffset = new Vector3(currentOffset.x, currentOffset.y, framingTransposer.m_TrackedObjectOffset.z);
        }
        else
        {
            // Smoothly return to the default offset when the right mouse button is released
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                defaultOffset,
                smoothSpeed * Time.deltaTime
            );
        }
    }
}
