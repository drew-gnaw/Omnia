using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public static class AudioTracks {
    // BGM
    public const string LullabyForAScrapyard = "Omnia - Lullaby For a Scrapyard";
    public const string CityOfMold = "Omnia - City of Mold";
    public const string SunkBeneath = "Omnia - Sunk Beneath";
    public const string JamiesTheme = "Omnia - Jamie_s Theme";
    public const string TrialBySteel = "Omnia - Trial By Steel";
    public const string UnclesTheme = "Omnia - Uncle Beau_s Theme";
    public const string CaveSpeak = "Omnia - Cave Speak";
    public const string FloraExMachina = "Omnia - Flora Ex Machina";
    public const string IntoTheWind = "Omnia - Into The Wind";
    public const string Undersound = "Omnia - Undersound";

    // SFX
    public const string Scrapgun = "Scrapgun";
    public const string ArmadilloAttack = "Armadillo_Attack";
    public const string ArmadilloCurl = "Armadillo_curl";
    public const string ArmadilloOpen = "Armadillo_open";
    public const string CrabHit = "Crab_hit";
    public const string CrabSpawn = "Crab_spawn";
    public const string DinkyMutate = "Dinky_Mutate";
    public const string HarpoonHit = "Harpoon_Hit";
    public const string HarpoonLaunch = "Harpoon_Launch";
    public const string HarpoonRetract = "Harpoon_Retract";
    public const string JamieLand = "Jamie_land";
    public const string JamieSlide = "Jamie_Slide";
    public const string JamieHurt1 = "JamieHurt1";
    public const string JamieHurt2 = "JamieHurt2";
    public const string JamieHurt3 = "JamieHurt3";
    public const string JamieHurt4 = "JamieHurt4";
    public const string PlantShoot = "Plant_Shoot";
    public const string Rumble = "Rumble";


    // AMBIENT
}

