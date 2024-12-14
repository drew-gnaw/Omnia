using UnityEngine;
using Enemies;
using Players;

public class Mace : WeaponClass
{
    Transform attackPoint;
    float radius;
    
    protected override void HandleAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, radius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().Hurt(damage);
        }
    }

    public override void UseSkill()
    {
        
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