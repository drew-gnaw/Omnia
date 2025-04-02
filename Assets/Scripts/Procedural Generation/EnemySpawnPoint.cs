using UnityEngine;

namespace Procedural_Generation {
    public enum SpawnPointType {
        Ground,
        Air
        // wall (for sundew)
    }

    public class EnemySpawnPoint : MonoBehaviour {


        [SerializeField] public SpawnPointType type;


    }
}
