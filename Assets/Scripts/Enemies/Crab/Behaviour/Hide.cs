using System;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Hide : IBehaviour {
        private readonly Crab self;
        private float t;

        public Hide(Crab crab) {
            self = crab;
        }

        public void OnEnter() {
            t = self.reloadTime;
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            self.UseBehaviour(new Idle(self));
        }

        public void OnUpdate() {
            t = Math.Max(0, t - Time.deltaTime);
        }
    }
}
