using System;
using System.Collections;
using Players;
using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    public GameObject player;
    [Header("Reference Layers")]
    public LayerMask hittableLayerMask; //Typically Enemies and Destructables
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Weapon Base Stats")]
    public float damage;
    public float attackCooldown; // seconds
    public float damageToFlowRatio = 1; // determines the rate at which damage is translated into flow.

    [SerializeField] public int maxAmmoCount;

    private int _currentAmmo;
    public int CurrentAmmo {
        get => _currentAmmo;
        set {
            if (_currentAmmo != value) {
                _currentAmmo = value;
                Debug.Log("Changed current ammo to " + value);
                if (gameObject.activeInHierarchy) {
                    OnAmmoChanged?.Invoke(_currentAmmo);
                }
            }
        }
    }

    public static event Action<int> OnAmmoChanged;

    private bool canAttack = true;
    protected Player playerComponent;

    public virtual void Start() {
        playerComponent = player.GetComponent<Player>();
        CurrentAmmo = maxAmmoCount;
    }

    public void Attack() {
        if (!canAttack) {
            return;
        }
        StartCoroutine(cooldown());
        HandleAttack();
    }

    protected abstract void HandleAttack();

    public abstract void UseSkill();
    public abstract void IntroSkill();

    public void SetSpriteActive(bool active) {
        GetComponentInChildren<SpriteRenderer>().enabled = active;
    }

    IEnumerator cooldown() {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
