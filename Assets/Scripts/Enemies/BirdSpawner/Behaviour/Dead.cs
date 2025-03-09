namespace Enemies.BirdSpawner.Behaviour {
    public class Dead : IBehaviour {
        private readonly BirdSpawner self;
        private int layer;

        public Dead(BirdSpawner self) {
            this.self = self;
        }

        public void OnEnter() {
            layer = self.gameObject.layer;
            self.SetLayer(self.bg);
        }

        public void OnExit() {
            self.gameObject.layer = layer;
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            /* There are no transitions out of this state. */
        }
    }
}
