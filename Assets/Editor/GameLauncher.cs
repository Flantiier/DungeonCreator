using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using Utils;

namespace Assets.Editor
{
    [CreateAssetMenu(fileName = "GameLauncherTool", menuName = "Editor/GameLauncher")]
    public class GameLauncher : ScriptableObject
    {
        [BoxGroup("Box", LabelText = "Game Launcher", CenterLabel = true)]
        [HideLabel]
        public GameScene gameScene;

        [BoxGroup("Box/Characters", LabelText = "Characters", CenterLabel = true)]
        public CharactersList characters;

        [TitleGroup("Scene Navigator", Alignment = TitleAlignments.Centered)]
        public EditScene[] scenes;
    }

    #region GameScene Class
    [System.Serializable]
    public class GameScene
    {
        private bool IsPlaying => EditorApplication.isPlaying;

        [HorizontalGroup("Header")]
        [LabelText("Game Scene"), Space, LabelWidth(80)]
        public string scene;

        #region Play/Stop/Pause
        [HorizontalGroup("Header")]
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

        [HorizontalGroup("Header")]
        [Button(Icon = SdfIconType.StopBtn, Name = "", ButtonHeight = 30)]
        [ShowIf("IsPlaying"), GUIColor(1.5f, 0.3f, 0f)]
        private void Stop()
        {
            EditorApplication.ExitPlaymode();
        }

        [HorizontalGroup("Header")]
        [Button(Icon = SdfIconType.PauseBtn, Name = "", ButtonHeight = 30)]
        [GUIColor(1, 1f, 0.2f)]
        private void Pause()
        {
            if (!EditorApplication.isPlaying)
                return;

            EditorApplication.isPaused = !EditorApplication.isPaused;
        }
        #endregion
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