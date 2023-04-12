using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class ColliderCheck : OdinEditorWindow
{
    [MenuItem("Tools/CheckMap")]
    public static void OpenWindow()
    {
        GetWindow<ColliderCheck>("Check maps").Show();
    }

    [Button("Check Colliders", ButtonSizes.Medium)]
    private void CheckCollider()
    {
        GameObject obj = Selection.activeGameObject; 
        Transform transform = obj.transform;

        if (transform.childCount <= 0)
            return;

        foreach (Transform child in transform)
        {
            if (!child)
                continue;
            if(child.gameObject.TryGetComponent(out Collider collider))
                Debug.Log("Collider found");
            else
                Debug.Log($"No collider : {child.gameObject}");
        }
    }

    [Button("Check Layers", ButtonSizes.Medium)]
    private void CheckLayers()
    {
        GameObject obj = Selection.activeGameObject;
        Transform transform = obj.transform;

        if (transform.childCount <= 0)
            return;

        foreach (Transform child in transform)
        {
            if (!child)
                continue;

            Debug.Log(LayerMask.LayerToName(child.gameObject.layer));
        }
    }
}
