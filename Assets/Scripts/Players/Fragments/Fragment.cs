using System.Collections.Generic;
using Players.Buff;
using UnityEngine;

namespace Players.Fragments {
    public abstract class Fragment : Buff.Buff {
        [SerializeField] protected List<Fragment> nextFragments;
        [SerializeField] public string fragmentName;
        [SerializeField, TextArea] public string description;

        public override void ApplyBuff() {
            BuffManager.Instance.AddFragmentsToPool(nextFragments);
            BuffManager.Instance.RemoveFragmentFromPool(this);
        }
    }
}
