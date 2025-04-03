using Players.Buff;
using Players.Fragments;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Descent {
    public class FragmentChoice : MonoBehaviour {
        [SerializeField] private Image fragmentImage;
        private Fragment fragment;
        private Color originalColor;

        public void SetFragment(Fragment frag) {
            fragment = frag;
        }

        private void Start() {
            if (fragmentImage == null) {
                fragmentImage = GetComponent<Image>();
            }
            originalColor = fragmentImage.color;
        }

        public void OnPointerEnter() {
            if (fragmentImage != null) {
                fragmentImage.color = new Color(originalColor.r * 0.7f, originalColor.g * 0.7f, originalColor.b * 0.7f, originalColor.a); // Darken by 30%
            }
        }

        public void OnPointerExit() {
            if (fragmentImage != null) {
                fragmentImage.color = originalColor;
            }
        }

        public void OnClick() {
            if (fragment != null) {
                BuffManager.Instance.ApplyBuff(fragment);
                LevelManager.Instance.NextLevel();
            }
        }
    }
}
