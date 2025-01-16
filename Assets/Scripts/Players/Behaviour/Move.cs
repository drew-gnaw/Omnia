using System;
using UnityEngine;

namespace Players.Behaviour {
    public class Move : IBehaviour {
        private static readonly int AnimatorSpeed = Animator.StringToHash("speed");

        private static IBehaviour _s;
        private readonly Player self;

        private Move(Player self) {
            this.self = self;
        }

        public void OnEnter() {
            self.UseAnimation("PlayerMove");
        }

        public void OnExit() {
            self.animator.SetFloat(AnimatorSpeed, 1);
        }

        public void OnTick() {
            var x = self.HorizontalVelocityOf(self.moving.x * self.moveSpeed, Time.fixedDeltaTime * self.moveAccel);
            self.rb.velocity = new Vector2(x, self.rb.velocity.y);
        }

        public void OnUpdate() {
            self.animator.SetFloat(AnimatorSpeed, Math.Sign(self.facing.x) == Math.Sign(self.moving.x) ? 1 : -1);

            self.UseBehaviour(Fall.If(self) ?? Jump.If(self) ?? Idle.If(self));
        }

        private static IBehaviour AdHoc(Player it) {
            return _s ??= new Move(it);
        }

        public static IBehaviour If(Player it) {
            return it.grounded && it.moving.x != 0 && it.checks[1].IsTouchingLayers(it.ground | it.semisolid) ? AdHoc(it) : null;
        }
    }
}
