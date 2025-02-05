using System.Collections;
using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    public GameObject player;
    [Header("Reference Layers")]
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Weapon Base Stats")]
    public float damage;
    public float attackCooldown; // seconds
    public float damageToFlowRatio = 1; // determines the rate at which damage is translated into flow.

    private bool canAttack = true;

    public void Attack() {
        if (!canAttack) {
            return;
        }
        StartCoroutine(cooldown());
        HandleAttack();
    }

    protected abstract void HandleAttack();

    public abstract void UseSkill();
    public abstract void IntroSkill();

    public void SetSpriteActive(bool active) {
        GetComponentInChildren<SpriteRenderer>().enabled = active;
    }

    IEnumerator cooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
