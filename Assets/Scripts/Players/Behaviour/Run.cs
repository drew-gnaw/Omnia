using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class Run : IBehaviour {
        private readonly Player self;

        private Run(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerRun");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * self.moveSpeed, Time.fixedDeltaTime * 10);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Fall.If(self) ?? Jump.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.moving.x != 0 && it.checks[1].IsTouchingLayers(it.ground) ? new Run(it) : null;
        }
    }
}
