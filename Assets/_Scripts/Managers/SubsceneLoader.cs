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
        [SerializeField] private int[] mapStart;
        [SerializeField] private int[] mapEnd;
        [SerializeField] private bool loadOverrite;

        private SceneSystem _sceneSystem;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            _sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();

            //Unload map
            UnloadAll();
        }

        private void Start()
        {
            if (PlayersManager.Role == Role.Master || loadOverrite)
            {
                LoadAll();
                return;
            }

            LoadMapStart();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load a subScene in the subscene array
        /// </summary>
        /// <param name="subscene"> Scene name to load </param>
        public void LoadSubscene(SubScene subscene)
        {
            SubSceneData subSceneData = Array.Find(subScenes, x => x.SubScene == subscene);
            if (subscene == null || subSceneData.IsLoaded)
                return;

            subSceneData.Load(_sceneSystem);
        }

        /// <summary>
        /// Unload a subScene in the subscene array
        /// </summary>
        /// <param name="subscene"> Scene name to unload </param>
        public void UnloadSubscene(SubScene subscene)
        {
            SubSceneData subSceneData = Array.Find(subScenes, x => x.SubScene == subscene);
            if (subscene == null || !subSceneData.IsLoaded)
                return;

            subSceneData.Unload(_sceneSystem);
        }

        /// <summary>
        /// Load the entire map
        /// </summary>
        public void LoadAll()
        {
            foreach (SubSceneData subscene in subScenes)
                LoadSubscene(subscene.SubScene);
        }

        /// <summary>
        /// Unload the entire map
        /// </summary>
        public void UnloadAll()
        {
            foreach (SubSceneData subscene in subScenes)
                UnloadSubscene(subscene.SubScene);
        }

        public void LoadMapStart()
        {
            UnloadAll();
            for (int i = 0; i < mapStart.Length; i++)
            {
                SubSceneData data = subScenes[mapStart[i]];
                LoadSubscene(data.SubScene);
            }
        }

        public void LoadMapEnd()
        {
            UnloadAll();
            for (int i = 0; i < mapEnd.Length; i++)
            {
                SubSceneData data = subScenes[mapStart[i]];
                LoadSubscene(data.SubScene);
            }
        }
        #endregion
    }

    #region Subscene Data class
    [Serializable]
    public class SubSceneData
    {
        [SerializeField] private SubScene subScene;
        [SerializeField] private GameObject colliders;

        public SubScene SubScene => subScene;
        public GameObject Colliders => colliders;
        public bool IsLoaded { get; set; } = false;

        public void Load(SceneSystem system)
        {
            if (IsLoaded)
                return;

            system.LoadSceneAsync(subScene.SceneGUID);
            colliders.SetActive(true);
            IsLoaded = true;
        }

        public void Unload(SceneSystem system)
        {
            if (!IsLoaded)
                return;

            system.UnloadScene(subScene.SceneGUID);
            colliders.SetActive(false);
            IsLoaded = false;
        }
    }
    #endregion
}
