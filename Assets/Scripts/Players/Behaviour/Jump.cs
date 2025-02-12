using UnityEngine;

namespace Players.Behaviour {
    public class Jump : IBehaviour {
        private readonly Player self;
        private float t;

        public Jump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.jumpLockoutTime;

            self.UseExternalVelocity(new Vector2(self.rb.velocity.x, self.jumpSpeed), 0);
            self.jump = false;
            self.UseAnimation("PlayerJump");
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (self.IsPhoon()) return;

            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.fallAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.jump ? new Jump(it) : null;
        }
    }
}
