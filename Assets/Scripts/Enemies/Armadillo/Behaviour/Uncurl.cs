using System.Collections;
using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Uncurl : IBehaviour {
        private Armadillo self;

        public Uncurl(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            self.StartCoroutine(DoUncurl());
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
        }

        private IEnumerator DoUncurl() {
            yield return new WaitForSeconds(0.5f);
            self.UseBehaviour(new Walk(self));
        }
    }
}
