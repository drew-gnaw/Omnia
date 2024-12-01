using System;
using UnityEngine;

namespace Player.Alt.Behaviour {
    public class Slide : IBehaviour {
        private readonly Player self;

        public Slide(Player self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.moving.x * 7, Math.Max(-1, self.rb.velocity.y));
        }

        public void OnUpdate() {
            if (WallJump.Check(self)) self.UseBehaviour(new WallJump(self));
            if (Check(self)) {
            } //
            else if (Fall.Check(self)) self.UseBehaviour(new Fall(self));
            else if (Move.Check(self)) self.UseBehaviour(new Move(self));
        }

        public static bool Check(Player it) {
            return !it.grounded && it.slide.x != 0 && it.checks[it.slide.x > 0 ? 0 : 2].IsTouchingLayers(it.ground);
        }
    }
}
