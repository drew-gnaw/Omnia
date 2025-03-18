using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class Reload : IBehaviour {
        private readonly Sundew self;
        private float t;

        private Reload(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.reload;
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(WindUp.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Sundew it) {
            return new Reload(it);
        }
    }
}
