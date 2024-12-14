using System.Collections;
using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    public GameObject player;
    [Header("Reference Layers")]
    public LayerMask enemyLayer;

    [Header("Weapon Base Stats")]
    public int damage;
    public float attackCooldown; // seconds

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

    IEnumerator cooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}