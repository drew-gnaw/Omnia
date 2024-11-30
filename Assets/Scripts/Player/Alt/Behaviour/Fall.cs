using UnityEngine;

namespace Player.Alt.Behaviour {
    public class Fall : IBehaviour {
        private readonly Player self;

        public Fall(Player self) {
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
            if (Slide.Check(self)) self.UseBehaviour(new Slide(self));
            else if (Check(self)) {
            } //
            else if (Move.Check(self)) self.UseBehaviour(new Move(self));
        }

        public static bool Check(Player it) {
            return !it.grounded && it.rb.velocity.y < 1;
        }
    }
}
