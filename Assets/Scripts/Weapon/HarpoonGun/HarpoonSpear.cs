using Unity.VisualScripting;
using UnityEngine;
using Enemies;
using Omnia.Utils;
/*
    The projectile for HarpoonGun
    This class should only be interacted upon by its prefabs and HarpoonGun
*/
public class HarpoonSpear : MonoBehaviour {
    public LayerMask enemyLayer;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    public Collider2D Collider2D;
    public Rigidbody2D Rigidbody2D;

    private bool dropped;
    private HarpoonGun gun;

    // Tracking enemy
    public Enemy TaggedEnemy { get; private set;}

    public void Awake() {
        this.dropped = false;
        this.TaggedEnemy = null;
    }

    public void OnEnable() {
        Enemy.Death += HandleEnemyDeath;
    }

    public void OnDisable() {
        Enemy.Death -= HandleEnemyDeath;
    }

    // Fires the spear in the rotation of the gun with its velocity
    public void Fire(HarpoonGun gun) {
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

        // TODO
    }

    public void Update() {
        var rb = GetComponent<Rigidbody2D>();

        // Rotate based on velocity
        if (!dropped && rb.velocity != Vector2.zero) {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (Collider2D.IsTouchingLayers(playerLayer) && dropped) {
            Debug.Log("HarpoonSpear collected by player");
            HandlePlayerCollision();
        }
        // FIXME: for whatever reason, checking if the spear collider is touching enemy does not always work,
        // other collider ends up as null if they are moving
        if (CollisionUtils.isLayerInMask(other.gameObject.layer, enemyLayer) && !dropped) {
            Debug.Log("HarpoonSpear hit enemy");
            HandleEnemyCollision(other.GetComponent<Enemy>());
        }
        if (Collider2D.IsTouchingLayers(groundLayer) && !dropped) {
            Debug.Log("HarpoonSpear hit ground");
            HandleGroundCollision();
        }
    }

    private void HandlePlayerCollision() {
        Unfreeze();
        // Tell gun to mark this spear as available
        gun.SpearCollected(this);
    }

    private void HandleEnemyCollision(Enemy enemy) {
        Freeze();

        this.TaggedEnemy = enemy;

        // HingeJoints are created during runtime as it can't be disabled
        HingeJoint2D hj = this.AddComponent<HingeJoint2D>();
        hj.connectedBody = TaggedEnemy.GetComponent<Rigidbody2D>();

        TaggedEnemy.GetComponent<Enemy>().Hurt(gun.damage);
    }

    private void HandleGroundCollision() {
        Freeze();
    }

    private void HandleEnemyDeath(Enemy enemy) {
        if (enemy == TaggedEnemy) {
            Unfreeze();
        }
    }

    // Unfreezes the spear
    private void Unfreeze() {
        Rigidbody2D.gravityScale = 1 * gun.harpoonSpearGravityScale;
        Rigidbody2D.freezeRotation = false;
        dropped = false;
        
        Destroy(GetComponent<HingeJoint2D>());
    }

    // Freezes the spear and marks as collectable
    private void Freeze() {
        Rigidbody2D.gravityScale = 0;
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.freezeRotation = true;
        dropped = true;
    }
}