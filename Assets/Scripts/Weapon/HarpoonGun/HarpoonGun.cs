using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class HarpoonGun : WeaponClass
{

    [Header("HarpoonGun Stats")]
    [SerializeField] public int harpoons = 3;
    [SerializeField] public float harpoonVelocity = 20;
    [SerializeField] public float harpoonSpearGravityScale = 1;
    [SerializeField] public GameObject harpoonSpearPrefab;

    // Assuming harpoons isn't too great, otherwise will use HashSet
    LinkedList<HarpoonSpear> spears = new LinkedList<HarpoonSpear>();

    public override void Attack()
    {
        if (spears.Count >= harpoons) {
            // Do nothing
            return;
        }

        GameObject spearObject = Instantiate(harpoonSpearPrefab, transform.position, transform.rotation);
        HarpoonSpear spear = spearObject.GetComponent<HarpoonSpear>();
        spears.AddFirst(spear);
        spear.Fire(this);
    }

    public override void UseSkill()
    {
        if (spears.Count == 0) {
            return;
        }

        // Find the next most recent spear that has tagged enemy
        Enemy tagged = null;
        foreach (var spear in spears) {
            if (spear.TaggedEnemy != null) {
                tagged = spear.TaggedEnemy;
                break;
            }
        }

        if (tagged == null) {
            return;
        }

        // TODO Pull to most recent
    }

    public override void IntroSkill() 
    {
        // Pull all enemies
        foreach (var spear in spears)
        {
            spear.PullEnemy();
        }
    }

    void Update()
    {
        HandleWeaponRotation();
    }

    public void SpearCollected(HarpoonSpear spear) {
        spears.Remove(spear);
    }


    private void HandleWeaponRotation() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)
        );
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // TODO temporary solution to flipping the gun when pointing left/right
        // until design/art team as consolidated on style
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        if (angle > 90 || angle < -90) {
            foreach (SpriteRenderer sr in children)
            {
                sr.flipY = true;
            }
        } else {
            foreach (SpriteRenderer sr in children)
            {
                sr.flipY = false;
            }
        }
    }
}