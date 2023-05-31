using _Scripts.Characters;
using _Scripts.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapTriggerLoader : MonoBehaviour
{
    [SerializeField] private bool loadObjects = false;
    [SerializeField] private bool hideObjects = false;
    [ShowIf("loadObjects")]
    [SerializeField] private GameObject[] objToLoad;
    [ShowIf("hideObjects")]
    [SerializeField] private GameObject[] objToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character) || !character.ViewIsMine())
            return;

        if (loadObjects)
        {
            foreach (GameObject obj in objToLoad)
                MapLoader.Instance.EnableMapPart(obj);
        }

        if (hideObjects)
        {
            foreach (GameObject obj in objToHide)
                MapLoader.Instance.DisableMapPart(obj);
        }
    }
}
