using UnityEngine;
using Utils;

namespace Player.Alt.Behaviour {
    public class Fall : IBehaviour {
        private readonly Player self;

        private Fall(Player self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * 5, Time.fixedDeltaTime * 5);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.UseBehaviour(Slide.If(self) ?? Move.If(self));
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x == 0 && it.rb.velocity.y < 0 ? new Fall(it) : null;
        }
    }
}
