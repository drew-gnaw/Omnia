using UnityEngine;

namespace AudioScripts {
    public class PlayAudio : MonoBehaviour {
        public void Start() {
            AudioManager.Instance.PlayBGM("Omnia - City of Mold");
        }
    }
}
