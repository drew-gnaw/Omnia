using Unity.VisualScripting;
using UnityEngine;

namespace Enemies.Common.Behaviour {
    public class Stagger : IBehaviour {
        private readonly Enemy self;
        private float t;

        private Stagger(Enemy self) {
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

        public static IBehaviour If(Enemy it) {
            return new Stagger(it);
        }
    }
}
