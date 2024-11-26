using System;
using System.Collections;
using System.Collections.Generic;
using Omnia.Player;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private bool zeroed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!zeroed)
        {
            player.ZeroYVelocity();
        }

        zeroed = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        zeroed = false;
    }
}
