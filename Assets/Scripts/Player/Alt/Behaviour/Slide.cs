using System;
using UnityEngine;
using Utils;

namespace Player.Alt.Behaviour {
    public class Slide : IBehaviour {
        private readonly Player self;

        private Slide(Player self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            var x = MathUtils.Lerpish(self.rb.velocity.x, self.moving.x * 5, Time.fixedDeltaTime * 20);
            self.rb.velocity = new Vector2(x, Math.Max(-1, self.rb.velocity.y));
        }

        public void OnUpdate() {
            self.UseBehaviour(WallJump.If(self) ?? Fall.If(self) ?? Move.If(self));
        }

        public static IBehaviour If(Player it) {
            return !it.grounded && it.slide.x != 0 && it.checks[it.slide.x > 0 ? 0 : 2].IsTouchingLayers(it.ground) ? new Slide(it) : null;
        }
    }
}
