using System;
using System.Collections;
using NPC;
using UnityEngine;
using Utils;

namespace Scenes {
    public class City : MonoBehaviour {
        public void Start() {
            Guard.OnSpeakToGuard += ExitScene;

        }

        public void OnDisable() {
            Guard.OnSpeakToGuard -= ExitScene;
        }

        private void ExitScene() {
            LevelManager.Instance.NextLevel();
        }
    }
}
