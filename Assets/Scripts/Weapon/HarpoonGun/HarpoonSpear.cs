using Unity.VisualScripting;
using UnityEngine;
using Enemies;
using Omnia.Utils;
using Players;

using System.Collections;

/*
    The projectile for HarpoonGun
    This class should only be interacted upon by its prefabs and HarpoonGun
*/
public class HarpoonSpear : MonoBehaviour {
    public LayerMask hittableLayerMask;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    public LayerMask semisolidLayer;

    public Collider2D Collider2D;
    public Rigidbody2D Rigidbody2D;

    private bool dropped;
    private bool collectable;
    private IEnumerator cooldown;
    private HarpoonGun gun;
    private Player player;
    private bool playerAbsorb;
    private IEnumerator absorbCooldown;

    // Tracking enemy
    public Enemy TaggedEnemy { get; private set; }
    public Transform PullTo { get; private set; }

    public bool IsCollectable => collectable;

    public void Awake() {
        dropped = false;
        TaggedEnemy = null;
        PullTo = null;
        playerAbsorb = false;

        player = GameObject.Find("Player")?.GetComponent<Player>();
        if (player == null) {
            Debug.LogWarning("Player not found or Player script is not attached to the GameObject.");
        }
    }

    public void OnEnable() {
        Enemy.Death += HandleEnemyDeath;
    }

    public void OnDisable() {
        Enemy.Death -= HandleEnemyDeath;
    }

    // Fires the spear in the rotation of the gun with its velocity
    public void Fire(HarpoonGun gun) {
        AudioManager.Instance.PlaySFX(AudioTracks.HarpoonLaunch);
        this.gun = gun;

        gameObject.SetActive(true);

        // Temporary fire from center of player until idea consolidated
        transform.position = gun.transform.position;
        transform.rotation = gun.transform.rotation;

        Rigidbody2D.velocity = gun.transform.right * gun.harpoonVelocity;
    }

    public void PullEnemy() {
        if (TaggedEnemy == null) {
            return;
        }
        Vector2 difference = (player.Center - transform.position).normalized;
        TaggedEnemy.GetComponent<Rigidbody2D>().AddForce(difference * gun.pullPower);
        AudioManager.Instance.PlaySFX(AudioTracks.HarpoonRetract);
        TaggedEnemy.GetComponent<Enemy>().Hurt(gun.damage);
    }

    public void ReturnToPlayer() {
        collectable = true;
        playerAbsorb = true;
    }

    public void Update() {
        // Rotate based on velocity
        if (!dropped && Rigidbody2D.velocity != Vector2.zero) {
            float angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        if (playerAbsorb) {
            transform.position = Vector2
                .MoveTowards(transform.position, player.Center, gun.spearReturnSpeed * Time.deltaTime);

            // Rotate over Z axis to face away from Player
            Vector3 difference = transform.position - player.Center;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (Collider2D.IsTouchingLayers(playerLayer) && collectable) {
            HandlePlayerCollision();
        }
        // FIXME: for whatever reason, checking if the spear collider is touching enemy does not always work,
        // other collider ends up as null if they are moving
        if (CollisionUtils.IsLayerInMask(other.gameObject.layer, hittableLayerMask) && !dropped) {
            AudioManager.Instance.PlaySFX(AudioTracks.HarpoonHit);
            HandleEnemyCollision(other.GetComponent<Enemy>());
        }

        if (Collider2D.IsTouchingLayers(semisolidLayer) && !dropped) {
            AudioManager.Instance.PlaySFX(AudioTracks.HarpoonHit);
            HandleSemisolidCollision(other.gameObject);
        }

        if (Collider2D.IsTouchingLayers(groundLayer) && !dropped) {
            AudioManager.Instance.PlaySFX(AudioTracks.HarpoonHit);
            HandleGroundCollision(other.gameObject);
        }
    }

    private void HandlePlayerCollision() {
        Unfreeze();
        playerAbsorb = false;
        collectable = false;
        PullTo = null;
        TaggedEnemy = null;

        // Tell gun to mark this spear as available
        gun.SpearCollected(this);
    }

    private void HandleEnemyCollision(Enemy enemy) {
        Freeze();
        StartCooldown();
        StartHarpoonTimer();

        TaggedEnemy = enemy;
        AttachToRigidBody(TaggedEnemy.GetComponent<Rigidbody2D>());
        TaggedEnemy.GetComponent<Enemy>().Hurt(gun.damage);
        player?.OnHit(gun.damage * gun.damageToFlowRatio);
    }

    private void HandleSemisolidCollision(GameObject semi) {
        Freeze();
        AttachToRigidBody(semi.GetComponent<Rigidbody2D>());
        PullTo = gameObject.transform;
        StartCooldown();
        StartHarpoonTimer();
    }

    private void HandleGroundCollision(GameObject ground) {
        Freeze();
        AttachToRigidBody(ground.GetComponent<Rigidbody2D>());
        StartCooldown();
        StartHarpoonTimer();
    }

    // To make the spear move, the hit object should have a rigidbody
    private void AttachToRigidBody(Rigidbody2D rb) {
        if (!rb) return;
        // HingeJoints are created during runtime as it can't be disabled
        HingeJoint2D hj = this.AddComponent<HingeJoint2D>();
        hj.connectedBody = rb;
    }

    private void HandleEnemyDeath(Enemy enemy) {
        if (enemy == TaggedEnemy) {
            TaggedEnemy = null;
            Unfreeze();
        }
    }

    // Unfreezes the spear
    private void Unfreeze() {
        if (cooldown != null) {
            StopCoroutine(cooldown);
        }


        Rigidbody2D.gravityScale = 1 * gun.harpoonSpearGravityScale;
        Rigidbody2D.freezeRotation = false;
        dropped = false;

        Destroy(GetComponent<HingeJoint2D>());
    }

    // Freezes the spear and marks as collectable
    private void Freeze() {
        if (cooldown != null) {
            StopCoroutine(cooldown);
        }

        Rigidbody2D.gravityScale = 0;
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.freezeRotation = true;
        dropped = true;
    }

    // Begin pickup cooldown at the first spear contact, after that free to collect anytime
    private void StartCooldown() {
        if (collectable) return;

        if (cooldown != null) {
            StopCoroutine(cooldown);
        }
        cooldown = DropCooldown();
        StartCoroutine(cooldown);
    }

    private IEnumerator DropCooldown() {
        yield return new WaitForSeconds(gun.harpoonSpearPickupCooldown);
        collectable = (TaggedEnemy == null);
    }

    private void StartHarpoonTimer() {
        if (playerAbsorb) return;

        if (absorbCooldown != null) {
            StopCoroutine(absorbCooldown);
        }
        absorbCooldown = DropHarpoonTimer();

        try {
            StartCoroutine(absorbCooldown);
        } catch (System.Exception e) {
            Debug.LogError($"Failed to start coroutine: {e.Message}");
        }
    }

    private IEnumerator DropHarpoonTimer() {
        yield return new WaitForSeconds(gun.harpoonTimer);
        ReturnToPlayer();
    }
}
