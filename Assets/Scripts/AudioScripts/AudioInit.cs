using UnityEngine;

public static class AudioInit
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializePrefab()
    {
        // Load the prefab from the Resources folder
        GameObject prefab = Resources.Load<GameObject>("AudioManager");

        if (prefab != null)
        {
            // Instantiate the prefab at the origin with no rotation
            GameObject instance = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(instance);
            Debug.Log("AudioManager instantiated successfully.");
        }
        else
        {
            Debug.LogError("AudioManager not found in Resources folder.");
        }
    }
}
