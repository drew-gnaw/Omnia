using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HarpoonRope : MonoBehaviour {
    [Header("Rope Connection")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Rope Settings")]
    public int segmentCount = 2;
    public float segmentLength = 0.30f;
    public float ropeWidth = 0.025f;     
    public Color ropeColor = new Color(0.6f, 0.3f, 0.1f);
    public int flex = 1;
    public GameObject ropeSegmentPrefab;

    public LineRenderer lineRenderer;
    private LinkedList<GameObject> segments = new LinkedList<GameObject>();

    public int initialPoolSize = 10;
    private ObjectPool<GameObject> ropeSegmentPool;

    private GameObject CreateRopeSegment(Vector2 position, float length, float width) {
        GameObject segmentObj = ropeSegmentPool.Get();

        segmentObj.transform.position = position;
        segmentObj.transform.rotation = Quaternion.identity;
        segmentObj.transform.SetParent(transform);

        return segmentObj;
    }

    void Awake() {
        if (lineRenderer == null) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = ropeColor;
            lineRenderer.endColor = ropeColor;
            lineRenderer.startWidth = ropeWidth;
            lineRenderer.endWidth = ropeWidth;
            lineRenderer.positionCount = 0;
            lineRenderer.numCapVertices = 10;
            lineRenderer.numCornerVertices = 10;
        }
    }

    public void Initialize(Vector2 direction, Transform start, Transform end, GameObject segmentPrefab) {
        startPoint = start;
        endPoint = end;
        ropeSegmentPrefab = segmentPrefab;

        ropeSegmentPool = new ObjectPool<GameObject>(
            // Create
            () => {
                GameObject segment = Instantiate(ropeSegmentPrefab);
                return segment;
            },
            // OnGet
            (GameObject segment) => {
                segment.SetActive(true);
            },
            // OnRelease
            (GameObject segment) => {
                segment.SetActive(false);
            },
            // OnDestroy
            (GameObject segment) => {
                Destroy(segment);
            },
            true,
            initialPoolSize
        );

        CreateRope(direction);
    }


    private void CreateRope(Vector2 direction) {
        if (startPoint == null || endPoint == null) {
            Debug.LogError("StartPoint and EndPoint must be assigned before creating the rope.");
            return;
        }

        if (ropeSegmentPrefab == null) {
            Debug.LogError("A rope segment prefab must be assigned.");
            return;
        }

        Vector2 segmentDirection = direction;
        Vector2 currentPos = startPoint.position;

        Rigidbody2D previousBody = startPoint.GetComponent<Rigidbody2D>();
        if (previousBody == null) {
            previousBody = startPoint.gameObject.AddComponent<Rigidbody2D>();
            previousBody.isKinematic = true;
        }

        for (int i = 0; i < segmentCount; i++) {
            GameObject segmentObj = CreateRopeSegment(currentPos, segmentLength, ropeWidth);

            HingeJoint2D hinge = segmentObj.GetComponent<HingeJoint2D>();

            hinge.autoConfigureConnectedAnchor = false;
            hinge.connectedBody = previousBody;
            hinge.connectedAnchor = new Vector2(segmentLength / 2, 0);

            segments.AddLast(segmentObj);

            currentPos += segmentDirection * segmentLength;
            previousBody = hinge.GetComponent<Rigidbody2D>();
        }

        Rigidbody2D endRb = endPoint.GetComponent<Rigidbody2D>();
        if (endRb == null) {
            endRb = endPoint.gameObject.AddComponent<Rigidbody2D>();
            endRb.isKinematic = true;
        }

        HingeJoint2D finalHinge = segments.Last.Value.AddComponent<HingeJoint2D>();
        finalHinge.autoConfigureConnectedAnchor = false;
        finalHinge.anchor = new Vector2(segmentLength / 2, 0);
        finalHinge.connectedBody = endRb;
        finalHinge.connectedAnchor = Vector2.zero;
    }

    public void ResizeRope(float newDistance) {
        int newSegmentCount = Mathf.FloorToInt(newDistance / segmentLength) + flex;

        // add
        if (newSegmentCount > segments.Count) {
            Rigidbody2D previousBody = segments.First.Value.GetComponent<Rigidbody2D>();
            Vector2 ropeDirection = (endPoint.position - startPoint.position).normalized;
            Vector2 currentPos = startPoint.position;

            for (int i = segments.Count; i < newSegmentCount; i++) {
                currentPos += ropeDirection * segmentLength;
                GameObject segmentObj = CreateRopeSegment(currentPos, segmentLength, ropeWidth);

                HingeJoint2D hinge = previousBody.GetComponent<HingeJoint2D>();
                hinge.autoConfigureConnectedAnchor = false;
                hinge.connectedBody = segmentObj.GetComponent<Rigidbody2D>();
                hinge.connectedAnchor = new Vector2(segmentLength / 2, 0);

                segments.AddFirst(segmentObj);

                previousBody = segmentObj.GetComponent<Rigidbody2D>();
            }

            HingeJoint2D final_hinge = previousBody.GetComponent<HingeJoint2D>();
            final_hinge.autoConfigureConnectedAnchor = false;
            final_hinge.connectedBody = startPoint.GetComponent<Rigidbody2D>();
            final_hinge.connectedAnchor = new Vector2(segmentLength / 2, 0);
        }

        // remove
        else if (newSegmentCount < segments.Count) {
            int segmentsToRemove = Mathf.Min(segments.Count - newSegmentCount, segments.Count - 1);
            for (int i = 0; i < segmentsToRemove; i++) {
                GameObject lastSegment = segments.First.Value;
                segments.RemoveFirst();
                ropeSegmentPool.Release(lastSegment);
            }

            HingeJoint2D final_hinge = segments.First.Value.GetComponent<HingeJoint2D>();
            final_hinge.autoConfigureConnectedAnchor = false;
            final_hinge.connectedBody = startPoint.GetComponent<Rigidbody2D>();
            final_hinge.connectedAnchor = new Vector2(segmentLength / 2, 0);
        }
    }

    void Update() {
        ResizeRope(Vector2.Distance(startPoint.position, endPoint.position));

        if (lineRenderer != null) {
            int pointsCount = segments.Count + 2;
            Vector3[] positions = new Vector3[pointsCount];

            positions[0] = startPoint.position;

            int index = 1;
            for (LinkedListNode<GameObject> node = segments.First; node != null; node = node.Next) {
                positions[index] = node.Value.transform.position;
                index++;
            }

            positions[pointsCount - 1] = endPoint.position; 

            lineRenderer.positionCount = pointsCount;
            lineRenderer.SetPositions(positions);
        }

    }

    public void DestroyRope() {
        foreach (GameObject rb in segments) {
            if (rb != null) {
                ropeSegmentPool.Release(rb.gameObject);
            }
        }
        segments.Clear();
        Destroy(gameObject);
    }
}
