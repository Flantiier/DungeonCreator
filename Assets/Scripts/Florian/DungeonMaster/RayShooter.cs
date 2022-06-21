using UnityEngine;
using System.Collections.Generic;

public class RayShooter : MonoBehaviour
{
    [SerializeField] private float checkOffset;
    [SerializeField] private LayerMask rayMask;
    private Ray _ray;
    public Transform _lastObjectHit;

    public int xTiles;
    public int yTiles;

    private List<Tile> tiles = new List<Tile>();

    private void Update()
    {
        _ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.blue);

        if (Physics.Raycast(_ray.origin, _ray.direction, out RaycastHit hit, Mathf.Infinity, rayMask))
        {
            if (_lastObjectHit == null)
                _lastObjectHit = hit.transform;
            else if (_lastObjectHit.gameObject == hit.collider.gameObject)
                return;

            Debug.Log($"Je touche un nouvel objet : {hit.collider.name}");
            _lastObjectHit = hit.collider.transform;

            Ray xRay = new Ray(_lastObjectHit.position, _lastObjectHit.right);
            Ray yRay = new Ray(_lastObjectHit.position, _lastObjectHit.forward);

            //SetTiles Materials
            ResetTiles();
            FillTilesList(xRay, yRay);
            SetTiles();
        }
    }

    [ContextMenu("Placement Verification")]
    private void PlacementVerification()
    {
        Ray xRay = new Ray(_lastObjectHit.position, _lastObjectHit.right);
        Ray yRay = new Ray(_lastObjectHit.position, _lastObjectHit.forward);

        if (!TilesCheck(xRay, xTiles) || !TilesCheck(yRay, yTiles))
        {
            Debug.Log("Ne peux pas poser le piège");
            return;
        }

        Debug.Log("Peux poser le piège");
    }

    private bool TilesCheck(Ray ray, int tilesNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            //Ne touche pas d'objets
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, _lastObjectHit.localScale.x + checkOffset, rayMask))
            {
                Debug.Log("Pas assez de tiles disponibles");
                return false;
            }
            else
            {
                ray.origin = hit.transform.position;
                continue;
            }
        }

        return true;
    }

    private void ResetTiles()
    {
        foreach (Tile tile in tiles)
        {
            tile.OnDeselected();
        }

        tiles.Clear();
    }

    private void FillTilesList(Ray xRay, Ray yRay)
    {
        if(_lastObjectHit.TryGetComponent(out Tile tile))
            tiles.Add(tile);

        FillOneAxis(xRay, xTiles);
        FillOneAxis(yRay, yTiles);
    }

    private void FillOneAxis(Ray ray, int tilesNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            //Ne touche pas d'objets
            if (Physics.Raycast(_ray.origin, _ray.direction, out RaycastHit hit, _lastObjectHit.localScale.x + checkOffset, rayMask))
            {
                tiles.Add(hit.transform.GetComponent<Tile>());
                ray.origin = hit.transform.position;
            }
        }
    }

    private void SetTiles()
    {
        foreach (Tile tile in tiles)
        {
            tile.OnSelected();
        }
    }
}
