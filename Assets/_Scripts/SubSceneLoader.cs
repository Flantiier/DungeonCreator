using UnityEngine;
using Unity.Entities;
using Unity.Scenes;

public class SubSceneLoader : ComponentSystem
{
    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Load
            LoadScene(SubSceneReferences.Instance.map1);
        }
    }

    private void LoadScene(SubScene subScene)
    {
        Debug.Log("Start loading");

        sceneSystem.LoadSceneAsync(subScene.SceneGUID);
    }

    private void UnloadScene(SubScene subScene)
    {
        sceneSystem.UnloadScene(subScene.SceneGUID);
    }
}