public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioSource BGMPlayer, SFXPlayer, AmbientPlayer;
    private AudioList bgmTracks, sfxTracks, ambientTracks;

    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    public void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    protected override void OnAwake() {
        ambientTracks = Resources.LoadAll<AudioList>("AudioResources")[0];
        bgmTracks = Resources.LoadAll<AudioList>("AudioResources")[1];
        sfxTracks = Resources.LoadAll<AudioList>("AudioResources")[2];
    }

    // this only starts playing after a scene change, but doesnt matter because a linear playthrough will have audiomanager already playing
    // tl;dr: this is only for development
    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (!BGMPlayer.isPlaying) {
            PlayBGM(AudioTracks.CaveSpeak);
        }
    }


    //////////////////////////////
    // Background Music Methods //
    //////////////////////////////

    // Starts playing the specified background track in a loop.
    // If the track is already playing, does nothing
    public void PlayBGM(string trackName) {
        AudioClip track = bgmTracks.GetClipByName(trackName);
        if (!BGMPlayer.isPlaying || BGMPlayer.clip != track) {
            BGMPlayer.clip = track;
            BGMPlayer.loop = true;
            BGMPlayer.Play();
        }
    }

    // Smoothly transition from current track to specified new track. Use crossfading for seamless experience
    public void SwitchBGM(string newTrackName) {
        AudioClip newTrack = bgmTracks.GetClipByName(newTrackName);

        if (BGMPlayer.clip != newTrack) {
            StartCoroutine(CrossfadeBGM(newTrack));
        }
    }

    private IEnumerator CrossfadeBGM(AudioClip newTrack) {
        float duration = 1f;  // Crossfade time
        float initialVolume = BGMPlayer.volume;

        // Fade out current track
        for (float t = 0; t < duration; t += Time.deltaTime) {
            BGMPlayer.volume = Mathf.Lerp(initialVolume, 0f, t / duration);
            yield return null;
        }

        // Switch to new track
        BGMPlayer.Stop();
        BGMPlayer.clip = newTrack;
        BGMPlayer.Play();

        // Fade in new track
        for (float t = 0; t < duration; t += Time.deltaTime) {
            BGMPlayer.volume = Mathf.Lerp(0f, initialVolume, t / duration);
            yield return null;
        }
    }

    // Coroutine to fade BGM out from initial volume
    private IEnumerator BGMFadeOut(float initialVolume) {
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            BGMPlayer.volume = Mathf.Lerp(initialVolume, 0f, t);
            yield return null;
        }
    }
    // Coroutine to fade BGM in to target volume
    private IEnumerator BGMFadeIn(float targetVolume) {
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            BGMPlayer.volume = Mathf.Lerp(0f, targetVolume, t);
            yield return null;
        }
    }

    // Stop the current background music entirely.
    // Fade out if a transition effect is needed
    public void StopBGM() {
        StartCoroutine(BGMFadeOut(BGMPlayer.volume));
        BGMPlayer.Stop();
    }

    // Pauses background music
    public void PauseBGM() {
        if (BGMPlayer.isPlaying) {
            BGMPlayer.Pause();
        }
    }
    // Resumes background music
    public void ResumeBGM() {
        if (!BGMPlayer.isPlaying) {
            BGMPlayer.Play();
        }
    }

    // Mutes background music
    public void ToggleBGM() {
        BGMPlayer.mute = !BGMPlayer.mute;
    }

    // Adjust BGM volume dynamically, allowing for changes
    // during intense scenes or quieter moments
    public void SetBGMVolume(float volume) {
        if (volume < 0f) {
            BGMPlayer.volume = 0f;
        }
        else if (volume > 1f) {
            BGMPlayer.volume = 1f;
        }
        else {
            BGMPlayer.volume = volume;
        }
    }

    ///////////////////////////
    // Sound Effects Methods //
    ///////////////////////////

    // Plays a specific sound effect by looking it up in AudioList
    // Ideal for short one-time sounds like footsteps, item pickups, enemy attacks
    public void PlaySFX(string soundName) {
        SFXPlayer.PlayOneShot(sfxTracks.GetClipByName(soundName));
    }

    // Stop any currently playing sound effects immediately
    // Useful for stopping all SFX during pause or special event
    public void StopSFX() {
        SFXPlayer.Stop();
    }

    // Adjusts the volume of all sound effects globally
    public void SetSFXVolume(float volume) {
        if (volume < 0f) {
            SFXPlayer.volume = 0f;
        }
        else if (volume > 1f) {
            SFXPlayer.volume = 1f;
        }
        else {
            SFXPlayer.volume = volume;
        }
    }

    // Mutes sound effects
    public void ToggleSFX() {
        SFXPlayer.mute = !SFXPlayer.mute;
    }

    public void PlayHurtSound() {
        SFXPlayer.PlayOneShot(sfxTracks.GetClipByName($"JamieHurt{Random.Range(1, 4)}"));
    }

    /////////////////////
    // Ambient Methods //
    /////////////////////

    // Starts an ambient sound by looking it up in the AudioList
    // Typically loops and continues until stopped or replaced
    public void PlayAmbient(string ambientName) {
        AmbientPlayer.clip = ambientTracks.GetClipByName(ambientName);
        AmbientPlayer.loop = true;
        AmbientPlayer.Play();
    }

    // Stops any currently playing ambient sound
    public void StopAmbient() {
        AmbientPlayer.Stop(); // want fade by default?
    }

    // Adjust the volume of all ambient sounds globally
    public void SetAmbientVolume(float volume) {
        if (volume < 0f) {
            AmbientPlayer.volume = 0f;
        }
        else if (volume > 1f) {
            AmbientPlayer.volume = 1f;
        }
        else {
            AmbientPlayer.volume = volume;
        }
    }

    // Mutes ambient sounds
    public void ToggleAmbient() {
        AmbientPlayer.mute = !AmbientPlayer.mute;
    }

    // Mutes all sounds
    public void ToggleAudio() {
        ToggleBGM();
        ToggleSFX();
        ToggleAmbient();
    }
}
