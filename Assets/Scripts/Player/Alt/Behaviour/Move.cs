using UnityEngine;

namespace Player.Alt.Behaviour {
    public class Move : IBehaviour {
        private readonly Player self;

        public Move(Player self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.moving.x * 7, self.rb.velocity.y);
        }

        public void OnUpdate() {
            if (Fall.Check(self)) self.UseBehaviour(new Fall(self));
            else if (Jump.Check(self)) self.UseBehaviour(new Jump(self));
        }

        public static bool Check(Player it) {
            return it.grounded && it.checks[1].IsTouchingLayers(it.ground);
        }
    }
}
