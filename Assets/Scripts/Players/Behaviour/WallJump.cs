using UnityEngine;

namespace Players.Behaviour {
    public class WallJump : IBehaviour {
        private static IBehaviour _s;

        private readonly Player self;
        private float t;

        private WallJump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.jumpLockoutTime;

            self.UseExternalVelocity(new Vector2(self.slide.x * self.moveSpeed * -1, self.jumpSpeed), self.jumpLockoutTime);
            self.jump = false;
            self.UseAnimation("PlayerJump");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.fallAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        private static IBehaviour AdHoc(Player it) {
            return _s ??= new WallJump(it);
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x != 0 && it.jump ? AdHoc(it) : null;
        }
    }
}
