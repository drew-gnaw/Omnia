using Players;
using Puzzle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFeetBehaviour : MonoBehaviour
{
    [SerializeField] public Player player;

    public Vector3 GetPlayerTransform() {
        return player.transform.position;
    }

    public void SetPlayerTransform(Vector2 position) {
        player.transform.position = position;
    }
}
