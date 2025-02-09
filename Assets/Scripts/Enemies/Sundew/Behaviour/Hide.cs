using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class Hide : IBehaviour {
        private readonly Sundew self;
        private float t;

        private Hide(Sundew self) {
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

            self.UseBehaviour(Idle.If(self) ?? Reveal.If(self));
        }

        public static IBehaviour If(Sundew it) {
            return !it.detected ? new Hide(it) : null;
        }
    }
}
