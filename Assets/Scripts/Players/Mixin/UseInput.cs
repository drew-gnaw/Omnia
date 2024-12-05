using UnityEngine;

namespace Players.Mixin {
    public class UseInput : MonoBehaviour {
        private static readonly string[] Keys = { "Horizontal", "Vertical", "Fire1", "Jump" };

        [SerializeField] internal Player self;
        [SerializeField] internal float delay;

        private float jt;
        private float ft;

        public void Update() {
            var fire = Input.GetButtonDown(Keys[2]);
            var jump = Input.GetButtonDown(Keys[3]);
            var held = Input.GetButton(Keys[3]);

            ft = fire ? delay : Mathf.Max(0, ft - Time.deltaTime);
            jt = jump ? delay : Mathf.Max(0, jt - Time.deltaTime);
            self.fire = self.fire ? ft > 0 : fire;
            self.jump = self.jump ? jt > 0 : jump;

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
    }
}
