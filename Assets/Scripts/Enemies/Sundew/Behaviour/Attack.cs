using System;
using UnityEngine;

namespace Enemies.Sundew.Behaviour {
    public class Attack : IBehaviour {
        private readonly Sundew self;
        private float t;

        private Attack(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.reload;
            self.WithAnimation("SundewAttack").FireProjectiles();
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(WindUp.If(self) ?? Hide.If(self));
        }

        public static IBehaviour If(Sundew it) {
            return new Attack(it);
        }
    }
}
