using System.Collections.Generic;
using System.Linq;
using Enemies;
using Players;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class HarpoonGun : WeaponClass
{

    [FormerlySerializedAs("harpoons")]
    [Header("HarpoonGun Stats")]
    [SerializeField] public float harpoonVelocity;
    [SerializeField] public float harpoonSpearGravityScale;
    [SerializeField] public float harpoonSpearPickupCooldown; // seconds
    [SerializeField] public float collectionRadius;
    [SerializeField] public float harpoonTimer; // seconds
    [SerializeField] public float spearReturnSpeed;
    [SerializeField] public float pullPower;

    [Header("HarpoonGun References")]
    public Transform gunBarrelTransform;
    public GameObject harpoonSpearPrefab;
    public GameObject harpoonRopePrefab;
    private ObjectPool<HarpoonSpear> harpoonSpearPool;

    [SerializeField] internal GameObject muzzleFlash;
    [SerializeField] internal GameObject barrelPosition;

    // Assuming number of spears isn't too big
    LinkedList<HarpoonSpear> firedSpears = new LinkedList<HarpoonSpear>();

    [Header("HarpoonGun Visuals")]
    public Material ropeMaterial;  // Assign a brown material for the rope
    public float ropeWidth = 0.1f; // Thickness of the rope

    override public void Start()
    {
        harpoonSpearPool = new ObjectPool<HarpoonSpear>(
            // Create
            () => {
                GameObject spearObject = Instantiate(harpoonSpearPrefab, transform.position, transform.rotation);
                return spearObject.GetComponent<HarpoonSpear>();
            },
            // OnGet
            (HarpoonSpear spear) => {
                spear.gameObject.SetActive(true);
            },
            // OnRelease
            (HarpoonSpear spear) => {
                spear.gameObject.SetActive(false);
            },
            // OnDestroy
            (HarpoonSpear spear) => {
                Destroy(spear.gameObject);
            },
            true,
            maxAmmoCount,
            maxAmmoCount
        );
        base.Start();
    }

    protected override void HandleAttack()
    {
        if (firedSpears.Count >= maxAmmoCount) {
            // Do nothing
            return;
        }
        HarpoonSpear spear = harpoonSpearPool.Get();
        spear.Fire(this);
        firedSpears.AddFirst(spear);

        GameObject ropeObject = new GameObject("HarpoonRope");
        HarpoonRope rope = ropeObject.AddComponent<HarpoonRope>();
        rope.Initialize(gunBarrelTransform.right, gunBarrelTransform, spear.transform, harpoonRopePrefab); 
        spear.rope = rope; 
        CurrentAmmo--;
        Instantiate(muzzleFlash, barrelPosition.transform.position, transform.rotation);
    }

    public override bool UseSkill()
    {
        if (firedSpears.Count == 0) {
            return false;
        }

        var spear = firedSpears.FirstOrDefault(s => s.TaggedEnemy != null || s.PullTo != null);

        Transform target = spear?.PullTo ?? spear?.TaggedEnemy?.transform;

        if (target == null) {
            return false;
        }

        playerComponent.UsePull(target);
        AudioManager.Instance.PlaySFX(AudioTracks.HarpoonRetract);
        return true;
    }

    public override void IntroSkill()
    {
        // Pull all enemies
        foreach (var spear in firedSpears) {
            spear.ReturnToPlayer();
            spear.PullEnemy();
        }
    }

    void Update()
    {
        HandleWeaponRotation();

        foreach (var spear in firedSpears) {
            if (spear.IsCollectable &&
                    Vector2.Distance(playerComponent.Center, spear.transform.position) <= collectionRadius)
            {
                spear.ReturnToPlayer();
            }

        }
    }

    public void SpearCollected(HarpoonSpear spear) {
        if (spear.rope != null) {
            spear.rope.DestroyRope(); // Remove rope when spear is collected
            spear.rope = null;
        }

        harpoonSpearPool.Release(spear);
        firedSpears.Remove(spear);
        CurrentAmmo++;
    }

    public void SpearCollectAll() {
        foreach (var spear in firedSpears) {
            SpearCollected(spear);
        }
        CurrentAmmo = maxAmmoCount;
    }

    private void HandleWeaponRotation() {
        Vector2 facing = playerComponent.facing;

        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        bool shouldFlip = angle > 90 || angle < -90;
        foreach (SpriteRenderer sr in children)
        {
            sr.flipY = shouldFlip;
        }
    }
}
