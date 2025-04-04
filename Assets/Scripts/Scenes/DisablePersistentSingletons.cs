using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes {
    public static class DisablePersistentSingletons {
        private static List<GameObject> disabledManagers = new List<GameObject>();
        private static bool isListening = false;

        static DisablePersistentSingletons() {
            if (!isListening) {
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                isListening = true;
            }
        }

        public static void DisableManager(GameObject manager) {
            if (manager != null && manager.activeSelf) {
                manager.SetActive(false);
                disabledManagers.Add(manager);
            }
        }
        public static void DisableScreenShakeManager() {
            DisableManager(ScreenShakeManager.Instance?.gameObject);
        }

        public static void DisableHUD() {
            DisableManager(HUDManager.Instance?.gameObject);
        }

        public static void DisableInventory() {
            DisableManager(InventoryManager.Instance?.gameObject);
        }

        public static void DisablePause() {
            DisableManager(PauseMenu.Instance?.gameObject);
        }

        private static void OnSceneUnloaded(Scene scene) {
            ReEnableDisabledManagers();
        }

        private static void ReEnableDisabledManagers() {
            foreach (var manager in disabledManagers) {
                if (manager != null) {
                    manager.SetActive(true);
                }
            }
            disabledManagers.Clear();
        }
    }
}
