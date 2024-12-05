namespace Enemies.Sundew.Behaviour {
    public class Idle : IBehaviour {
        private readonly Sundew self;
        private int layer;

        private Idle(Sundew self) {
            this.self = self;
        }

        public void OnEnter() {
            layer = self.gameObject.layer;

            self.SetLayer(self.bg);
            self.UseAnimation("SundewIdle");
        }

        public void OnExit() {
            self.gameObject.layer = layer;
        }

        public void OnTick() {
        }

        public void OnUpdate() {
            self.UseBehaviour(Reveal.If(self));
        }

        public static IBehaviour AsDefaultOf(Sundew it) {
            return new Idle(it);
        }

        public static IBehaviour If(Sundew it) {
            return !it.detected ? new Idle(it) : null;
        }
    }
}
