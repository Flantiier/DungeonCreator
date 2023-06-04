using System;
using UnityEngine;
using Unity.Scenes;
using Unity.Entities;

namespace _Scripts.Managers
{
    public class SubsceneLoader : MonoBehaviourSingleton<SubsceneLoader>
    {
        #region Variables
        [SerializeField] private SubSceneData[] subScenes;
        [SerializeField] private string[] mapStartArray;

        private SceneSystem _sceneSystem;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            _sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load a subScene in the subscene array
        /// </summary>
        /// <param name="sceneName"> Scene name to load </param>
        public void LoadSubscene(string sceneName)
        {
            string name = "SubScene_" + sceneName;
            SubSceneData subSceneData = Array.Find(subScenes, x => x.SubScene == name);
            subSceneData.Load(_sceneSystem);
        }

        /// <summary>
        /// Unload a subScene in the subscene array
        /// </summary>
        /// <param name="sceneName"> Scene name to unload </param>
        public void UnloadSubscene(string sceneName)
        {
            string name = "SubScene_" + sceneName;
            SubSceneData subSceneData = Array.Find(subScenes, x => x.SubScene == name);
            subSceneData.Unload(_sceneSystem);
        }
        #endregion
    }

    #region Subscene Data class
    [Serializable]
    public class SubSceneData
    {
        [SerializeField] private SubScene subScene;
        [SerializeField] private GameObject colliders;

        public string SubScene => subScene.SceneName;
        public bool IsLoaded { get; set; } = false;

        public void Load(SceneSystem system)
        {
            if (IsLoaded)
                return;

            colliders.SetActive(true);
            system.LoadSceneAsync(subScene.SceneGUID);
            IsLoaded = true;
        }

        public void Unload(SceneSystem system)
        {
            if (!IsLoaded)
                return;

            colliders.SetActive(false);
            system.UnloadScene(subScene.SceneGUID);
            IsLoaded = false;
        }
    }
    #endregion
}
