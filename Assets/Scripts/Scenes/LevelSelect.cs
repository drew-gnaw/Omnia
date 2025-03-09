using System.Collections;
using Initializers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;


namespace Scenes {
    public class LevelSelect : MonoBehaviour
    {
        public Button[] buttons;
        public GameObject levelButtons;
        [SerializeField] protected FadeScreenHandler fadeScreen;

        protected virtual void Awake()
        {
            ButtonArray();
            fadeScreen.SetDarkScreen();
            StartCoroutine(fadeScreen.FadeInLightScreen(1f));
        }

        public void OpenScene(string s)
        {
            StartCoroutine(FadeLevelIn(s));
        }

        IEnumerator FadeLevelIn(string levelName)
        {
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(0.8f));
            SceneManager.LoadScene(levelName);
        }

        void ButtonArray()
        {
            int children = levelButtons.transform.childCount;
            buttons = new Button[children];
            for (int i = 0; i < children; i++)
            {
                buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
            }
        }
    }


}
