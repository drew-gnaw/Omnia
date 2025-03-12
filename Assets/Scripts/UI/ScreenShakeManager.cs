using UnityEngine;
using Cinemachine;
using Utils;

public class ScreenShakeManager : PersistentSingleton<ScreenShakeManager> {
    private CinemachineImpulseSource impulseSource;

    protected override void OnAwake() {
        impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    public void Shake(float intensity) {
        if (impulseSource) {
            impulseSource.GenerateImpulse(intensity);
        } else {
            Debug.LogError("CinemachineImpulseSource not found!");
        }
    }
}
