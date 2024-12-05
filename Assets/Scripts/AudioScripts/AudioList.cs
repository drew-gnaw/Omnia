using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[CreateAssetMenu(fileName = "New Audio List", menuName = "Audio List")]
public class AudioList : ScriptableObject {
    [SerializeField] private List<AudioClip> clipList;
    private Dictionary<string, AudioClip> bgmTracks;

    public AudioClip GetClipByName(string trackName) {
        if (bgmTracks == null) InitializeDictionary();
        return bgmTracks[trackName];
    }

    public void InitializeDictionary() {
        bgmTracks = new();
        foreach (AudioClip clip in clipList) {
            bgmTracks[clip.name] = clip;
        }
    }
}