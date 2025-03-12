using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Alert : IBehaviour {
        private Armadillo self;
        private float t;

        public Alert(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            t = 1f;
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(new Rush(self));
        }
    }
}
