using Puzzle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MovingPlatform movable;

    public float GetPlatformSpeed() {
        return movable.GetPlatformSpeed();
    }

    public Vector2 GetDestination() {
        return movable.GetDestination();
    }
}
