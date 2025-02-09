using System;
using System.Collections.Generic;
using UnityEngine;

// Buffs must be attached to GameObject such that they can have their own particle effects
// and side effects. Buffs can only apply to the player, as per the current spec
namespace Players.Buff {
    public abstract class Buff : MonoBehaviour {
        protected Player player; // In case we want access to some specific property of the player

        // ***** Event system that buffs can attach to ***** //
        // PERF: Delegates might be more optimal, but from what I've seen they're a lot less readable
        // PERF: There may be something faster than a LinkedList, perhaps a HashSet?

        // When the player takes damage, how should we modify the incoming damage?
        public static LinkedList<Func<float, float>> OnDamageTaken = new LinkedList<Func<float, float>>();

        protected virtual void Start() {
            if (player == null) Debug.LogWarning($"Player is not set for buff: {name}");
        }

        public virtual void Initialize(Player p) {
            player = p;
        }

        // Responsible for appending `this` to any events
        public abstract void ApplyBuff();
        public abstract void RevokeBuff();
    }
}
