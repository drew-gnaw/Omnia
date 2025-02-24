using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class WindUp : IBehaviour {
        private readonly Sundew self;
        private float t;

        private WindUp(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.windup;
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Attack.If(self));
        }

        public static IBehaviour If(Sundew it) {
            return it.detected ? new WindUp(it) : null;
        }
    }
}
