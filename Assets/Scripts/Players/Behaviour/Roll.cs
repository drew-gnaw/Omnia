using UnityEngine;

namespace Players.Behaviour {
    public class Roll : IBehaviour {
        private static IBehaviour _s;
        private readonly Player self;
        private float t;

        private Roll(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.rollDuration;
            self.roll = false;

            self.UseAnimation("PlayerSlide");

        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.rollSpeed, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        public static IBehaviour AdHoc(Player it) {
            return _s ??= new Roll(it);
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.checks[1].IsTouchingLayers(it.ground | it.semisolid) ? AdHoc(it) : null;
        }
    }
}
