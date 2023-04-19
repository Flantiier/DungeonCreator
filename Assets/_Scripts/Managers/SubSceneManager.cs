using Unity.Scenes;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SubSceneManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private SubScene[] subScenes;
        public SceneSystem sceneSystem { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Loading a given subScene
        /// </summary>
        /// <param name="subScene"> SubScene to load </param>
        public void LoadSubScene(SubScene subScene)
        {
            if (subScene.IsLoaded)
                return;

            sceneSystem.LoadSceneAsync(subScene.SceneGUID);
        }

        /// <summary>
        /// Loading a subScene by giving its index
        /// </summary>
        /// <param name="subScene"> subScene index </param>
        public void LoadSubScene(int index)
        {
            if (index < 0 || index >= subScenes.Length)
                return;

            LoadSubScene(subScenes[index]);
        }

        /// <summary>
        /// Loading a subScene by giving its name
        /// </summary>
        /// <param name="subScene"> subScene name </param>
        public void LoadSubScene(string name)
        {
            if (subScenes.Length <= 0)
                return;

            foreach (SubScene subScene in subScenes)
            {
                if (subScene.SceneName != name)
                    continue;
                else
                {
                    LoadSubScene(subScene);
                    return;
                }
            }
        }
        #endregion
    }
}
