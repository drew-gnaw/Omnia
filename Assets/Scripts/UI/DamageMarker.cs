
using UnityEngine;

public class DamageMarker : MonoBehaviour {

    [SerializeField] internal float maxRotationAngle = 0f;
    [SerializeField] internal float duration = 0.5f;
    [SerializeField] internal float maxInitialVelocity = 0.5f;
    [SerializeField] internal float minInitialVelocity = 0.5f;
    [SerializeField] internal float maxInitialAngle = 0.5f;

    [SerializeField] internal Rigidbody2D rb;


    public void Start() {
        rb.velocity = new Vector2(Random.Range(minInitialVelocity, maxInitialVelocity), Random.Range(minInitialVelocity, maxInitialVelocity));
    }

}