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
            self.rb.velocity = new Vector2(mode == Mode.Idle ? 0 : self.facing.x * 6, self.rb.velocity.y);

            if (mode == Mode.Rush && CheckHit()) UseStun();
        }

        public void OnUpdate() {
        }

        private IEnumerator DoRush() {
            yield return new WaitForSeconds(1);
            mode = Mode.Rush;
        }

        private bool CheckHit() {
            return self.checks[self.facing.x > 0 ? 0 : 3].IsTouchingLayers(self.ground | self.player);
        }

        private void UseStun() {
            if (CheckPlayer(out RaycastHit2D hit)) self.Attack(hit.collider.gameObject);
            self.UseBehaviour(new Stun(self));
        }

        private bool CheckPlayer(out RaycastHit2D player) {
            var hits = Enemy.Sweep(self.sprite.transform.position, self.facing, 45, 5, 5, self.ground | self.player);
            player = hits.FirstOrDefault(hit => Enemy.IsOnLayer(hit, self.player));
            return player != default;
        }


        private enum Mode {
            Idle,
            Rush,
        }
    }
}
