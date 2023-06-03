using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

namespace _Scripts.Editor
{
    public class SubSceneColliderGenerator : OdinEditorWindow
    {
        [MenuItem("Tools/Collider Generator")]
        public static void OpenWindow()
        {
            GetWindow<SubSceneColliderGenerator>("Collider Generator").Show();
        }

        [Button("Get Colliders")]
        private void GetColliders()
        {
            Transform parent = new GameObject().transform;
            parent.name = "Subscene_colliders";

            //Get all colliders
            Collider[] colliders = GetCollidersArray();

            //Create an instance of each
            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject obj = new GameObject();
                obj.name = $"{colliders[i].name}_Col{i + 1}";
                obj.transform.position = colliders[i].transform.position;
                obj.transform.rotation = colliders[i].transform.rotation;
                obj.transform.localScale = colliders[i].transform.localScale;
                obj.layer = colliders[i].gameObject.layer;
                obj.transform.parent = parent;

                //Get type of collider at i
                System.Type type = colliders[i].GetType();

                //Switch its type and add the component of type <type>
                switch (type.ToString())
                {
                    case "UnityEngine.BoxCollider":
                        BoxCollider box = obj.AddComponent<BoxCollider>();
                        SetBoxCollider(ref box, colliders[i] as BoxCollider);
                        break;
                    case "UnityEngine.SphereCollider":
                        SphereCollider sphere = obj.AddComponent<SphereCollider>();
                        SetSphereCollider(ref sphere, colliders[i] as SphereCollider);
                        break;
                    case "UnityEngine.MeshCollider":
                        MeshCollider mesh = obj.AddComponent<MeshCollider>();
                        SetMeshCollider(ref mesh, colliders[i] as MeshCollider);
                        break;
                }
            }
        }

        private Collider[] GetCollidersArray()
        {
            Collider[] temp = FindObjectsOfType<Collider>();
            List<Collider> colliders = new List<Collider>();

            foreach (Collider col in temp)
            {
                if (col.transform.parent != null && col.transform.root.name == "Tiling")
                    continue;
                else
                    colliders.Add(col);
            }

            return colliders.ToArray();
        }

        private void SetBoxCollider(ref BoxCollider instance, BoxCollider collider)
        {
            instance.size = collider.size;
            instance.center = collider.center;
        }

        private void SetSphereCollider(ref SphereCollider instance, SphereCollider collider)
        {
            instance.radius = collider.radius;
            instance.center = collider.center;
        }

        private void SetMeshCollider(ref MeshCollider instance, MeshCollider collider)
        {
            instance.sharedMesh = collider.sharedMesh;
        }
    }
}
