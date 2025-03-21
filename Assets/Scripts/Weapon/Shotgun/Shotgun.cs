using System;
using System.Collections;
using Enemies;
using Omnia.Utils;
using Utils;
using Players;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using MathUtils = Utils.MathUtils;

public class Shotgun : WeaponClass {

    private const bool DEBUG_RAYS = true;

    [Header("Shotgun Stats")]
    [SerializeField] public float reloadTime; // Seconds
    [SerializeField] public float blastAngle; // Deg, The total angle with the halfway point being player's aim
    [SerializeField] public float range;
    [SerializeField] public int subDivide; // Number of raycasts that divide up the damage
    [SerializeField] public Transform muzzlePosition;
    [SerializeField] public GameObject tracerPrefab;

    private Coroutine reloadCoroutine;

    protected override void HandleAttack() {
        Shoot();
    }

    public override void UseSkill() {
        // TODO Skill
    }

    public override void IntroSkill() {
        Shoot();
        player.GetComponent<Player>().UseRecoil(10);
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
        if (CurrentAmmo <= 0) {
            return;
        }

        --CurrentAmmo;

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
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, groundLayer | hittableLayerMask);

            if (hit.collider != null && CollisionUtils.IsLayerInMask(hit.collider.gameObject.layer, hittableLayerMask)) {
                // Apply damage to the enemy
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    float distance = Vector2.Distance(origin, hit.point);
                    enemy.Hurt(DamageDropOff(distance));
                    player.GetComponent<Player>().OnHit(damage * damageToFlowRatio);
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

    private void HandleTracers() {
        for (int i = 0; i < subDivide; i++) {
            float randomAngle = MathUtils.RandomGaussian(-blastAngle / 2, blastAngle / 2);
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * muzzlePosition.right;

            Tracer tracer = Instantiate(tracerPrefab, muzzlePosition.position, Quaternion.identity).GetComponent<Tracer>();
            tracer.Initialize(muzzlePosition.position, direction, range, hittableLayerMask | groundLayer);
        }
    }
    private void HandleReload() {

        if (reloadCoroutine != null) {
            StopCoroutine(reloadCoroutine);
        }
        if (CurrentAmmo < maxAmmoCount) {
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        CurrentAmmo += 1;
        reloadCoroutine = null;
        HandleReload();
    }
}
