using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField]
    private AudioSource musicSource;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        SceneManager.sceneLoaded += OnSceneChange;
    }
    
    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        
    }

    public void ToggleMusic() {
        musicSource.mute = !musicSource.mute;
    }

    public void MusicVolume(float volume) {
        musicSource.volume = volume;
    }

    protected IEnumerator FadeAudioRoutine(AudioSource audioSource, bool isFadingOut, float fadeTime) {
        float startVolume = audioSource.volume;
        float endVolume = isFadingOut ? 0 : 1;
        float time = 0;

        while (time < fadeTime) {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / fadeTime);

            yield return null;
        }

        if (isFadingOut) {
            audioSource.Stop();
            audioSource.volume = startVolume;
        } else {
            audioSource.volume = endVolume;
        }
    }
}
