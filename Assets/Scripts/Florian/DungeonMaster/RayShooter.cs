using UnityEngine;

public class RayShooter : MonoBehaviour
{
    [SerializeField] private float checkOffset;
    [SerializeField] private LayerMask rayMask;
    private Ray ray;
    private RaycastHit hit;
    public Transform _lastObjectHit;

    public int xTiles;
    public int yTiles;

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f);

        Ray Xray = new Ray(_lastObjectHit.position, _lastObjectHit.right);
        Debug.DrawRay(ray.origin, ray.direction * 100f);

        Ray yRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, rayMask))
        {

            if (hit.collider.gameObject == _lastObjectHit)
                return;

            Debug.Log($"Je touche un nouvel objet : {hit.collider.name}");
            _lastObjectHit = hit.collider.transform;
        }
    }

    [ContextMenu("Placement Verification")]
    private void PlacementVerification()
    {
        if(!xTilesCheck() || !yTilesCheck())
        {
            Debug.Log("Ne peux pas poser le piège");
            return;
        }

        Debug.Log("Peux poser le piège");
    }

    private bool xTilesCheck()
    {
        for (int i = 0; i < xTiles; i++)
        {
            Ray xRay = new Ray(_lastObjectHit.position, _lastObjectHit.right);

            //Ne touche pas d'objets
            if (!Physics.Raycast(xRay.origin, xRay.direction, out RaycastHit xHit, _lastObjectHit.localScale.x + checkOffset, rayMask))
            {
                Debug.Log("Pas de tiles disponibles sur X");
                return false;
            }
            else
                continue;
        }

        return true;
    }

    private bool yTilesCheck()
    {
        for (int i = 0; i < xTiles; i++)
        {
            Ray yRay = new Ray(_lastObjectHit.position, _lastObjectHit.right);

            //Ne touche pas d'objets
            if (!Physics.Raycast(yRay.origin, yRay.direction, out RaycastHit yHit, _lastObjectHit.localScale.x + checkOffset, rayMask))
            {
                Debug.Log("Pas de tiles disponibles sur X");
                return false;
            }
            else
                continue;
        }

        return true;
    }
}
