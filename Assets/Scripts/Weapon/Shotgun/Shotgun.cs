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
using System.Collections.Generic;

public class Shotgun : WeaponClass {

    private const bool DEBUG_RAYS = true;

    [Header("Shotgun Stats")]
    [SerializeField] public float reloadTime; // Seconds
    [SerializeField] public float blastAngle; // Deg, The total angle with the halfway point being player's aim
    [SerializeField] public float range;
    [SerializeField] public int subDivide; // Number of raycasts that divide up the damage

    [SerializeField] internal GameObject muzzleFlash;
    [SerializeField] internal GameObject tracer;
    [SerializeField] internal GameObject barrelPosition;

    [SerializeField] internal float skillForce;

    [SerializeField] private float skillLockDuration = 1f;
    [SerializeField] private float introDelayTime = 0.5f;

    private float skillLockTimer = 0f;

    private Coroutine reloadCoroutine;

    protected override void HandleAttack() {
        Shoot();
    }

    public override bool UseSkill() {
        transform.rotation = Quaternion.Euler(0, 0, 270);
        skillLockTimer = skillLockDuration;

        Shoot();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = new Vector2(rb.velocity.x, skillForce);
        }

        return true;
    }

    public override void IntroSkill() {
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine() {
        yield return new WaitForSeconds(introDelayTime);
        Shoot();
        Shoot();
        Shoot();
        player.GetComponent<Player>().UseRecoil(10);
        ScreenShakeManager.Instance.Shake(3f);
    }

    private void Update() {
        if (skillLockTimer > 0) {
            skillLockTimer -= Time.deltaTime;
        } else {
            HandleWeaponRotation();
        }
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

    private void ShootUltimate() {
        var hits = PerformRayCasts();
        ScreenShakeManager.Instance.Shake(intensity: 2f);
        ApplyDamage(hits, damage,  false); // Ultimate shot ignores drop-off
        HandleTracers();
    }

    private void Shoot() {
        if (CurrentAmmo <= 0) {
            return;
        }
        AudioManager.Instance.PlaySFX(AudioTracks.Scrapgun);

        --CurrentAmmo;

        var hits = PerformRayCasts();
        ApplyDamage(hits, damage, true); // Regular shot uses drop-off

        HandleMuzzleFlash();

        HandleTracers();
        HandleReload();
    }

    private List<RaycastHit2D> PerformRayCasts() {
        Vector2 origin = transform.position;
        float halfAngle = blastAngle / 2;
        float angleStep = blastAngle / (subDivide - 1);
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        for (int i = 0; i < subDivide; i++) {
            float currentAngle = -halfAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, groundLayer | hittableLayerMask);

            if (hit.collider != null) {
                hits.Add(hit);
            }

            if (DEBUG_RAYS) {
                Debug.DrawRay(origin, hit.collider == null ? direction * range : hit.point - origin, Color.red, 1.0f);
            }
        }

        return hits;
    }

    private void ApplyDamage(List<RaycastHit2D> hits, float baseDamage, bool applyDropOff) {
        Vector2 origin = transform.position;
        Player playerScript = player.GetComponent<Player>();

        foreach (var hit in hits) {
            if (hit.collider != null && CollisionUtils.IsLayerInMask(hit.collider.gameObject.layer, hittableLayerMask)) {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    float damageAmount = baseDamage / subDivide;
                    if (applyDropOff) {
                        float distance = Vector2.Distance(origin, hit.point);
                        damageAmount *= DamageDropOff(distance);
                    }

                    damageAmount = Math.Max(damageAmount, 0);
                    enemy.Hurt(damageAmount);
                    playerScript.OnHit(damageAmount * damageToFlowRatio);
                }
            }
        }
    }

    private float DamageDropOff(float distance) {
        return Math.Max((1 - distance / range), 0);
    }

    private void HandleMuzzleFlash() {
        Instantiate(muzzleFlash, barrelPosition.transform.position, transform.rotation);
    }

    private void HandleTracers() {
        Vector2 origin = barrelPosition.transform.position;
        for (int i = 0; i < subDivide; i++) {
            float randomAngle = MathUtils.RandomGaussian(-blastAngle / 2, blastAngle / 2);
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * transform.right;

            Tracer instance = Instantiate(tracer, origin, Quaternion.identity).GetComponent<Tracer>();
            instance.Initialize(origin, direction, range, hittableLayerMask | groundLayer);
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
