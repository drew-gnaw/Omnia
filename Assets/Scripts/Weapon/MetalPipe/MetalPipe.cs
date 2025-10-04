using System.Collections;
using UnityEngine;

namespace Weapon.MetalPipe {
    public class MetalPipe : WeaponClass {

        [SerializeField] private float reloadTime = 0.1f;
        private Coroutine reloadCoroutine;
        protected override void HandleAttack() {

        }

        public override bool UseSkill() {
            return false;
        }

        public override void IntroSkill() {

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
            if (CurrentAmmo >= maxAmmoCount) yield break;
            CurrentAmmo += 1;
            reloadCoroutine = null;
            HandleReload();
        }
    }
}
