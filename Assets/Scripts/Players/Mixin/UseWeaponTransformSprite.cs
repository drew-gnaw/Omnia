using UnityEngine;

namespace Players.Mixin {
    public class UseWeaponTransformSprite : MonoBehaviour {
        [SerializeField] internal Player self;
        [SerializeField] internal Transform weapons;

        /* Controlled by the player animator;
           synced with the current animation. */
        [SerializeField] internal Vector2 weaponsPosition;

        public void Update() {
            self.sprite.flipX = self.facing.x == 0 ? self.sprite.flipX : self.facing.x < 0;
            weapons.gameObject.SetActive(self.IsAttackEnabled());

            var flip = self.sprite.flipX ? -1 : 1;
            weapons.localPosition = new Vector3(weaponsPosition.x * flip, weaponsPosition.y, weapons.localPosition.z);
        }
    }
}
