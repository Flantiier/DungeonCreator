using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TestSphereCast : MonoBehaviour
{
    #region Variables
    [TitleGroup("FlameThrower properties")]
    [SerializeField] private VisualEffect fx;
    [SerializeField] private Transform rayStart;
    [SerializeField] private float maxDistance = 2f;
    [SerializeField] private float detectRadius = 1f;
    [SerializeField] private float fixOffset = 0.1f;
    [SerializeField] private LayerMask rayMask;

    private float _rayLength;
    #endregion

    #region Builts_In
    private void Start()
    {
        StartCoroutine("test");
    }

    private void Update()
    {
        ShootRayTowards();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Shoot a ray and detect is smth is colliding towards the flame point
    /// </summary>
    private void ShootRayTowards()
    {
        Vector3 start = rayStart.position - rayStart.forward * (detectRadius);
        float finalDistance = maxDistance + detectRadius;

        Ray ray = new Ray(start, rayStart.forward);
        Debug.DrawRay(rayStart.position, ray.direction * finalDistance, Color.cyan);

        if (Physics.SphereCast(ray, detectRadius, out RaycastHit hit, finalDistance, rayMask, QueryTriggerInteraction.Collide))
        {
            _rayLength = Vector3.Distance(rayStart.position, hit.point);
            Debug.DrawRay(rayStart.position, ray.direction * _rayLength, Color.red);
        }
    }

    private IEnumerator test()
    {
        fx.Play();

        yield return new WaitForSecondsRealtime(3f);

        fx.Stop();

        yield return new WaitForSecondsRealtime(3f);

        StartCoroutine("test");
    }
    #endregion
}
