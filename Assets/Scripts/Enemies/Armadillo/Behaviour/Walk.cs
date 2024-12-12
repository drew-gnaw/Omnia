using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Walk : IBehaviour {
        private readonly Armadillo self;
        private Coroutine co;
        private Mode mode = Mode.Walk;

        public Walk(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            co = self.StartCoroutine(DoWalk());
        }

        public void OnExit() {
            self.StopCoroutine(co);
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(mode == Mode.Idle ? 0 : self.facing.x * 1, self.rb.velocity.y);

            if (CheckPlayer()) self.UseBehaviour(new Rush(self));
        }

        public void OnUpdate() {
        }

        private IEnumerator DoWalk() {
            for (var direction = self.facing; self; direction = GetDirection()) {
                yield return null;
                if (self.facing == direction) continue;

                yield return DoIdle();
                mode = Mode.Walk;
                self.facing = direction;
            }
        }

        private IEnumerator DoIdle() {
            mode = Mode.Idle;
            yield return new WaitForSeconds(1);
        }

        private bool CheckPlayer() {
            return Enemy.Sweep(self.sprite.transform.position, self.facing, 45, 15, 5, self.ground | self.player).Any(it => Enemy.IsOnLayer(it, self.player));
        }

        private Vector2 GetDirection() {
            var checks = self.checks.Select(it => it.IsTouchingLayers(self.ground)).ToArray();
            var l = checks[0] || !checks[1];
            var r = checks[3] || !checks[2];
            return l && !r ? Vector2.left : r && !l ? Vector2.right : self.facing;
        }

        private enum Mode {
            Idle,
            Walk,
        }
    }
}
