using UnityEngine;

namespace Players.Behaviour {
    public class Pull : IBehaviour {
        private readonly Player self;
        private float t;
        private Vector2 direction;

        private readonly Transform target;

        public Pull(Player self, Transform target) {
            this.self = self;
            this.target = target;
        }

        /* TODO: Figure out what should happen on collision during pull? */
        public void OnEnter() {
            t = Vector2.Distance(self.transform.position, target.position) / self.pullSpeed;
            direction = target.position - self.transform.position;

            self.UseAnimation("PlayerIdle");
        }

        public void OnExit() {
            self.rb.velocity = new Vector2(self.rb.velocity.x, Mathf.Min(self.jumpSpeed * 2, self.rb.velocity.y));
        }

        public void OnTick() {
            self.rb.velocity = direction.normalized * self.pullSpeed;
        }

        public void OnUpdate() {
            t = Mathf.Max(0, t - Time.deltaTime);
            if (t != 0) return;

            self.UseBehaviour(new Fall(self));
        }
    }
}
