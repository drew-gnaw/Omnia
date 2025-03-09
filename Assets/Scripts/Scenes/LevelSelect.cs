using System.Collections;
using Initializers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Scenes {
    public class LevelSelect : MonoBehaviour
    {
        public Button[] buttons;
        public GameObject levelButtons;

        public void OpenScene(string s)
        {
            SceneInitializer.LoadScene(s);
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
