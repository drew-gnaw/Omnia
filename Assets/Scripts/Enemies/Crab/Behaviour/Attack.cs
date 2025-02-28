using System;
using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Attack : IBehaviour {
        private Crab self;
        private float vulnerableTimer;

        public Attack(Crab crab) {
            self = crab;
            vulnerableTimer = self.vulnerableTime;
        }

        public void OnEnter() {
            Player p = self.GetPlayerWithinAttackArea();
            if (p) self.Attack(p);
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (vulnerableTimer > 0) {
                vulnerableTimer = Math.Max(0, vulnerableTimer - Time.deltaTime);
                return;
            }
            self.UseBehaviour(new Hide(self));
        }

        public void OnUpdate() {
        }
    }
}
