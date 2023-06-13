using UnityEngine;
using Unity.Scenes;
using _Scripts.Managers;

public class TeleportPointDisplay : MonoBehaviour
{
    [SerializeField] private SubScene[] subscenes;
    [Header("Visual")]
    [SerializeField] private Color color = new Color(0, 1, 1, 1);
    [SerializeField] private Vector3 size = new Vector3(0.75f, 2, 0.75f);

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + new Vector3(0, size.y / 2, 0), size);
    }

    public void LoadMap()
    {
        SubsceneLoader.Instance.UnloadAll();
        SubsceneLoader.Instance.LoadFromList(subscenes);
    }
}
