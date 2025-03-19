using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    [SerializeField] private LayerMask hittableLayerMask;
    // Pair of distance and time
    private List<Tuple<float, float>> thresholds = new List<Tuple<float, float>>
    {
        new Tuple<float, float>(1f, 0.05f),
        new Tuple<float, float>(3.3f, 0.1f),
        new Tuple<float, float>(8.5f, 0.15f),
    };

    // This is a really bad work around, this is coupling the distance and the time of the
    // tracer animation at which it will be that distance long
    // really really really consider animating in-game rather than using animated assets
    // raycasts will help
    public void Initialize(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, hittableLayerMask);
        float distance = hit ? hit.distance : thresholds.First().Item1;

        var killTime = thresholds.LastOrDefault(t => distance >= t.Item1)?.Item2 ?? thresholds.First().Item2;

        transform.position = origin;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Destroy(gameObject, killTime);
    }
}
