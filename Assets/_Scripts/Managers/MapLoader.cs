using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers
{
    public class MapLoader : MonoBehaviourSingleton<MapLoader>
    {
        #region Variables
        [SerializeField] private List<GameObject> scenes = new List<GameObject>();
        [SerializeField] private GameObject[] loadAtStart;
        [SerializeField] private bool loadAllOverrite;
        #endregion

        #region Builts_In
        private void Start()
        {
            if (PlayersManager.Role == Role.Master || loadAllOverrite)
                return;

            EnableMapStart();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load an additive scene
        /// </summary>
        public void EnableMapPart(GameObject scene)
        {
            if (!scene || !scenes.Contains(scene) || scene.activeInHierarchy)
                return;

            scene.SetActive(true);
        }

        /// <summary>
        /// Unload an additive scene
        /// </summary>
        public void DisableMapPart(GameObject scene)
        {
            if (!scene || !scenes.Contains(scene) || !scene.activeInHierarchy)
                return;

            scene.SetActive(false);
        }

        /// <summary>
        /// Load all scene in the array
        /// </summary>
		public void EnableAllMap()
        {
            foreach (GameObject scene in scenes)
                EnableMapPart(scene);
        }

        /// <summary>
        /// Load all scene in the array
        /// </summary>
        private void DisableAllMap()
        {
            foreach (GameObject scene in scenes)
                DisableMapPart(scene);
        }

        /// <summary>
        /// Enable only the beginning of the map
        /// </summary>
        public void EnableMapStart()
        {
            DisableAllMap();
            foreach (GameObject obj in loadAtStart)
                obj.SetActive(true);
        }
        #endregion
    }
}
