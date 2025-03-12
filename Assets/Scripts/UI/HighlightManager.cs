using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace UI
{
    public class HighlightManager : PersistentSingleton<HighlightManager>
    {
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private GameObject reticlePrefab;
        [SerializeField] private float rotationSpeed = 30f;

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

        private void Update()
        {
            RotateReticles();
        }

        public void HighlightUI(Vector2 screenPosition, float size = 0.2f)
        {
            Vector2 uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
            highlightMaterial.SetVector(cutoutPosID, uv);
            highlightMaterial.SetFloat(cutoutSizeID, size);
        }

        public void HideHighlight()
        {
            highlightMaterial.SetFloat(cutoutSizeID, 0f);
        }

        public void HighlightGameObject(GameObject target)
        {
            if (!highlightedObjects.Contains(target))
            {
                highlightedObjects.Add(target);

                GameObject reticleObj = Instantiate(reticlePrefab, target.transform);
                Transform reticleTransform = reticleObj.transform;

                reticles.Add(reticleTransform);
                reticleTransform.localPosition = Vector3.zero;

                ReticleCleanup cleanup = reticleObj.AddComponent<ReticleCleanup>();
                cleanup.Init(target, this);
            }
        }

        public void UnhighlightGameObject(GameObject target)
        {
            int index = highlightedObjects.IndexOf(target);
            if (index >= 0)
            {
                highlightedObjects.RemoveAt(index);

                if (index < reticles.Count)
                {
                    if (reticles[index] != null)
                    {
                        Destroy(reticles[index].gameObject);
                    }
                    reticles.RemoveAt(index);
                }
            }
        }

        private void RotateReticles()
        {
            for (int i = reticles.Count - 1; i >= 0; i--)
            {
                if (reticles[i] == null)
                {
                    reticles.RemoveAt(i);
                }
                else
                {
                    reticles[i].Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                }
            }
        }

        public void ClearAllHighlights()
        {
            foreach (var reticle in reticles)
            {
                if (reticle != null) Destroy(reticle.gameObject);
            }

            highlightedObjects.Clear();
            reticles.Clear();

            HideHighlight();
        }
    }

    public class ReticleCleanup : MonoBehaviour
    {
        private GameObject target;
        private HighlightManager manager;

        public void Init(GameObject target, HighlightManager manager)
        {
            this.target = target;
            this.manager = manager;
        }

        private void OnDestroy()
        {
            if (target != null)
            {
                manager?.UnhighlightGameObject(target);
            }
        }
    }
}
