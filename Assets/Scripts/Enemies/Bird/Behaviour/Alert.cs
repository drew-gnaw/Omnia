using UnityEngine;
using Utils;

namespace Enemies.Bird.Behaviour {
    public class Alert : IBehaviour {
        private float t;
        private readonly Bird self;

        public Alert(Bird self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.delay;
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = MathUtils.Lerpish(self.rb.velocity, Vector2.zero, Time.fixedDeltaTime * self.airAcceleration);
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(new Attack(self));
        }
    }
}
