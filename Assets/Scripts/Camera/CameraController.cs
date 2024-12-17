using System.Collections.Generic;
using Cinemachine;
using Players;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Player player;
    [SerializeField] private float aimOffsetDistance = 3f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float lockPointRadius = 5f;

    private CinemachineFramingTransposer framingTransposer;
    private Vector3 defaultOffset;
    private Vector3 currentOffset;
    private List<Transform> lockPoints;
    private Transform closestLockPoint;

    void Start()
    {
        if (virtualCamera != null)
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (framingTransposer != null)
            defaultOffset = framingTransposer.m_TrackedObjectOffset;

        lockPoints = new List<Transform>();

        Debug.Log(transform.Find("Lockpoints").transform);

        foreach (Transform child in transform.Find("Lockpoints"))
        {
            lockPoints.Add(child);
        }
    }

    void LateUpdate()
    {
        if (player == null || framingTransposer == null) return;

        closestLockPoint = GetClosestLockPoint();
        if (closestLockPoint != null)
        {
            Vector3 lockPosition = closestLockPoint.position;
            lockPosition.z = player.transform.position.z;
            virtualCamera.transform.position = Vector3.Lerp(
                virtualCamera.transform.position,
                lockPosition,
                smoothSpeed * Time.deltaTime
            );
            return;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 desiredOffset = (Vector3)player.facing.normalized * aimOffsetDistance;
            currentOffset = Vector3.Lerp(currentOffset, desiredOffset, smoothSpeed * Time.deltaTime);
            framingTransposer.m_TrackedObjectOffset = new Vector3(currentOffset.x, currentOffset.y, framingTransposer.m_TrackedObjectOffset.z);
        }
        else
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                defaultOffset,
                smoothSpeed * Time.deltaTime
            );
        }
    }

    private Transform GetClosestLockPoint()
    {
        Transform closest = null;
        float closestDistance = lockPointRadius;

        foreach (Transform lockPoint in lockPoints)
        {
            float distance = Vector3.Distance(player.transform.position, lockPoint.position);
            if (distance < closestDistance)
            {
                closest = lockPoint;
                closestDistance = distance;
            }
        }

        return closest;
    }
}
