using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.Common;
using UnityEngine;
using Utils;

namespace Enemies.Spawnball.Behaviour {
    public class Move : IBehaviour {
        private List<Vector3> path;
        private float t;
        private Coroutine co;
        private readonly Spawnball self;

        public Move(Spawnball self) {
            this.self = self;
        }

        public void OnEnter() {
            path = CalculatePath();
            t = self.lifespan;
            co = self.StartCoroutine(DoRecalculatePathToTarget());
        }

        public void OnExit() {
            self.StopCoroutine(co);
        }

        public void OnTick() {
            if (t == 0 || IsNear(self.target.position)) self.UseBehaviour(new Activate(self));
            else {
                Vector2 next = path.LastOrDefault(it => IsNear(it));
                if (next == default) return;
                var direction = next - self.rb.worldCenterOfMass;
                self.rb.velocity = MathUtils.Lerpish(self.rb.velocity, direction.normalized * self.speed, Time.fixedDeltaTime * self.airAcceleration);
            }
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }

        private IEnumerator DoRecalculatePathToTarget() {
            for (; self; path = CalculatePath()) {
                yield return new WaitForSeconds(0.150f);
            }
        }

        private bool IsNear(Vector2 it) => Vector2.Distance(self.rb.worldCenterOfMass, it) < self.triggerDistance;

        private List<Vector3> CalculatePath() => Pathfinder.FindPath(self.rb.worldCenterOfMass, self.target.position);
    }
}
