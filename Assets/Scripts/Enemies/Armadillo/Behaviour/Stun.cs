using System.Collections;
using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Stun : IBehaviour {
        private readonly Armadillo self;
        private Coroutine co;
        private Mode mode = Mode.Stun;

        public Stun(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            self.rb.velocity = new Vector2(self.facing.x * -1, 3);

            co = self.StartCoroutine(DoStun());
        }

        public void OnExit() {
            self.StopCoroutine(co);
        }

        public void OnTick() {
            if (mode == Mode.Idle) self.UseBehaviour(new Walk(self));
        }

        public void OnUpdate() {
        }

        private IEnumerator DoStun() {
            yield return new WaitForSeconds(3);
            mode = Mode.Idle;
        }

        private enum Mode {
            Idle,
            Stun,
        }
    }
}
