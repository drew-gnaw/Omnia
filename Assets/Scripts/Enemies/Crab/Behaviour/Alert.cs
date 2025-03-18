using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Alert : IBehaviour {
        private readonly Crab self;
        private float t;

        public Alert(Crab crab) {
            self = crab;
        }

        public void OnEnter() {
            t = self.windupTime;
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            self.UseBehaviour(new Pinch(self));
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
