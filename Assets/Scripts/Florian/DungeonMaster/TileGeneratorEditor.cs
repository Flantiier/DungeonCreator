using UnityEngine;
using UnityEditor;

namespace Tiling
{
    [CustomEditor(typeof(TilingGenerator))]
    public class TilingGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(20);
            TilingGenerator generator = (TilingGenerator)target;

            if(GUILayout.Button("Create Tiles", GUILayout.Height(40)))
            {
                generator.CreateTiling();
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Reset Tiles",GUILayout.Height(40)))
            {
                generator.ResetTiling();
            }
        }
    }
}
