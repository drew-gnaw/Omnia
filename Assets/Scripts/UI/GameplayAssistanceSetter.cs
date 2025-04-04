using Players;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameplayAssistanceSetter : MonoBehaviour
{
    [SerializeField] private Transform fireflyEndpoint;
    [SerializeField] private bool repeat;
    private bool activated = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() == null) return;
        if (activated) return;
        activated = true;
        GameplayAssistance.Instance.interval = 4;
        GameplayAssistance.Instance.repeat = repeat;
        GameplayAssistance.SetPathHintTarget(fireflyEndpoint);
    }



}
