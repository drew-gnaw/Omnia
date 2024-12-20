using UnityEngine;
using Enemies;

public class Mace : WeaponClass
{
    Transform attackPoint;
    float radius;
    
    public override void Attack()
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
}