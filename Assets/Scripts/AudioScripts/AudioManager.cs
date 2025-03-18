using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
using Utils;

public class AudioManager : PersistentSingleton<AudioManager>
{
    private EventInstance bgmInstance;
    private EventInstance ambientInstance;

    private Bus masterBus;
    private Bus sfxBus;
    private bool isMuted = false;

    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    protected override void OnAwake()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode) {
        // Optionally, you can change BGM or ambient sound based on scene
    }

    public void SetMasterVolume(float value) {
        float volume = Mathf.Clamp(value, 0f, 1f); // Ensure volume is between 0 and 1
        masterBus.setVolume(volume);
    }

    //////////////////////////////
    // Background Music Methods //
    //////////////////////////////

    // Play a new FMOD event as background music
    public void PlayBGM(EventReference eventReference) {
        if (bgmInstance.isValid()) {
            bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bgmInstance.release();
        }
        bgmInstance = RuntimeManager.CreateInstance(eventReference);
        bgmInstance.start();
    }

    // Switch to a different FMOD event with fade-out and fade-in
    public void SwitchBGM(string newEventPath) {
        StartCoroutine(BGMTransition(newEventPath));
    }

    private IEnumerator BGMTransition(string newEventPath) {
        if (bgmInstance.isValid()) {
            bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            yield return new WaitForSeconds(1f); // Wait for fade-out duration
            bgmInstance.release();
        }
        bgmInstance = RuntimeManager.CreateInstance(newEventPath);
        bgmInstance.start();
    }

    ///////////////////////////
    // Sound Effects Methods //
    ///////////////////////////

    public void PlaySFX(EventReference eventReference) {
        RuntimeManager.PlayOneShot(eventReference);
    }

    /////////////////////
    // Ambient Methods //
    /////////////////////

    public void PlayAmbient(string eventPath) {
        if (ambientInstance.isValid()) {
            ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambientInstance.release();
        }
        ambientInstance = RuntimeManager.CreateInstance(eventPath);
        ambientInstance.start();
    }

    public void StopAmbient() {
        if (ambientInstance.isValid()) {
            ambientInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambientInstance.release();
        }
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
        float volume = isMuted ? 0f : 1f;
        masterBus.setVolume(volume);
    }
}
