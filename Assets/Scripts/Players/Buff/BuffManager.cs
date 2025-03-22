using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Players.Buff {
    public class BuffManager : PersistentSingleton<BuffManager> {
        private Player player;
        private List<Buff> activeBuffs;

        protected override void OnAwake() {
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (player == null) {
                Debug.LogWarning("Player not found in the new scene.");
            }
        }

        public void ApplyBuff(Buff buff) {
            if (player == null) {
                Debug.LogWarning("Player reference is missing, cannot apply buff.");
                return;
            }

            buff.Initialize(player);
            buff.ApplyBuff();

            if (!activeBuffs.Contains(buff)) {
                activeBuffs.Add(buff);
            }
        }

        public void RemoveBuff(Buff buff) {
            if (player == null) {
                Debug.LogWarning("Player reference is missing, cannot remove buff.");
                return;
            }

            buff.RevokeBuff();

            if (activeBuffs.Contains(buff)) {
                activeBuffs.Remove(buff);
            }
        }

        // Optionally: You could create a method to reapply buffs across scene loads
        public void ReapplyBuffs() {
            if (player == null) {
                Debug.LogWarning("Player reference is missing, cannot reapply buffs.");
                return;
            }

            // Example logic: Reapply all active buffs
            foreach (Buff activeBuff in activeBuffs) {
                activeBuff.Initialize(player);
                activeBuff.ApplyBuff();
            }
        }
    }
}
