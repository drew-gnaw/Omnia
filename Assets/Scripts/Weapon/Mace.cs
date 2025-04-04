using UnityEngine;
using Enemies;
using Players;

public class Mace : WeaponClass
{
    [Header("Mace Stats")]
    [SerializeField] Transform AttackPoint;
    [SerializeField] float Radius;

    protected override void HandleAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, Radius, hittableLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            bool isCrit = Random.Range(0f, 1f) < player.GetComponent<Player>().critChance;
            enemy.GetComponent<Enemy>().Hurt(
                damage * (isCrit ? player.GetComponent<Player>().critMultiplier : 1),
                crit: isCrit
            );
        }
    }

    public override bool UseSkill() {
        return true;
    }

    public override void IntroSkill() {

    }

    void Update() {
        HandleWeaponRotation();
    }

    private void HandleWeaponRotation() {
        Vector2 facing = player.GetComponent<Player>().facing;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facing.x < 0 ? -1 : 1);
        transform.localScale = scale;
    }
}
