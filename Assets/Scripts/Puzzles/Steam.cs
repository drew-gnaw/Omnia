using Enemies;
using Omnia.Utils;
using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour {
    [SerializeField] private Collider2D Collider2d;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;

    void OnTriggerEnter2D(Collider2D other) {
        if (Collider2d.IsTouchingLayers(playerLayer)) {
            var player = other.gameObject.GetComponent<Player>();
            if (player != null) player.Die();
        } else if (Collider2d.IsTouchingLayers(enemyLayer)) {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null) enemy.Die();
        }
    }
}
