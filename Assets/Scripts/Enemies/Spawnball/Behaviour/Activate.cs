using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.Common;
using UnityEngine;
using Utils;

namespace Enemies.Spawnball.Behaviour {
    public class Activate : IBehaviour {
        private float t;
        private readonly Spawnball self;

        public Activate(Spawnball self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.delay;
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = Vector2.zero;
            if (t == 0) self.OnSpawnEnemy();
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
