using System.Collections;
using UnityEngine;
using Unity.Scenes;
using Unity.Entities;
using Sirenix.OdinInspector;
using _Scripts.Managers;

public class SubsceneTrigger : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool loadObjects = false;
    [SerializeField] private bool hideObjects = false;
    [ShowIf("loadObjects")]
    [SerializeField] private SubScene[] sceneToLoad;
    [ShowIf("hideObjects")]
    [SerializeField] private SubScene[] sceneToHide;

    [Header("Detection")]
    [SerializeField] private Vector3 size;
    [SerializeField] private LayerMask mask;

    private bool _canLoad;
    private bool _canHide;
    private bool _enterWait;
    #endregion

    #region Builts_In
    private void Update()
    {
        DetectPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, size);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Detect the player and load scene if possible
    /// </summary>
    private void DetectPlayer()
    {
        //Load conditions
        _canLoad = CheckLoading(sceneToLoad, true);
        _canHide = CheckLoading(sceneToHide, false);

        Collider[] colliders = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, mask);
        if (colliders.Length <= 0)
            return;

        if (!HasLocalPlayer(colliders) || _enterWait)
            return;

        if (loadObjects && _canLoad)
            foreach (SubScene scene in sceneToLoad)
                SubsceneLoader.Instance.LoadSubscene(scene);

        if (hideObjects && _canHide)
            foreach (SubScene scene in sceneToHide)
                SubsceneLoader.Instance.UnloadSubscene(scene);

        _enterWait = true;
        StartCoroutine(EnterRoutine());
    }

    /// <summary>
    /// Check if the differents scene can be loaded
    /// </summary>
    private bool CheckLoading(SubScene[] list, bool value)
    {
        foreach (SubScene scene in list)
        {
            if (!scene)
                continue;

            Entity entity = SubsceneLoader.Instance.SceneSystem.GetSceneEntity(scene.SceneGUID);
            if (SubsceneLoader.Instance.SceneSystem.IsSceneLoaded(entity) == value)
                continue;
            else
                return true;
        }

        return false;
    }

    /// <summary>
    /// Indicates if the local player is detected in the trigger
    /// </summary>
    private bool HasLocalPlayer(Collider[] colliders)
    {
        foreach (Collider col in colliders)
        {
            if (!col || !col.TryGetComponent(out _Scripts.Characters.Character character))
                continue;

            if (!character.ViewIsMine())
                continue;
            else
                return true;
        }

        return false;
    }

    private IEnumerator EnterRoutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        _enterWait = false;
    }
    #endregion
}