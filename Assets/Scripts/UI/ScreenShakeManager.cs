using System.Collections;
using Cinemachine;
using Players.Buff;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class ScreenShakeManager : PersistentSingleton<ScreenShakeManager> {
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    protected override void OnAwake() {
        UpdateCameraReference();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        UpdateCameraReference();
    }

    public void Shake(float intensity = 1.0f, float duration = 0.5f) {
        if (perlinNoise != null) {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        } else {
            Debug.LogError("Perlin noise module is missing!");
        }
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration) {
        Debug.Log("Shaking");
        float elapsed = 0f;

        // Store the original values
        float originalAmplitude = perlinNoise.m_AmplitudeGain;
        float originalFrequency = perlinNoise.m_FrequencyGain;

        // Set the shake values
        perlinNoise.m_AmplitudeGain = intensity;
        perlinNoise.m_FrequencyGain = intensity;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            // Gradually reduce the amplitude over time (decay effect)
            float decayFactor = Mathf.Lerp(1f, 0f, elapsed / duration);
            perlinNoise.m_AmplitudeGain = intensity * decayFactor;

            yield return null;
        }

        // Reset to the original values
        perlinNoise.m_AmplitudeGain = originalAmplitude;
        perlinNoise.m_FrequencyGain = originalFrequency;
    }

    private void UpdateCameraReference() {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (!virtualCamera) {
            Debug.LogWarning("Couldn't find virtual camera in scene!");
            return;
        }

        perlinNoise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        if (!perlinNoise) {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component is missing from the camera!");
        }
    }
}
