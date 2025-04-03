using System;
using System.Collections.Generic;
using Players;
using Players.Buff;
using Players.Fragments;
using UnityEngine;

namespace Scenes.Descent {
    public class DescentLevelManager : MonoBehaviour {
        public void OnEnable() {
            Player.Death += EndRun;
        }

        public void OnDisable() {
            Player.Death -= EndRun;
        }

        private void EndRun() {
            LevelManager.Instance.CustomLevel(new Warped_Depths());
        }
    }
}
