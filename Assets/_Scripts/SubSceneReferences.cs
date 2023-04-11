using Unity.Scenes;
using UnityEngine;

public class SubSceneReferences : MonoBehaviour
{
    public static SubSceneReferences Instance { get; private set; }

    public SubScene map1;

    private void Awake()
    {
        Instance = this;
    }
}