using UnityEngine;

public class TestSphereCast : MonoBehaviour
{
    public float offset;
    public float radius;
    public float distance;
    public LayerMask mask;

    private void Update()
    {
        ShootRay();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + transform.forward * offset + radius * transform.forward, radius);
        Gizmos.DrawSphere(transform.position + transform.forward * (offset + distance - radius) + radius * transform.forward, radius);
        Gizmos.DrawRay(transform.position + transform.forward * offset, transform.forward * (distance + 2 * (radius / 2)));
    }

    private void ShootRay()
    {
        if (Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, Vector3.Project(hit.point - transform.position, transform.forward));
        }
    }
}
