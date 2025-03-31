using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float endPositionY = 1500f;

    [SerializeField] private FadeScreenHandler fadeScreen;

    void Start() {
        fadeScreen.SetDarkScreen();
        StartCoroutine(fadeScreen.FadeInLightScreen(2f));

        DisablePersistentSingletons.DisableHUD();
        DisablePersistentSingletons.DisableInventory();
        DisablePersistentSingletons.DisablePause();
    }

    void Update()
    {
        creditsPanel.transform.Translate(Vector3.up * (scrollSpeed * Time.deltaTime));

        if (creditsPanel.transform.localPosition.y >= endPositionY) {
            StartCoroutine(EndSequence());
        }
    }

    private IEnumerator EndSequence() {
        yield return fadeScreen.FadeInDarkScreen(3f);
        SceneManager.LoadScene("1_Title");
    }
}
