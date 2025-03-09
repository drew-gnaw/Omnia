using System;
using UnityEngine;

namespace Enemies.BirdSpawner.Behaviour {
    public class Attack : IBehaviour {
        private readonly BirdSpawner self;
        private float cdt;

        public Attack(BirdSpawner self) {
            this.self = self;
        }

        public void OnEnter() {
            cdt = self.cooldown;
            self.SpawnBird();
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            cdt = Math.Max(0, cdt - Time.deltaTime);
            if (cdt != 0) return;

            self.UseBehaviour(self.spawns > 0 ? new Attack(self) : new Idle(self));
        }
    }
}
