using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;

namespace Assets.Editor
{
    [CreateAssetMenu(menuName = "Editor/Launcher")]
    [InlineEditor]
    public class SceneDatas : ScriptableObject
    {
        public List<EditScene> scenes = new List<EditScene>();
    }

    [System.Serializable]
    public class EditScene
    {
        public string scene;

        [Button("Load scene", ButtonSizes.Medium), GUIColor(0.3f, 1f, 0.5f)]
        private void LaunchScene()
        {
            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene("Assets/_Scenes/" + scene + ".unity");
        }
    }
}