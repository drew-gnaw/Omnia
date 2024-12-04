using System;
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
            self.jump = false;
            t = 0.1f;
            self.rb.velocity = new Vector2(self.slide.x * self.moveSpeed * -1, self.jumpForce);
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * self.moveSpeed, (self.wallJumpLockoutTime - t) / self.wallJumpLockoutTime * Time.fixedDeltaTime * self.C);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Math.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Move.If(self));
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x != 0 && it.jump ? new WallJump(it) : null;
        }
    }
}
