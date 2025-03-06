using Enemies;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class Shield : MonoBehaviour {
    [SerializeField] internal float maxShieldHealth;
    [SerializeField] internal float shieldHealth;
    private Enemy self;

    // Start is called before the first frame update
    private void Start() {
        shieldHealth = maxShieldHealth;
        self = GetComponent<Enemy>();
        self.OnHurt += OnHit;
    }

    private float OnHit(float damage) {
        if (shieldHealth <= 0) {
            self.OnHurt -= OnHit;
            return damage;
        }
        shieldHealth = Mathf.Max(shieldHealth - damage, 0);
        return 0; // Prevent overflow
    }

    private void OnDestroy() {
        if (self == null) return;
        self.OnHurt -= OnHit;
    }
}
