using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace Players.Behaviour {
    public class Roll : IBehaviour {
        private readonly Player self;
        private float t;
        private float direction;
        private Vector2 originalColliderSize;

        public Roll(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            t = self.rollDuration;
            self.roll = false;
            self.invulnerable = true;

            originalColliderSize = self.cc.size;
            shrinkCollider();

            // If player is moving above threshold, roll in that direction, else roll in facing direction
            direction = Mathf.Sign(Mathf.Abs(self.rb.velocity.x) > self.rollThreshold ? self.rb.velocity.x : self.facing.x);

            self.OnRoll();
            self.UseAnimation("PlayerSlide");
        }

        public void OnExit() {
            self.invulnerable = false;
            resetCollider();
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.rollSpeed * direction, self.rb.velocity.y);
        }

        public void OnUpdate() {
            t -= Time.deltaTime;
            // If needed, flip direction if stuck in tunnel and deadend

            // If roll into wall, pass and enter new state
            if (self.checks[0].IsTouchingLayers(self.ground) || self.checks[2].IsTouchingLayers(self.ground)) {}
            // else check if roll is active or stuck in small space to continue
            else if (t > 0 || self.checks[3].IsTouchingLayers(self.ground | self.semisolid)) return;

            self.UseBehaviour(Slide.If(self) ?? Fall.If(self) ?? Move.If(self) ?? Idle.If(self));
        }

        public static IBehaviour If(Player it) {
            return it.roll && it.canRoll && it.grounded && it.checks[1].IsTouchingLayers(it.ground | it.semisolid) ? new Roll(it) : null;
        }

        // Probably better to have a separate dedicated collider for rolling state
        private void shrinkCollider() {
            self.cc.size = new Vector2(self.cc.size.x, self.cc.size.y / 2);
            self.cc.offset = new Vector2(self.cc.offset.x, self.cc.offset.y - self.cc.size.y / 2);
        }

        private void resetCollider() {
            self.cc.size = originalColliderSize;
            self.cc.offset = new Vector2(self.cc.offset.x, self.cc.size.y / 2);
        }
    }
}
