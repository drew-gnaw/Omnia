using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players.Buff {
// ***** Can instantiate an instance of a Buff onto the Player for debug purposes ***** //
    public class DebugBuffProvider : MonoBehaviour {
        [SerializeField] private Buff buff;
        [SerializeField] private Player player;

        // Solely for debugging buff.RevokeBuff(). If buffs have durations, they should be implemented in the buff itself.
        [SerializeField] private int buffDuration = -1;

        private Buff buffInstance;

        private void InstantiateBuff() {
            buffInstance = Instantiate(buff, player.buffsParent);
            buffInstance.Initialize(player);
            buffInstance.ApplyBuff();

            if (buffDuration != -1) {
                Debug.Log($"Buff {buffInstance.name} will end in {buffDuration}");
                StartCoroutine(DisableBuff());
            }
        }

        private IEnumerator DisableBuff() {
            yield return new WaitForSeconds(buffDuration);
            buffInstance.RevokeBuff();
            Debug.Log($"Buff {buffInstance.name} has been revoked", gameObject);
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if (other != player.GetComponent<Collider2D>()) return;
            Debug.Log($"Player entered debug buff provider, instantiating: {buff.name}");
            InstantiateBuff();

            // Hide this
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
