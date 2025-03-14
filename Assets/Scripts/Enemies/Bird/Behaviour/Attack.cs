using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.Common;
using UnityEngine;
using Utils;

namespace Enemies.Bird.Behaviour {
    public class Attack : IBehaviour {
        private readonly Bird self;
        private List<Vector3> path;
        private float t;
        private Coroutine co;

        public Attack(Bird self) {
            this.self = self;
        }

        public void OnEnter() {
            path = PathToTarget();
            t = self.fuse;
            co = self.StartCoroutine(DoRecalculatePathToTarget());
        }

        public void OnExit() {
            self.StopCoroutine(co);
        }

        public void OnTick() {
            if (IsNear(self.targetInstance.sprite.transform.position)) {
                self.OnExplode();
            } else {
                var next = path.LastOrDefault(IsNear);
                if (next == default) return;
                var direction = next - self.transform.position;

                self.rb.velocity = MathUtils.Lerpish(self.rb.velocity, direction.normalized * self.speed, Time.fixedDeltaTime * self.airAcceleration);
            }
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.OnExplode(); /* Fuse ran out. */
        }

        private IEnumerator DoRecalculatePathToTarget() {
            yield return new WaitForSeconds(0.1f);

            while (self.isActiveAndEnabled) {
                path = PathToTarget();
                yield return new WaitForSeconds(0.1f);
            }
        }

        private bool IsNear(Vector3 it) => Vector2.Distance(self.transform.position, it) < self.triggerDistance;

        private List<Vector3> PathToTarget() => Pathfinder.FindPath(self.transform.position, self.targetInstance.sprite.transform.position);
    }
}
