using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class Move : IBehaviour {
        private readonly Player self;

        private Move(Player self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * 5, Time.fixedDeltaTime * 10);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Fall.If(self) ?? Jump.If(self));
        }

        public static IBehaviour AsDefaultOf(Player it) {
            return new Move(it);
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.checks[1].IsTouchingLayers(it.ground) ? new Move(it) : null;
        }
    }
}
