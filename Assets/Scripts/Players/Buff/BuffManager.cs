using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Players.Buff {
    public class BuffManager : PersistentSingleton<BuffManager> {
        private Player player;
        private List<Buff> activeBuffs = new List<Buff>();

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
            ReapplyBuffs();
        }

        public Buff ApplyBuff(Buff buffPrefab) {
            if (player == null) {
                Debug.LogWarning("Player reference is missing, cannot apply buff.");
                return null;
            }

            Debug.Log($"Applying buff {buffPrefab.name}");

            // Instantiate the buff under this manager for persistence.
            Buff buffInstance = Instantiate(buffPrefab, gameObject.transform);
            buffInstance.Initialize(player);
            buffInstance.ApplyBuff();

            activeBuffs.Add(buffInstance);
            return buffInstance;
        }

        public void RemoveBuff(Buff buffInstance) {
            if (player == null) {
                Debug.LogWarning("Player reference is missing, cannot remove buff.");
                return;
            }

            buffInstance.RevokeBuff();
            activeBuffs.Remove(buffInstance);
            Destroy(buffInstance.gameObject);
        }

        // Required because the player was destroyed and reinstantiated on scene load, losing buff properties.
        public void ReapplyBuffs() {
            foreach (Buff buff in activeBuffs) {
                buff.Initialize(player);
                buff.ApplyBuff();
            }
        }
    }
}
