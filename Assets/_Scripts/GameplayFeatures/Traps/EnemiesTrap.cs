using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.IA;

namespace _Scripts.GameplayFeatures.Traps
{
	public class EnemiesTrap : TrapClass1
	{
        #region Variables
        [TitleGroup("Enemies List")]
		[SerializeField] private List<Enemy> enemiesList = new List<Enemy>();
        #endregion

        #region Builts_In
        protected void Awake() { }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            ResetOccupedTiles();
        }

        private void Update()
        {
			if (!ViewIsMine() || enemiesList.Count <= 0)
				return;

			CheckEnemiesList();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checking if the enemies filled in this base are still alive
        /// </summary>
        private void CheckEnemiesList()
		{
			for (int i = 0; i < enemiesList.Count; i++)
			{
				Enemy enemy = enemiesList[i];

				if (!enemy || enemy.CurrentHealth <= 0)
					enemiesList.RemoveAt(i);
				else
					continue;
			}

			if (enemiesList.Count > 0)
				return;

			ResetOccupedTiles();
        }

#if UNITY_EDITOR
		[ContextMenu("Get Enemies")]
		private void GetEnemies()
		{
            enemiesList = GetComponentsInChildren<Enemy>().ToList();
		}
#endif
        #endregion
    }
}
