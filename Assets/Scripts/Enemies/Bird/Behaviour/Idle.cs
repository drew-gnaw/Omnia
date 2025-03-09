using UnityEngine;

namespace Enemies.Bird.Behaviour {
    public class Idle : IBehaviour {
        private float t;
        private readonly Bird self;

        public Idle(Bird self) {
            this.self = self;
        }

        public void OnEnter() {
            self.rb.gravityScale = 1;
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            t = 0.1f;

            if (self.IsTargetDetected(out var target)) self.UseBehaviour(new Alert(self));
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
