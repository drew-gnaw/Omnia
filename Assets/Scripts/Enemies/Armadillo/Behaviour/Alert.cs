﻿using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Alert : IBehaviour {
        private readonly Armadillo self;
        private float t;

        public Alert(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.alertTime;
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(0, self.rb.velocity.y);
            if (t != 0) return;
            self.UseBehaviour(new Roll(self));
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
        }
    }
}
