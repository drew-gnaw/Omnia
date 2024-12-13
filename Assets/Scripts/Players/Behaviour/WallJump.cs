using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class WallJump : IBehaviour {
        private readonly Player self;
        private float t;

        private WallJump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.jumpLockoutTime;

            self.rb.velocity = new Vector2(self.slide.x * self.moveSpeed * -1, self.jumpSpeed);
            self.jump = false;
            self.UseAnimation("PlayerJump");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var ratio = 1 - t / self.jumpLockoutTime;

            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * self.moveSpeed, ratio * Time.fixedDeltaTime * self.jumpAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Run.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x != 0 && it.jump ? new WallJump(it) : null;
        }
    }
}
