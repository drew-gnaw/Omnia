using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class Attack : IBehaviour {
        private readonly Sundew self;
        private float t;

        public Attack(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.windup;

            self.lag = 0;
            self.FireProjectiles();
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            self.UseBehaviour(new Idle(self));
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
