using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Spawn?.Invoke(this);
    }
    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log("Enemy died.");
        Death?.Invoke(this);
        Destroy(gameObject);
    }

    public static event Action<EnemyBase> Spawn;
    public static event Action<EnemyBase> Death;
}
