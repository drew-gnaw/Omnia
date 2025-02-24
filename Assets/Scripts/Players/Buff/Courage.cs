using UI;
using UnityEngine;

namespace Players.Buff {
    public class Courage : Buff {
        [SerializeField] private float damageReductionPercentage;

        protected override void Start() {
            base.Start();
        }

        private float ReduceDamage(float incomingDamage) {
            float outDamage = incomingDamage * damageReductionPercentage;
            Debug.Log($"{name}: Reducing incoming damage {incomingDamage} to {outDamage}", gameObject);
            return outDamage;
        }

        public override void ApplyBuff() {
            Buff.OnDamageTaken.AddLast(ReduceDamage);
            UIController.Instance.SetHealthBarColor(UIController.Instance.buffHealthColor);
        }

        public override void RevokeBuff() {
            Buff.OnDamageTaken.Remove(ReduceDamage);
            UIController.Instance.SetHealthBarColor(UIController.Instance.defaultHealthColor);
        }
    }
}
