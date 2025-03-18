using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Utils;

public class FMODEvents : PersistentSingleton<FMODEvents> {
    protected override void OnAwake() {

    }

    // [field: Header("Music")]
    // [field: SerializeField]
    // public EventReference music { get; private set; }

    [field: Header("Harpoon SFX")]
    [field: SerializeField]
    public EventReference harpoonLaunch { get; private set; }
    [field: SerializeField]
    public EventReference harpoonHit { get; private set; }
    [field: SerializeField]
    public EventReference harpoonRetract { get; private set; }
}
