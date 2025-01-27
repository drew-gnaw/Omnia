using UnityEngine;

namespace Players.Behaviour {
    public class Idle : IBehaviour {
        private static IBehaviour _s;
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
            var x = self.HorizontalVelocityOf(0, Time.fixedDeltaTime * self.moveAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Fall.If(self) ?? Jump.If(self) ?? Move.If(self) ?? Roll.If(self));
        }

        public static IBehaviour AdHoc(Player it) {
            return _s ??= new Idle(it);
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.moving.x == 0 && it.checks[1].IsTouchingLayers(it.ground | it.semisolid) ? AdHoc(it) : null;
        }
    }
}
