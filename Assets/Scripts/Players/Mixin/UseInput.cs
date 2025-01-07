using System.Collections.Generic;
using UnityEngine;

namespace Players.Mixin {
    public class UseInput : MonoBehaviour {
        private static readonly string[] Keys = { "Horizontal", "Vertical", "Fire1", "Jump", "Fire2"};

        [SerializeField] internal Player self;
        [SerializeField] internal float delay;

        private readonly Dictionary<KeyCode, int> weaponKeyMap = new Dictionary<KeyCode, int> {
            { KeyCode.Alpha1, 0 }, // 1 key -> Harpoon Gun
            { KeyCode.Alpha2, 1 }, // 2 key -> Shotgun
        };

        private float jt;
        private float ft;
        private float skt;
        private float swt;

        public void Update() {
            var fire = Input.GetButtonDown(Keys[2]);
            var jump = Input.GetButtonDown(Keys[3]);
            var held = Input.GetButton(Keys[3]);
            var skill = Input.GetButtonDown(Keys[4]);

            foreach (var kvp in weaponKeyMap) {
                if (Input.GetKeyDown(kvp.Key)) {
                    SwapWeapon(kvp.Value);
                }
            }

            ft = fire ? delay : Mathf.Max(0, ft - Time.deltaTime);
            jt = jump ? delay : Mathf.Max(0, jt - Time.deltaTime);
            skt = skill ? delay : Mathf.Max(0, skt - Time.deltaTime);

            self.fire = self.fire ? ft > 0 : fire;
            self.jump = self.jump ? jt > 0 : jump;
            self.skill = self.skill ? skt > 0 : skill;

            self.facing = GetFacingInput(self);
            self.moving = GetMovingInput();
            self.held = self.jump && (self.grounded || self.slide.x != 0) || self.held && held;
        }

        private static Vector2 GetMovingInput() {
            return new Vector2(Input.GetAxisRaw(Keys[0]), Input.GetAxisRaw(Keys[1]));
        }

        private static Vector2 GetFacingInput(Player it) {
            return it.cam.ScreenToWorldPoint(Input.mousePosition) - it.sprite.transform.position;
        }

        private void SwapWeapon(int weaponIndex) {
            if (self.selectedWeapon != weaponIndex) {
                self.selectedWeapon = weaponIndex;
                Debug.Log($"Swapped to weapon {weaponIndex}");
            }
        }
    }
}
