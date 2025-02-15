using System;
using System.Collections;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Idle : IBehaviour {
        private readonly Crab self;
        private float reloadTimer;

        // If initial, don't set a reload timer so that the Crab is immediately ready
        public Idle(Crab crab) {
            self = crab;
        }

        public void OnEnter() {
            self.SetInvulnerable(true);
        }

        public void OnExit() {
            self.SetInvulnerable(false);
        }

        public void OnTick() {
            Vector2 v = self.CheckPlayer();
            if (v == Vector2.zero) return;
            self.SetDirection(v);
            self.UseBehaviour(new Alert(self));
        }

        public void OnUpdate() {
        }
    }
}
