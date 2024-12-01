using System;
using UnityEngine;

namespace Player.Alt.Behaviour {
    public class WallJump : IBehaviour {
        private readonly Player self;
        private float t;
        private Vector2 side;

        public WallJump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.jump = false;
            t = 0.1f;
            side = self.slide;
            self.rb.velocity = new Vector2(self.slide.x * 7 * -1, 7);
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(Mathf.Lerp(self.moving.x * 7, side.x * 7 * -1, Math.Clamp(self.rb.velocity.y / 5, 0, 1)), self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Math.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            if (Slide.Check(self)) self.UseBehaviour(new Slide(self));
            if (Check(self)) {
            } //
            else if (Fall.Check(self)) self.UseBehaviour(new Fall(self));
            else if (Move.Check(self)) self.UseBehaviour(new Move(self));
        }

        public static bool Check(Player it) {
            return !it.grounded && it.slide.x != 0 && it.jump;
        }
    }
}
