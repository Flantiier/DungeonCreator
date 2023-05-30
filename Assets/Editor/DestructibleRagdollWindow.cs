using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using _Scripts.GameplayFeatures;

public class DestructibleRagdollWindow : OdinEditorWindow
{
    [SerializeField] private Transform ragdollParent;
    [SerializeField] private bool cleanParent;

    [MenuItem("Tools/Destructible Ragdoll")]
    private static void OpenWindow()
    {
        GetWindow<DestructibleRagdollWindow>().Show();
    }

    [Button("Create New Ragdoll", ButtonSizes.Medium)]
    private void CreateRagdollParts()
    {
        Transform parent = RagdollParent();

        if (parent == ragdollParent && cleanParent)
            DestroyAllChildren();

        Object[] meshs = Selection.GetFiltered(typeof(SkinnedMeshRenderer), SelectionMode.DeepAssets);

        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            if (mesh.TryGetComponent(out DestructibleRagdollPart part))
                continue;

            GameObject instance = NewRagdollPart(mesh);
            instance.transform.SetParent(parent);
        }
    }

    /// <summary>
    /// Create an instance of the given mesh
    /// </summary>
    private GameObject NewRagdollPart(SkinnedMeshRenderer mesh)
    {
        //Create an instance of the object
        GameObject instance = new GameObject();
        instance.name = mesh.name + "_ragdoll";
        instance.transform.position = mesh.transform.position;
        instance.transform.rotation = mesh.transform.rotation;
        instance.transform.localScale = mesh.transform.localScale;

        //Add mesh renderer
        MeshFilter filter = instance.AddComponent<MeshFilter>();
        filter.mesh = mesh.sharedMesh;
        MeshRenderer renderer = instance.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = mesh.sharedMaterial;

        //Add rigidbody
        Rigidbody rb = instance.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        //Add collider
        MeshCollider collider = instance.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh.sharedMesh;
        collider.convex = true;

        //Add script
        DestructibleRagdollPart ragdoll = instance.AddComponent<DestructibleRagdollPart>();
        ragdoll.Bone = mesh;
        ragdoll.rb = rb;
        ragdoll.meshCollider = collider;
        ragdoll.filter = filter;

        return instance;
    }

    /// <summary>
    /// Return the selected parent or a new instance
    /// </summary>
    private Transform RagdollParent()
    {
        if (ragdollParent)
            return ragdollParent;

        GameObject instance = new GameObject();
        instance.name = "Ragdoll Parent";
        return instance.transform;
    }

    /// <summary>
    /// Destroy each child of the gievn parent
    /// </summary>
    [Button("Destroy All Children", ButtonSizes.Medium)]
    private void DestroyAllChildren()
    {
        for (int i = ragdollParent.childCount; i > 0; --i)
            DestroyImmediate(ragdollParent.GetChild(0).gameObject);
    }
}
