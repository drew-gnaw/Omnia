using Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGate : MonoBehaviour
{
    [TypeFilter(typeof(LevelData))]
    [SerializeField] private SerializableType customLevelData;
    [SerializeField] GateBehaviour gateBehaviour;
    private LevelData LevelData { get => LevelData.Parse(customLevelData.Type); }
    public enum GateBehaviour { NextLevel, PrevLevel, RestartLevel, CustomLevel }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            (gateBehaviour switch {
                GateBehaviour.NextLevel     => (Action)(() => LevelManager.Instance.NextLevel()),
                GateBehaviour.PrevLevel     => () => LevelManager.Instance.PrevLevel(),
                GateBehaviour.CustomLevel   => () => LevelManager.Instance.CustomLevel(LevelData),
                _                           => () => LevelManager.Instance.Restart()
            })();
        }
    }

}
