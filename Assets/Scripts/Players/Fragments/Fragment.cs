using System.Collections.Generic;
using Players.Buff;
using UnityEngine;

namespace Players.Fragments {
    public abstract class Fragment : Buff.Buff {
        [SerializeField] protected List<Fragment> nextFragments;

        public override void ApplyBuff() {
            BuffManager.Instance.AddFragmentsToPool(nextFragments);
        }
    }
}
