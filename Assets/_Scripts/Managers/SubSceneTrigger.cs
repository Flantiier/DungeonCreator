using Unity.Scenes;
using UnityEngine;
using _Scripts.Managers;
using _Scripts.Characters;

public class SubSceneTrigger : MonoBehaviour
{
    [SerializeField] private SubScene[] scenesToLoad;
    [SerializeField] private SubScene[] scenesToUnload;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out TPS_Character character))
            return;

        if (!character.ViewIsMine())
            return;

        LoadScenes();
    }

    [ContextMenu("Load")]
    private void LoadScenes()
    {
        if(scenesToUnload.Length > 0)
            foreach (SubScene scene in scenesToUnload)
                SubScenesManager.Instance.UnloadSubScene(scene);

        if (scenesToLoad.Length > 0)
            foreach (SubScene scene in scenesToLoad)
                SubScenesManager.Instance.LoadSubScene(scene);
    }
}
