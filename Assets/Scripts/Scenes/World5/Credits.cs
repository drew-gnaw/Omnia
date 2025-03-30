using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float resetPositionY = 1000f; // Adjust based on your UI
    [SerializeField] private float startPositionY = -500f; // Adjust based on your UI

    void Update()
    {
        // Move the credits panel up
        creditsPanel.transform.Translate(Vector3.up * (scrollSpeed * Time.deltaTime));

        // Optional: Reset position if it goes too high (for looping effect)
        if (creditsPanel.transform.localPosition.y >= resetPositionY)
        {
            creditsPanel.transform.localPosition = new Vector3(
                creditsPanel.transform.localPosition.x,
                startPositionY,
                creditsPanel.transform.localPosition.z
            );
        }
    }
}
