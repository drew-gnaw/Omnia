using UnityEngine;

namespace Enemies.Spawnball.Behaviour {
    public class Idle : IBehaviour {
        private readonly Spawnball self;
        private float t;

        public Idle(Spawnball self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.startDelay;
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            self.UseBehaviour(new Move(self));
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
