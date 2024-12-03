namespace Players {
    public interface IBehaviour {
        void OnEnter();
        void OnExit();
        void OnTick();
        void OnUpdate();
    }
}
