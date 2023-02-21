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

            tree.AddAssetAtPath("Game Launcher", "Assets/Editor/GameLauncherTool.asset", typeof(GameLauncher));
            tree.AddAssetAtPath("Scene Navigator", "Assets/Editor/NavigatorTool.asset", typeof(SceneNavigator));

            return tree;
        }
    }
}
