using System;
using Players;
using UnityEngine;

namespace Enemies.Crab.Behaviour {
    public class Pinch : IBehaviour {
        private readonly Crab self;
        private float t;

        public Pinch(Crab crab) {
            self = crab;
        }

        public void OnEnter() {
            t = self.vulnerableTime;

            if (IsHitTarget(out var target)) self.Attack(target);
        }

        public void OnExit() {
        }

        public void OnTick() {
            if (t != 0) return;
            self.UseBehaviour(new Hide(self));
        }

        public void OnUpdate() {
            t = Math.Max(0, t - Time.deltaTime);
        }

        private bool IsHitTarget(out Player player) {
            var hit = Physics2D.OverlapCircle(self.sprite.transform.position, self.attackRadius, self.player);
            player = null;
            return hit && hit.TryGetComponent(out player);
        }
    }
}
