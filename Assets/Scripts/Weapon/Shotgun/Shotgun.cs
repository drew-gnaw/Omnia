using System;
using System.Collections;
using Enemies;
using Omnia.Utils;
using Players;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : WeaponClass {

    private const bool DEBUG_RAYS = true;

    [Header("Shotgun Stats")]
    [SerializeField] public int maxShells;
    [SerializeField] public float reloadTime; // Seconds
    [SerializeField] public float blastAngle; // Deg, The total angle with the halfway point being player's aim
    [SerializeField] public float range;
    [SerializeField] public int subDivide; // Number of raycasts that divide up the damage

    [SerializeField] private Material tracerMaterial;

    public int shells { get; private set; }

    private Coroutine reloadCoroutine;

    private void Start() {
        shells = maxShells;
    }

    protected override void HandleAttack() {
        Shoot();
    }

    public override void UseSkill() {
        // TODO Skill
    }

    public override void IntroSkill() {
        // TODO Recoil player and damage markiplier?
        Shoot();
    }

    void Update() {
        HandleWeaponRotation();
    }

    private void HandleWeaponRotation() {
        Vector2 facing = player.GetComponent<Player>().facing;

        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        bool shouldFlip = angle > 90 || angle < -90;
        foreach (SpriteRenderer sr in children) {
            sr.flipY = shouldFlip;
        }
    }

    private void Shoot() {
        if (shells <= 0) {
            return;
        }

        --shells;

        HandleRayCasts();

        HandleTracers();

        HandleReload();
    }

    private void HandleRayCasts() {
        Vector2 origin = transform.position;
        float halfAngle = blastAngle / 2;
        float angleStep = blastAngle / (subDivide - 1);
        for (int i = 0; i < subDivide; i++) {
            float currentAngle = -halfAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, groundLayer | enemyLayer);

            if (hit.collider != null && CollisionUtils.isLayerInMask(hit.collider.gameObject.layer, enemyLayer)) {
                // Apply damage to the enemy
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    float distance = Vector2.Distance(origin, hit.point);
                    enemy.Hurt(DamageDropOff(distance));
                    player.GetComponent<Player>().OnHit(10);
                }
            }

            if (DEBUG_RAYS) {
                if (hit.collider == null) {
                    Debug.DrawRay(origin, direction * range, Color.red, 1.0f);
                } else {
                    Debug.DrawRay(origin, hit.point - origin, Color.red, 1.0f);
                }
            }
        }
    }

    private float DamageDropOff(float distance) {
        return Math.Max(damage / subDivide * (1 - distance / range), 0);
    }

    // TODO TEMP TRACER until assets consolidated
    private void HandleTracers() {
        Vector2 origin = transform.position;
        for (int i = 0; i < subDivide; i++) {
            float randomAngle = UnityEngine.Random.Range(-blastAngle / 2, blastAngle / 2);
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, groundLayer | enemyLayer);

            LineRenderer line = new GameObject("Tracer").AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, origin);
            line.SetPosition(1, hit.collider != null ? hit.point : direction * range + origin);
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            line.material = tracerMaterial;
            Destroy(line.gameObject, 0.1f);
        }
    }
    private void HandleReload() {
        Debug.Log("Shotgun shells: " + shells);

        if (reloadCoroutine != null) {
            StopCoroutine(reloadCoroutine);
        }
        if (shells < maxShells) {
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        shells += 1;
        reloadCoroutine = null;
        HandleReload();
    }
}
