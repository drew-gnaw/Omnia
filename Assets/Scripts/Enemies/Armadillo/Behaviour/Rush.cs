using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Rush : IBehaviour {
        private readonly Armadillo self;
        private Coroutine co;
        private Mode mode = Mode.Idle;

        public Rush(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            co = self.StartCoroutine(DoRush());
        }

        public void OnExit() {
            self.StopCoroutine(co);
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(mode == Mode.Idle ? 0 : self.facing.x * 3, self.rb.velocity.y);

            if (mode == Mode.Rush && CheckHit()) UseStun();
        }

        private IEnumerator DoRush() {
            yield return new WaitForSeconds(1);
            mode = Mode.Rush;
        }

        private bool CheckHit() {
            return self.checks[Math.Sign(self.facing.x) == 1 ? 0 : 3].IsTouchingLayers(self.ground | self.player);
        }

        private void UseStun() {
            var hit = Enemy.Sweep(self.sprite.transform.position, self.facing, 0, 1, 1, self.player).First();
            if (hit) self.Attack(hit.collider.gameObject);

            self.UseBehaviour(new Stun(self));
        }

        private enum Mode {
            Idle,
            Rush,
        }
    }
}
