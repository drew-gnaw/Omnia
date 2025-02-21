using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Initializers {
    public class SceneInitializer : PersistentSingleton<SceneInitializer> {
        [SerializeField] internal GameObject[] managers;
        [SerializeField] internal GameObject transition;

        public void OnEnable() {
            SceneManager.sceneLoaded += OnSceneChange;
        }

        public void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneChange;
        }

        protected override void OnAwake() {
        }

        private void OnSceneChange(Scene scene, LoadSceneMode mode) {
            foreach (var manager in managers) {
                Instantiate(manager);
            }
        }

        private IEnumerator DoLoadSceneWithTransition(string sceneName) {
            var it = Instantiate(transition);
            DontDestroyOnLoad(it);
            var st = it.GetComponent<SceneTransition>();

            yield return st.DoTransitionToBlack();
            SceneManager.LoadScene(sceneName);
            yield return st.DoTransitionToScene();

            Destroy(it);
        }

        /**
         * Load the scene with a transition effect to hide any stutters.
         */
        public static void LoadScene(string name) {
            Instance.StartCoroutine(Instance.DoLoadSceneWithTransition(name));
        }
    }
}
