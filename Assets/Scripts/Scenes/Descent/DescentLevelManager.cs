using System;
using System.Collections.Generic;
using Players.Buff;
using Players.Fragments;
using UnityEngine;

namespace Scenes.Descent {
    public class DescentLevelManager : MonoBehaviour {
        [SerializeField] private List<Fragment> fragmentPrefabs;
        public void Start() {
            foreach (var fragment in fragmentPrefabs) {
                Debug.Log(fragment.name);
                BuffManager.Instance.ApplyBuff(fragment);
            }
        }
    }
}
