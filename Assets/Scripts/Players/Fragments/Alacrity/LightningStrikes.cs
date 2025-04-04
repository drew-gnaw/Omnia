using UnityEngine;

namespace Players.Fragments {
    public class LightningStrikes : Fragment {
        [SerializeField] private float damageMultiplierBuff;

        private Rigidbody2D rb;

        private void Update() {
            if (rb == null || player == null) return;

            float speed = rb.velocity.magnitude;
            player.damageMultiplier += (speed * damageMultiplierBuff);
        }

        public override void ApplyBuff() {
            base.ApplyBuff();
            rb = player.GetComponent<Rigidbody2D>();
        }

        public override void RevokeBuff() {
            player.damageMultiplier -= damageMultiplierBuff;
        }
    }
}
