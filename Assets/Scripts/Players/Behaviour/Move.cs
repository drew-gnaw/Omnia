using UnityEngine;

namespace Players.Behaviour {
    public class Move : IBehaviour {
        private readonly Player self;

        public Move(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerRun");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.moveAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Fall.If(self) ?? Jump.If(self) ?? Roll.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.moving.x != 0 && it.checks[1].IsTouchingLayers(it.ground | it.semisolid) ? new Move(it) : null;
        }
    }
}
