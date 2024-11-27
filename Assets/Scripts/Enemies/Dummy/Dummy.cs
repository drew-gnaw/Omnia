using UnityEngine;

namespace Enemies.Dummy {
    public class Dummy : Enemy {
        public override void Hurt(int damage) {
            base.Hurt(damage);
            Debug.Log(this + " took damage " + damage);
        }
    }
}
