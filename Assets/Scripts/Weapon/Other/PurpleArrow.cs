using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies.Common;
using System.Linq;
using Enemies;
using Players;

public class PurpleArrow : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float nodeThreshold = 0.1f;
    [SerializeField] private float pathRecalculationInterval = 0.2f;
    private List<Vector3> path;
    private int currentIndex = 0;
    private Coroutine pathRecalculationCoroutine;
    private Rigidbody2D rb;
    private Enemy targetInstance;

    public void Initialize(Enemy target) {
        targetInstance = target;
        rb = GetComponent<Rigidbody2D>();
        path = Pathfinder.FindPath(transform.position, targetInstance.transform.position);
        currentIndex = 0;
        pathRecalculationCoroutine = StartCoroutine(RecalculatePathRoutine());
    }

    private void Update() {
        if (path == null || currentIndex >= path.Count) return;

        Vector3 target = path[currentIndex];
        Vector3 direction = (target - transform.position).normalized;
        rb.velocity = direction * speed;

        transform.right = (target - transform.position).normalized;

        if (Vector3.Distance(transform.position, target) < nodeThreshold) {
            currentIndex++;
        }
    }

    private IEnumerator RecalculatePathRoutine() {
        while (targetInstance != null) {
            path = Pathfinder.FindPath(transform.position, targetInstance.transform.position);
            currentIndex = 0;
            yield return new WaitForSeconds(pathRecalculationInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }

    private void OnDestroy() {
        if (pathRecalculationCoroutine != null) {
            StopCoroutine(pathRecalculationCoroutine);
        }
    }
}
