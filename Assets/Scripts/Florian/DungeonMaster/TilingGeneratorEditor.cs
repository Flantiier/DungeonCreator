using UnityEngine;
using UnityEditor;

namespace _Scripts.TrapSystem
{
    [CustomEditor(typeof(TilingGenerator))]
    public class TilingGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TilingGenerator generator = (TilingGenerator)target;

            GUILayout.Space(20);
            if (GUILayout.Button("CreateTiling", GUILayout.Height(40)))
            {
                generator.CreateTiling();
            }
        }
    }
}
