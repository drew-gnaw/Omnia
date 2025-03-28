using System;
using Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Players {
    public class PlayerDataManager : PersistentSingleton<PlayerDataManager> {
        [SerializeField] private Trinket[] trinkets;

        public void Start() {
            foreach (Trinket trinket in trinkets) {
                trinket.isLocked = true;
            }
        }

        public int playerSelectedWeapon = 0;
        public void OnEnable() {
            Player.OnWeaponChanged += HandleWeaponChange;
        }

        public void OnDisable() {
            Player.OnWeaponChanged -= HandleWeaponChange;
        }


        // Trinket data

        public void AddTrinket(Trinket trinket) {
            Debug.Log("Searching for: " + trinket.name);
            foreach (var t in trinkets) {
                Debug.Log(t.name);
                if (t.trinketName == trinket.trinketName) {
                    t.isLocked = false;
                    return;
                }
            }
        }



        // Current weapon

        public void HandleWeaponChange(int targetWeapon) {
            playerSelectedWeapon = targetWeapon;
        }






    }
}
