using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class Attack : IBehaviour {
        private readonly Sundew self;
        private float t;

        private Attack(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.reload / 2; // Simply to complete the attack animation

            self.FireProjectiles();
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(Reload.If(self));
        }

        public static IBehaviour If(Sundew it) {
            return it.detected ? new Attack(it) : null;
        }
    }
}
