using UnityEngine;

namespace Enemies.Common.Behaviour {
    public class Stagger : IBehaviour {
        private readonly Enemy self;
        private float t;

        public Stagger(Enemy self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.staggerDurationS;
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(self.prevBehaviour);
        }
    }
}
