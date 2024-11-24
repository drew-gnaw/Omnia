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

            if (mode == Mode.Rush && CheckHit()) self.UseBehaviour(new Stun(self));
        }

        private IEnumerator DoRush() {
            yield return new WaitForSeconds(1);
            mode = Mode.Rush;
        }

        private bool CheckHit() {
            var hit = Armadillo.Sweep(self.sprite.transform.position, self.facing, 0, 0.625f, 1, self.ground | self.player).First();
            if (!hit) return false;
            if (Armadillo.IsOnLayer(hit, self.player)) self.OnAttack(hit.collider.gameObject);
            return true;
        }

        private enum Mode {
            Idle,
            Rush,
        }
    }
}
