using System;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Alert : IBehaviour {
        private Crab self;
        private float windupTimer;

        public Alert(Crab crab) {
            self = crab;
            windupTimer = self.windup;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (windupTimer > 0) {
                windupTimer = Math.Max(0, windupTimer - Time.deltaTime);
                return;
            }

            self.UseBehaviour(new Attack(self));
        }

        public void OnUpdate() {
        }
    }
}
