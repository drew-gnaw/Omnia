using System;
using UnityEngine;

namespace Player.Alt.Behaviour {
    public class Jump : IBehaviour {
        private readonly Player self;
        private float t;

        public Jump(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.jump = false;
            t = 0.1f;
            self.rb.velocity = new Vector2(self.moving.x * 7, 7);
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.moving.x * 7, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t = Math.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            if (Slide.Check(self)) self.UseBehaviour(new Slide(self));
            else if (Fall.Check(self)) self.UseBehaviour(new Fall(self));
            else if (Move.Check(self)) self.UseBehaviour(new Move(self));
        }

        public static bool Check(Player it) {
            return it.grounded && it.jump;
        }
    }
}
