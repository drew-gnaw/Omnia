using Players;
using Unity.VisualScripting;
using UnityEngine;
using MathUtils = Utils.MathUtils;

public class Tracer : MonoBehaviour {
    [SerializeField] public Material tracerMaterial;
    [SerializeField] public float tracerEndDuration;
    [SerializeField] public float tracerStartWidth;
    [SerializeField] public float tracerEndWidth;
    [SerializeField] public float minTracerStartOffset;
    [SerializeField] public float maxTracerStartOffset;
    [SerializeField] public float tracerSpeed;


    bool initialized = false;
    private LineRenderer lineRenderer;
    private float trailEndTime;
    private Vector2 endPosition;

    public void Initialize(Vector2 origin, Vector2 direction, float range, LayerMask hittableLayerMask) {
        trailEndTime = tracerEndDuration;

        Vector2 startPosition = origin + direction * Random.Range(minTracerStartOffset, maxTracerStartOffset);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, hittableLayerMask);
        endPosition = hit.collider != null ? hit.point : origin + (direction * range);

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = tracerMaterial;
        lineRenderer.startWidth = tracerStartWidth;
        lineRenderer.endWidth = tracerEndWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);

        initialized = true;
    }

    public void Update() {
        if (!initialized) return;

        trailEndTime = Mathf.Max(trailEndTime - Time.deltaTime, 0);


        if ((Vector2) lineRenderer.GetPosition(0) == endPosition) {
            Destroy(gameObject);
        }
        if (trailEndTime <= 0) {
            lineRenderer
                .SetPosition(
                    0,
                    Vector2.MoveTowards(
                        (Vector2)lineRenderer.GetPosition(0), endPosition, tracerSpeed * Time.deltaTime
                    )
                );
        }
        lineRenderer
            .SetPosition(
                1, 
                Vector2.MoveTowards(
                    (Vector2) lineRenderer.GetPosition(1), endPosition, tracerSpeed * Time.deltaTime
                )
            );
    }
}
