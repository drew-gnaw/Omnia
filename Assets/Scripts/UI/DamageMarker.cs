using TMPro;
using UnityEngine;

public class DamageMarker : MonoBehaviour {

    [SerializeField] internal float duration = 1f;
    [SerializeField] internal float maxInitialVelocity = 5f;
    [SerializeField] internal float minInitialVelocity = 2f;

    [SerializeField] internal Rigidbody2D rb;
    [SerializeField] internal TextMeshPro textMesh;

    private float start;

    public void Initialize(Vector2 position, float value) {
        start = duration;
        textMesh.alpha=0;

        transform.position = position;
        textMesh.text = value.ToString();
        
        var initVelocity = new Vector2(
            Random.Range(-maxInitialVelocity, maxInitialVelocity),
            Random.Range(minInitialVelocity, maxInitialVelocity)
        );
        rb.rotation = -Mathf.Acos(initVelocity.y / initVelocity.magnitude) * Mathf.Rad2Deg * Mathf.Sign(initVelocity.x);
        rb.velocity = initVelocity;
    }

    public void Update() {
        if (start <= 0) Destroy(gameObject);
        
        start = Mathf.Max(start - Time.deltaTime, 0);
        textMesh.alpha = start / duration;
    }
}