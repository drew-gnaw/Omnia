using System.Collections;
using Background;
using Initializers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;


namespace Scenes {
    public class LevelSelect : MonoBehaviour
    {
        [SerializeField] private GameObject dustParent;

        private Image[] dustImages;

        private void Start() {
            dustImages = dustParent.GetComponentsInChildren<Image>();

            foreach (var img in dustImages) {
                img.gameObject.AddComponent<FloatingImage>();
            }

            DisablePersistentSingletons.DisableHUD();
            DisablePersistentSingletons.DisableInventory();
            DisablePersistentSingletons.DisablePause();
        }

        public void Back() {
            SceneManager.LoadScene("1_Title");
        }


        public void GoToWorld(int worldNumber) {
            switch (worldNumber) {
                case 0:
                    SceneInitializer.LoadScene("2_Opening");
                    break;
                case 1:
                    SceneInitializer.LoadScene("W-1-2");
                    break;
                case 2:
                    SceneInitializer.LoadScene("W-2-1");
                    break;
                case 3:
                    SceneInitializer.LoadScene("W-3-1");
                    break;
                case 4:
                    SceneInitializer.LoadScene("W-4-1");
                    break;
                case 5:
                    SceneInitializer.LoadScene("W-B-B");
                    break;
                default:
                    Debug.Log("Invalid world number");
                    SceneInitializer.LoadScene("1_Title");
                    break;
            }
        }

    }


}
