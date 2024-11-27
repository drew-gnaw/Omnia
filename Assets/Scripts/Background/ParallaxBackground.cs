using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player’s transform
    [SerializeField] private float parallaxEffectMultiplier = 0.5f; // Speed of the parallax effect

    private Vector3 lastPlayerPosition;

    private void Start()
    {
        // Initialize the last position to the player's initial position
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        // Calculate the difference in player movement
        Vector3 deltaMovement = player.position - lastPlayerPosition;

        // Move the background by a fraction of the player's movement
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0, 0);

        // Update the last position for the next frame
        lastPlayerPosition = player.position;
    }
}