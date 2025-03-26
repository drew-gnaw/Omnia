using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Players {
    public class PlayerDataManager : PersistentSingleton<PlayerDataManager> {
        public int playerSelectedWeapon = 0;

        public void OnEnable() {
            Player.OnWeaponChanged += HandleWeaponChange;
        }

        public void OnDisable() {
            Player.OnWeaponChanged -= HandleWeaponChange;
        }


        // Trinket data


        // Current weapon

        public void HandleWeaponChange(int targetWeapon) {
            playerSelectedWeapon = targetWeapon;
        }






    }
}
