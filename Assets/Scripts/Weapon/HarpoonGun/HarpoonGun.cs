using System.Collections.Generic;
using System.Linq;
using Enemies;
using Players;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class HarpoonGun : WeaponClass
{

    [Header("HarpoonGun Stats")]
    [SerializeField] public int harpoons;
    [SerializeField] public float harpoonVelocity;
    [SerializeField] public float harpoonSpearGravityScale;
    [SerializeField] public float harpoonSpearPickupCooldown; // seconds

    [Header("HarpoonGun References")]
    public GameObject harpoonSpearPrefab;

    private ObjectPool<HarpoonSpear> harpoonSpearPool;

    // Assuming number of spears isn't too big
    LinkedList<HarpoonSpear> firedSpears = new LinkedList<HarpoonSpear>();

    private void Start()
    {
        Debug.Log(damage);
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
            harpoons,
            harpoons
        );
    }

    protected override void HandleAttack()
    {
        if (firedSpears.Count >= harpoons) {
            // Do nothing
            return;
        }
        HarpoonSpear spear = harpoonSpearPool.Get();
        spear.Fire(this);
        firedSpears.AddFirst(spear);
    }

    public override void UseSkill()
    {
        if (firedSpears.Count == 0) {
            return;
        }

        var spear = firedSpears.FirstOrDefault(s => s.TaggedEnemy != null || s.PullTo != null);

        Transform target = spear?.PullTo ?? spear?.TaggedEnemy?.transform;

        if (target == null) {
            return;
        }

        player.GetComponent<Player>().UsePull(target);
    }


    public override void IntroSkill()
    {
        // Pull all enemies
        foreach (var spear in firedSpears)
        {
            spear.PullEnemy();
        }
    }

    void Update()
    {
        HandleWeaponRotation();
    }

    public void SpearCollected(HarpoonSpear spear) {
        harpoonSpearPool.Release(spear);
        firedSpears.Remove(spear);
    }

    public void SpearCollectAll() {
        foreach (var spear in firedSpears) {
            SpearCollected(spear);
        }
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
}
