using System.Collections;
using UnityEngine;

namespace Player.Alt.Mixin {
    public class UseInput : MonoBehaviour {
        [SerializeField] internal Player self;
        [SerializeField] internal float delay;

        private Coroutine co;
        private WaitForSeconds wait;

        public void Awake() {
            wait = new WaitForSeconds(delay);
        }

        public void Update() {
            var holdable = self.grounded || self.slide.x != 0;
            var held = Input.GetButton("Jump");
            var jump = Input.GetButtonDown("Jump");

            self.facing = GetFacingInput(self);
            self.moving = GetMovingInput();
            self.jump = self.jump || jump;
            self.held = self.jump && holdable || self.held && held;
        }

        public void OnEnable() {
            co = self.StartCoroutine(DoReset());
        }

        public void OnDisable() {
            self.StopCoroutine(co);
        }

        private IEnumerator DoReset() {
            while (self) {
                while (!self.jump) yield return null;
                yield return wait;

                self.jump = false;
            }
        }

        private static Vector2 GetMovingInput() {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        private static Vector2 GetFacingInput(Player it) {
            return it.cam.ScreenToWorldPoint(Input.mousePosition) - it.sprite.transform.position;
        }
    }
}
