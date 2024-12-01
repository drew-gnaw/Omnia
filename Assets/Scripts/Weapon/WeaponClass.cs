using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    public GameObject player;
    [Header("Reference Layers")]
    public LayerMask enemyLayer;

    [Header("Weapon Base Stats")]
    public int damage;

    public abstract void Attack();
    public abstract void UseSkill();
    public abstract void IntroSkill();
}