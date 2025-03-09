namespace Enemies.BirdSpawner.Behaviour {
    public class Idle : IBehaviour {
        private readonly BirdSpawner self;

        public Idle(BirdSpawner self) {
            this.self = self;
        }

        public void OnEnter() {
        }

        public void OnExit() {
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            if (self.spawns > 0) self.UseBehaviour(new Attack(self));
        }
    }
}
