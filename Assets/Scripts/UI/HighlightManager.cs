using UnityEngine;
using Utils;

namespace UI {
    public class HighlightManager : PersistentSingleton<HighlightManager>
    {
        [SerializeField] private Material highlightMaterial;

        private int cutoutPosID;
        private int cutoutSizeID;

        protected override void OnAwake()
        {
            cutoutPosID = Shader.PropertyToID("_CutoutPosition");
            cutoutSizeID = Shader.PropertyToID("_CutoutSize");
        }

        public void Highlight(Vector2 screenPosition, float size = 0.2f)
        {
            // Convert screen position to normalized UV coordinates (0 to 1)
            Vector2 uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);

            // Apply UV position to shader
            highlightMaterial.SetVector(cutoutPosID, uv);
            highlightMaterial.SetFloat(cutoutSizeID, size);
        }

        public void HideHighlight()
        {
            highlightMaterial.SetFloat(cutoutSizeID, 0f);
        }
    }

}
