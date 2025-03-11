using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UI
{
    public class HighlightManager : PersistentSingleton<HighlightManager>
    {
        [SerializeField] private Material highlightMaterial; // Material for UI highlighting
        [SerializeField] private GameObject reticlePrefab; // Prefab for the reticle sprite for GameObjects
        [SerializeField] private float rotationSpeed = 30f; // Speed of rotation in degrees per second

        private int cutoutPosID;
        private int cutoutSizeID;

        private List<GameObject> highlightedObjects = new List<GameObject>();
        private List<Transform> reticles = new List<Transform>();

        protected override void OnAwake()
        {
            cutoutPosID = Shader.PropertyToID("_CutoutPosition");
            cutoutSizeID = Shader.PropertyToID("_CutoutSize");

            HideHighlight();
        }

        // Update is called every frame for rotation of reticles
        private void Update()
        {
            RotateReticles();
        }

        #region UI Highlighting (Cutout)

        // Highlight a UI element by setting its position and size in screen space (cutout)
        public void HighlightUI(Vector2 screenPosition, float size = 0.2f)
        {
            // Convert screen position to normalized UV coordinates (0 to 1)
            Vector2 uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);

            // Apply UV position and size to shader
            highlightMaterial.SetVector(cutoutPosID, uv);
            highlightMaterial.SetFloat(cutoutSizeID, size);
        }

        // Hide the UI highlighting (set the size to 0)
        public void HideHighlight()
        {
            highlightMaterial.SetFloat(cutoutSizeID, 0f);
        }

        #endregion

        #region GameObject Highlighting (Reticle)

        // Highlights a GameObject by displaying a reticle at its position
        public void HighlightGameObject(GameObject target)
        {
            if (!highlightedObjects.Contains(target))
            {
                highlightedObjects.Add(target);

                // Create a new reticle for the highlighted GameObject
                GameObject reticleObj = Instantiate(reticlePrefab, target.transform.position, Quaternion.identity);
                Transform reticleTransform = reticleObj.GetComponent<Transform>();

                // Add the reticle to the list of reticles for rotation
                reticles.Add(reticleTransform);

                // Set its layer order, z-index, or sorting order if needed
                reticleTransform.SetAsLastSibling();
            }
        }

        // Unhighlights a GameObject by removing its reticle
        public void UnhighlightGameObject(GameObject target)
        {
            if (highlightedObjects.Contains(target))
            {
                highlightedObjects.Remove(target);

                // Find and destroy the reticle associated with this GameObject
                int index = highlightedObjects.IndexOf(target);
                if (index >= 0 && index < reticles.Count)
                {
                    Destroy(reticles[index].gameObject); // Destroy reticle GameObject
                    reticles.RemoveAt(index); // Remove from the list of reticles
                }
            }
        }

        // Rotate all active reticles slowly
        private void RotateReticles()
        {
            foreach (Transform reticle in reticles)
            {
                reticle.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }

        #endregion

        // Optionally, clear all highlights (UI and GameObjects)
        public void ClearAllHighlights()
        {
            // Clear GameObject highlights (reticles)
            foreach (var target in highlightedObjects)
            {
                UnhighlightGameObject(target);
            }

            // Clear UI highlight
            HideHighlight();

            highlightedObjects.Clear();
            reticles.Clear();
        }
    }
}
