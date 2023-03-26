using System;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private void Update()
    {
        FieldOfViewCheck();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length <= 0)
            return;

        foreach (Collider item in rangeChecks)
        {
            Transform target = item.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                Ray ray = new Ray(transform.position, directionToTarget);
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(ray, distanceToTarget, obstructionMask))
                    Debug.DrawRay(ray.origin, ray.direction * distanceToTarget, Color.red);
                else
                    Debug.DrawRay(ray.origin, ray.direction * distanceToTarget, Color.green);
            }
        }
    }

    /// <summary>
    /// Return the closest target in the view
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length <= 0)
            return null;

        Transform currentTarget = rangeChecks[0].transform;

        foreach (Collider item in rangeChecks)
        {
            Transform target = item.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                Ray ray = new Ray(transform.position, directionToTarget);
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                float distanceToCurrent = Vector3.Distance(transform.position, currentTarget.position);

                Debug.Log($"Current : {currentTarget} ({distanceToCurrent}) / Target : {target} ({distanceToTarget})");

                if (Physics.Raycast(ray, distanceToTarget, obstructionMask))
                    continue;

                if (distanceToTarget >= distanceToCurrent)
                    continue;
                else
                    currentTarget = target;

            }
        }

        return currentTarget;
    }
}
