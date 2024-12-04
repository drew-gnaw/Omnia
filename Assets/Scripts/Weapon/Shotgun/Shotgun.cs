using Players;
using UnityEngine;

public class Shotgun : WeaponClass
{

    [Header("Shotgun Stats")]
    [SerializeField] public int shells = 6;
    [SerializeField] public int spreadAngleDeg = 30;

    private void Start()
    {

    }

    protected override void HandleAttack()
    {

    }

    public override void UseSkill()
    {

    }

    public override void IntroSkill()
    {

    }

    void Update()
    {
        HandleWeaponRotation();
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
