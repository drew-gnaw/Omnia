using UnityEngine;

namespace Players.Behaviour {
    public class Fall : IBehaviour {
        private static IBehaviour _s;
        private readonly Player self;

        private Fall(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerFall");
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (self.IsPhoon()) return;

            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.fallAccel);
            self.rb.velocity = new Vector2(x, Mathf.Max(self.jumpSpeed * 2 * -1, self.rb.velocity.y));
        }

        public void OnUpdate() {
            self.UseBehaviour(Slide.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        public static IBehaviour AdHoc(Player it) {
            return _s ??= new Fall(it);
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x == 0 && it.rb.velocity.y <= 0 ? AdHoc(it) : null;
        }
    }
}
