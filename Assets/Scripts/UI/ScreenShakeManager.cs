using System.Collections;
using Cinemachine;
using UnityEngine;
using Utils;

public class ScreenShakeManager : PersistentSingleton<ScreenShakeManager> {
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    protected override void OnAwake() {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        if (perlinNoise == null) {
            Debug.LogError("CinemachineBasicMultiChannelPerlin is missing from the camera!");
        }
    }

    public void Shake(float intensity = 1.0f, float duration = 0.5f) {
        if (perlinNoise != null) {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        } else {
            Debug.LogError("Perlin noise module is missing!");
        }
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration) {
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
}
