using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace Assets.Editor
{
    public class EditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/EditorWindow")]
        private static void OpenWindow()
        {
            GetWindow<EditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            tree.AddAllAssetsAtPath("Scene Loader", "Assets/Editor", typeof(SceneDatas));

            return tree;
        }
    }
}
