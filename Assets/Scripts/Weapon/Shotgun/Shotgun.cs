using System;
using Enemies;
using Players;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : WeaponClass
{

    [Header("Shotgun Stats")]
    [SerializeField] public int maxShells;
    [SerializeField] public float blastAngle; // Deg, The total angle with the halfway point being player's aim
    [SerializeField] public float blastRadius;
    [SerializeField] public int radiusSubdivisions;
    [SerializeField] public int maxEnemies; // To damage within the blastArea

    public int shells { get; private set; }

    private PolygonCollider2D blastArea;

    private void Start()
    {
        shells = maxShells;

        blastArea = GetComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[radiusSubdivisions + 1];
        
        points[radiusSubdivisions] = Vector2.zero;
        for (int i = 0; i < radiusSubdivisions; i++)
        {
            var deg = blastAngle / 2 - (float) i / radiusSubdivisions * blastAngle;
            Vector2 point = new Vector2(Mathf.Cos(deg * Mathf.PI / 180f), Mathf.Sin(deg * Mathf.PI / 180f)) * blastRadius;
            points[i] = point;
        }

        blastArea.points = points;
        blastArea.SetPath(0, points);
    }

    protected override void HandleAttack()
    {
        if (shells <= 0) {
            // TODO Reload mechanic
            shells = maxShells;
            return;
        }

        --shells;
        DamageEnemiesInArea();
    }

    public override void UseSkill()
    {
        // TODO Skill
    }

    public override void IntroSkill()
    {
        // TODO Recoil player and damage markiplier?
        DamageEnemiesInArea();
    }

    void Update()
    {
        HandleWeaponRotation();
    }

    private void HandleWeaponRotation() {
        Vector2 facing = player.GetComponent<Player>().facing;

        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        bool shouldFlip = angle > 90 || angle < -90;
        foreach (SpriteRenderer sr in children)
        {
            sr.flipY = shouldFlip;
        }
    }

    private void DamageEnemiesInArea() {

        Collider2D[] hitEnemies = new Collider2D[maxEnemies];
        ContactFilter2D filter = new ContactFilter2D { layerMask = enemyLayer, useLayerMask = true };
        int n = Physics2D.OverlapCollider(blastArea, filter, hitEnemies);
        for (int i = 0; i < n; i++)
        {
            // Maybe not the best, requires that the shotgun has line of sight to *center* of enemy
            if (!Physics2D.Linecast(transform.position, hitEnemies[i].transform.position, groundLayer))
            {
                // TODO Damage falloff?
                hitEnemies[i].GetComponent<Enemy>().Hurt(damage);
            }
        }
    }
}
