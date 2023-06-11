using UnityEngine;
using Unity.Scenes;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _Scripts.Managers;

public class SubsceneTrigger : MonoBehaviour
{
    [SerializeField] private bool loadObjects = false;
    [SerializeField] private bool hideObjects = false;
    [ShowIf("loadObjects")]
    [SerializeField] private SubScene[] sceneToLoad;
    [ShowIf("hideObjects")]
    [SerializeField] private SubScene[] sceneToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character) || !character.ViewIsMine())
            return;

        if (loadObjects)
        {
            foreach (SubScene scene in sceneToLoad)
                SubsceneLoader.Instance.LoadSubscene(scene);
        }

        if (hideObjects)
        {
            foreach (SubScene scene in sceneToHide)
                SubsceneLoader.Instance.UnloadSubscene(scene);
        }
    }
}
