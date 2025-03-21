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

    [SerializeField] internal GameObject muzzleFlash;
    [SerializeField] internal GameObject tracer;
    [SerializeField] internal GameObject barrelPosition;

    [SerializeField] internal float skillForce;

    [SerializeField] private float skillLockDuration = 1f;
    [SerializeField] private float introDelayTime = 1f;

    [SerializeField] private GameObject chargeUpEffectPrefab;

    [SerializeField] private float maxScale = 1.5f; // Maximum size of the charge-up effect
    [SerializeField] private float chargeDuration = 2f; // How long the charge-up effect takes to reach full size
    [SerializeField] private Color startColor = Color.white; // Start color of the effect
    [SerializeField] private Color endColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private float chargeTimer = 0f;

    private float skillLockTimer = 0f;

    private Coroutine reloadCoroutine;

    private GameObject chargeUpInstance;

    protected override void HandleAttack() {
        Shoot();
    }

    public override void UseSkill() {
        transform.rotation = Quaternion.Euler(0, 0, 270);
        skillLockTimer = skillLockDuration;

        Shoot();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = new Vector2(rb.velocity.x, skillForce);
        }
    }

    public override void IntroSkill() {
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine() {

        chargeUpInstance = Instantiate(chargeUpEffectPrefab, barrelPosition.transform.position, Quaternion.identity);
        chargeUpInstance.transform.SetParent(barrelPosition.transform);

        float chargeTimer = 0f;
        while (chargeTimer < introDelayTime)
        {
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        Shoot(false);
        Shoot(false);
        Shoot(false);
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

    private void Shoot(bool consumeAmmo = true) {
        if (consumeAmmo) {
            if (CurrentAmmo <= 0) {
                return;
            }

            --CurrentAmmo;
        }

        HandleRayCasts();

        HandleMuzzleFlash();

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
