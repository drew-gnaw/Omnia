using System;
using System.Collections.Generic;
using UnityEngine;

namespace Players.Mixin {
    public class UseInput : MonoBehaviour {
        private enum KeysEnum {
            Horizontal,
            Vertical,
            Fire1,
            Fire2,
            Fire3,
            Jump,
            Roll,
        }

        private static readonly Dictionary<KeysEnum, string> KeyMap = new Dictionary<KeysEnum, string> {
            { KeysEnum.Horizontal, "Horizontal" },
            { KeysEnum.Vertical, "Vertical" },
            { KeysEnum.Fire1, "Fire1" },
            { KeysEnum.Fire2, "Fire2" },
            { KeysEnum.Fire3, "Fire3" },
            { KeysEnum.Jump, "Jump" },
            { KeysEnum.Roll, "Roll" },
        };


        [SerializeField] internal Player self;
        [SerializeField] internal float delay;

        private float jt;
        private float ft;
        private float skt;
        private float swt;
        private float rlt;
        private float ist;

        public void Start() {
            InventoryManager.OnInventoryOpened += StopMoving;
        }

        public void Update() {
            if (Player.controlsLocked) {
                self.moving = Vector2.zero;
                return;
            }
            if (DialogueManager.Instance?.IsInDialogue() == true || InventoryManager.Instance?.IsInventoryOpen == true) return;
            if (PauseMenu.IsPaused) return;

            var fire = Input.GetButtonDown(KeyMap[KeysEnum.Fire1]);
            var jump = Input.GetButtonDown(KeyMap[KeysEnum.Jump]);
            var held = Input.GetButton(KeyMap[KeysEnum.Jump]);
            var skill = Input.GetButtonDown(KeyMap[KeysEnum.Fire2]);
            var roll = Input.GetButtonDown(KeyMap[KeysEnum.Roll]) && self.shoeEquipped;
            var intro = Input.GetButton(KeyMap[KeysEnum.Fire3]);

            ft = fire ? delay : Mathf.Max(0, ft - Time.deltaTime);
            jt = jump ? delay : Mathf.Max(0, jt - Time.deltaTime);
            skt = skill ? delay : Mathf.Max(0, skt - Time.deltaTime);
            rlt = roll ? delay : Mathf.Max(0, rlt - Time.deltaTime);
            ist = intro ? delay : Mathf.Max(0, ist - Time.deltaTime);

            self.fire = self.fire ? ft > 0 : fire;
            self.jump = self.jump ? jt > 0 : jump;
            self.skill = self.skill ? skt > 0 : skill;
            self.roll = self.roll ? rlt > 0 : roll;
            self.intro = self.intro ? ist > 0 : intro;

            self.facing = GetFacingInput(self);
            self.moving = GetMovingInput();
            self.held = self.jump && (self.grounded || self.slide.x != 0) || self.held && held;
        }

        public void OnDestroy() {
            InventoryManager.OnInventoryOpened -= StopMoving;
        }

        private static Vector2 GetMovingInput() {
            return new Vector2(Input.GetAxisRaw(KeyMap[KeysEnum.Horizontal]), Input.GetAxisRaw(KeyMap[KeysEnum.Vertical]));
        }

        private static Vector2 GetFacingInput(Player it) {
            return it.cam.ScreenToWorldPoint(Input.mousePosition) - it.sprite.transform.position;
        }

        private void StopMoving() {
            self.moving = Vector2.zero;
        }
    }
}
