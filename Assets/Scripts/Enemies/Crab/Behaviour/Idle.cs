namespace Enemies.Crab.Behaviour {
    public class Idle : IBehaviour {
        private readonly Crab self;
        private int layer;

        public Idle(Crab crab) {
            self = crab;
        }

        public void OnEnter() {
            layer = self.gameObject.layer;
            self.SetLayer(self.bg);
        }

        public void OnExit() {
            self.gameObject.layer = layer;
        }

        public void OnTick() {
            if (self.IsTargetDetected()) self.UseBehaviour(new Alert(self));
        }

        public void OnUpdate() {
        }
    }
}
