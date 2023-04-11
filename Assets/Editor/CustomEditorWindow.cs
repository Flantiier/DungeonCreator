using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using _ScriptableObjects.Traps;
using _ScriptableObjects.GameManagement;

namespace Assets.Editor
{
    public class CustomEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/CustomEditor")]
        private static void OpenWindow()
        {
            GetWindow<CustomEditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            tree.AddAssetAtPath("Game Launcher", "Assets/Editor/SO/GameLauncherTool.asset", typeof(GameLauncher));
            tree.AddAssetAtPath("Game Management", "Assets/_ScriptablesObjects/GameManagement/GameProperties.asset", typeof(GameProperties));
            tree.AddAssetAtPath("Folder Navigator", "Assets/Editor/SO/Folder Paths.asset");
            tree.AddAllAssetsAtPath("Edit Traps/Traps", "Assets/_ScriptablesObjects/Traps", typeof(TrapSO));
            tree.AddAllAssetsAtPath("Edit Traps/Traps Enemies", "Assets/_ScriptablesObjects/Traps/Traps Enemies", typeof(TrapSO));
            tree.AddAllAssetsAtPath("Edit Traps/Enemies", "Assets/_ScriptablesObjects/Traps/Enemies Properties", typeof(EnemyProperties));
            tree.AddAllAssetsAtPath("Edit Characters", "Assets/_ScriptablesObjects/Characters/Edit Characters");

            return tree;
        }
    }
}
