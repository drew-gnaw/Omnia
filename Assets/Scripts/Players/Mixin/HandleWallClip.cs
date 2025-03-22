using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players.Mixin {
    [RequireComponent(typeof(Player))]
    public class HandleWallClip : MonoBehaviour {
        [SerializeField] internal BoxCollider2D check;
        private Player player;
        private LayerMask ground;

        void Start() {
            player = GetComponent<Player>();
            ground = player.ground;
        }

        // Update is called once per frame
        void Update() {
            if (IsPlayerEncased()) {
                Debug.Log("Player is encased, killing");
                player.Die();
            }
        }

        bool IsPlayerEncased() {
            return check.IsTouchingLayers(ground);
        }
    }
}
