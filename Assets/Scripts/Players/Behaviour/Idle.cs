using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class Idle : IBehaviour {
        private readonly Player self;

        private Idle(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerIdle");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, 0, Time.fixedDeltaTime * 10);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Fall.If(self) ?? Jump.If(self) ?? Run.If(self));
        }

        public static IBehaviour AsDefaultOf(Player it) {
            return new Idle(it);
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.moving.x == 0 && it.checks[1].IsTouchingLayers(it.ground) ? new Idle(it) : null;
        }
    }
}
