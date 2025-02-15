using System;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Hide : IBehaviour {
        private Crab self;
        private float reloadTimer;

        public Hide(Crab crab) {
            self = crab;
            reloadTimer = self.reload;
        }

        public void OnEnter() {
            self.SetInvulnerable(true);
        }

        public void OnExit() {
        }

        public void OnTick() {
            // Don't begin popping up if the crab is "reloading"
            if (reloadTimer > 0) {
                reloadTimer = Math.Max(0, reloadTimer - Time.deltaTime);
                return;
            }

            self.UseBehaviour(new Idle(self));
        }

        public void OnUpdate() {
        }
    }
}
