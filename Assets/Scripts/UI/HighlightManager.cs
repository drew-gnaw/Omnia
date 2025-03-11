using UnityEngine;
using Utils;

namespace UI
{
    public class HighlightManager : PersistentSingleton<HighlightManager>
    {
        [SerializeField] private Material highlightMaterial;

        private int cutoutPosID;
        private int cutoutSizeID;
        private int cutoutEnabledID;

        protected override void OnAwake()
        {
            cutoutPosID = Shader.PropertyToID("_CutoutPosition");
            cutoutSizeID = Shader.PropertyToID("_CutoutSize");
            cutoutEnabledID = Shader.PropertyToID("_CutoutEnabled");

            HideHighlight(); // Start disabled
        }

        public void HighlightObject(GameObject target, float size = 0.2f)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            Highlight(screenPos, size);
        }

        public void Highlight(Vector2 screenPosition, float size = 0.2f)
        {
            Vector2 uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);

            highlightMaterial.SetVector(cutoutPosID, uv);
            highlightMaterial.SetFloat(cutoutSizeID, size);
            highlightMaterial.SetFloat(cutoutEnabledID, 1f); // Enable effect
        }

        public void HideHighlight()
        {
            highlightMaterial.SetFloat(cutoutEnabledID, 0f); // Disable effect
        }
    }
}
