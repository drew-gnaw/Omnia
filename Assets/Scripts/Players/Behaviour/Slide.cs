using UnityEngine;
using Utils;

namespace Players.Behaviour {
    public class Slide : IBehaviour {
        private static IBehaviour _s;
        private readonly Player self;

        private Slide(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerSlide");
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.moveAccel);
            self.rb.velocity = new Vector2(x, Mathf.Max(self.jumpSpeed / 2 * -1, self.rb.velocity.y));
        }

        public void OnUpdate() {
            self.UseBehaviour(WallJump.If(self) ?? Fall.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        private static IBehaviour AdHoc(Player it) {
            return _s ??= new Slide(it);
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x != 0 && it.checks[it.slide.x > 0 ? 0 : 2].IsTouchingLayers(it.ground) ? AdHoc(it) : null;
        }
    }
}
