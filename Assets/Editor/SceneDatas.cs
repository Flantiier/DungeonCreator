using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using Utils;

namespace Assets.Editor
{
    [CreateAssetMenu(menuName = "Editor/Launcher")]
    [InlineEditor]
    public class SceneDatas : ScriptableObject
    {
        [TitleGroup("Game Launcher"), HideLabel]
        public GameScene gameScene;

        [TitleGroup("Scene Navigator")]
        public List<EditScene> scenes = new List<EditScene>();
    }

    #region GameScene Class
    [System.Serializable]
    public class GameScene
    {
        [HorizontalGroup("Group")]
        [LabelText("Game Scene"), LabelWidth(80)]
        public string scene;
        private bool IsPlaying => EditorApplication.isPlaying;

        [HorizontalGroup("Group")]
        [Button(Icon = SdfIconType.PlayBtn, Name = "", ButtonHeight = 30)]
        [HideIf("IsPlaying"), GUIColor(0.5f, 1.5f, 0.5f)]
        private void Play()
        {
            //Check if scene exists
            if (scene.Length <= 0 || !Utilities.ExistingScene(scene))
            {
                Debug.LogWarning("No scene to launch");
                return;
            }

            //Save the opend scene
            EditorSceneManager.SaveOpenScenes();

            //Load the scene
            if (EditorSceneManager.GetActiveScene().name != scene)
                EditorSceneManager.OpenScene("Assets/_Scenes/" + scene + ".unity");

            //Start PlayMode
            EditorApplication.EnterPlaymode();
        }

        [HorizontalGroup("Group")]
        [Button(Icon = SdfIconType.StopBtn, Name = "", ButtonHeight = 30)]
        [ShowIf("IsPlaying"), GUIColor(1.5f, 0.3f, 0f)]
        private void Stop()
        {
            EditorApplication.ExitPlaymode();
        }

        [HorizontalGroup("Group")]
        [Button(Icon = SdfIconType.PauseBtn, Name = "", ButtonHeight = 30)]
        [GUIColor(1, 1f, 0.2f)]
        private void Pause()
        {
            if (!EditorApplication.isPlaying)
                return;

            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
    }
    #endregion

    #region EditScene Class
    [System.Serializable]
    public class EditScene
    {
        [InlineButton("LoadScene", "Load Scene"), LabelWidth(80)]
        public string scene;

        private void LoadScene()
        {
            if (scene.Length <= 0 || !Utilities.ExistingScene(scene))
            {
                Debug.LogWarning("No scene to launch");
                return;
            }

            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene("Assets/_Scenes/" + scene + ".unity");
        }
    }
    #endregion
}