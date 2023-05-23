using UnityEngine;
using Unity.Scenes;
using Unity.Entities;
using Sirenix.OdinInspector;

namespace _Scripts.Managers
{
    public class SubScenesManager : MonoBehaviourSingleton<SubScenesManager>
    {
        [SerializeField] private SubScene[] loadedAtStart;
        private SubScene[] _subScenes;
        private SceneSystem _sceneSystem;

        public override void Awake()
        {
            base.Awake();
            _sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();
            _subScenes = GetSubScenes();

            UnloadAll(_subScenes);
            LoadAll(loadedAtStart);
        }

        /// <summary>
        /// Get all SubScenes in the scene
        /// </summary>
        private SubScene[] GetSubScenes()
        {
            var m_subScenes = FindObjectsOfType<SubScene>();
            return m_subScenes;
        }

        /// <summary>
        /// Load a given subScene
        /// </summary>
        /// <param name="scene"> SubScene to load </param>
        public void LoadSubScene(SubScene scene)
        {
            if (scene.IsLoaded)
                return;

            _sceneSystem.LoadSceneAsync(scene.SceneGUID);
        }

        /// <summary>
        /// Unload a given subScene
        /// </summary>
        /// <param name="scene"> SubScene to unload </param>
        public void UnloadSubScene(SubScene scene)
        {
            _sceneSystem.UnloadScene(scene.SceneGUID);
        }

        /// <summary>
        /// Load all subScenes in the list
        /// </summary>
        private void LoadAll(SubScene[] scenes)
        {
            foreach (SubScene scene in scenes)
                LoadSubScene(scene);
        }

        /// <summary>
        /// Unload all subscenes in the list
        /// </summary>
        private void UnloadAll(SubScene[] scenes)
        {
            foreach (SubScene scene in scenes)
                UnloadSubScene(scene);
        }
    }
}
