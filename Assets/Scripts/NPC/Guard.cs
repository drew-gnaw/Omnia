using System;

namespace NPC {
    public class Guard : GenericNPC {
        public static event Action OnSpeakToGuard;
        public override void Interact() {
            base.Interact();
            OnSpeakToGuard?.Invoke();
        }
    }
}
