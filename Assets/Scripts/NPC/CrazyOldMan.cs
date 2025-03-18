using System;

namespace NPC {
    public class CrazyOldMan : GenericNPC {
        public static event Action OnSpeakToCrazyOldMan;
        public override void Interact() {
            base.Interact();
            OnSpeakToCrazyOldMan?.Invoke();
        }
    }
}
