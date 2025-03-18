using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies.Armadillo.Behaviour {
    public class Rush : IBehaviour {
        private readonly Armadillo self;

        public Rush(Armadillo self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
            self.rb.velocity = new Vector2(self.facing.x * self.rollSpeed, self.rb.velocity.y);

            float targetFps = 48f;
            float frameTime = 1f / targetFps; // Target frame time
            float speed = Mathf.Abs(self.rb.velocity.x);

            // Use Time.deltaTime to adjust based on actual frame rate
            float rotationAmount = -self.facing.x * 360f * speed * frameTime * (Time.deltaTime / frameTime);

            self.sprite.transform.Rotate(0f, 0f, rotationAmount);

            if (CheckHit()) {
                self.sprite.transform.rotation = Quaternion.identity;
                UseStun();
            }
        }


        public void OnUpdate() {
        }

        private bool CheckHit() {
            return self.checks[self.facing.x > 0 ? 0 : 3].IsTouchingLayers(self.ground | self.player);
        }

        private void UseStun() {
            var hit = Enemy.Sweep(self.sprite.transform.position, self.facing, 0, self.hitDistance, 1, self.player).First();
            if (hit) self.Attack(hit.collider.gameObject);

            self.UseBehaviour(new Stun(self));
        }
    }
}
