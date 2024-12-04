using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class Jump : IBehaviour {
        private readonly Player self;
        private float t;

        private Jump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = 0.1f;

            self.rb.velocity = new Vector2(self.moving.x * self.moveSpeed, self.jumpForce);
            self.jump = false;
            self.UseAnimation("PlayerJump");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.moveSpeed);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Run.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.jump ? new Jump(it) : null;
        }
    }
}
