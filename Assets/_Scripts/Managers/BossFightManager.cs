using System;
using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Characters;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.Managers
{
	public class BossFightManager : NetworkMonoBehaviour
	{
		#region Variables
		[Header("Fight references")]
		[SerializeField] private Transform[] combatPoints;

        public static event Action OnBossFightReached;
        public static event Action OnBossFightStarted;
        #endregion

        #region Builts_In
        public override void OnEnable()
		{
			//GameManager.Instance.OnBossFightReached += TeleportPlayerInArea;
		}

		public override void OnDisable()
		{
            //GameManager.Instance.OnBossFightReached -= TeleportPlayerInArea;
        }
        #endregion

        #region Methods
		/// <summary>
		/// Teleport player at fight boss positions
		/// </summary>
        private void TeleportPlayerInArea()
		{
			if (!PhotonNetwork.IsMasterClient)
				return;

			for (int i = 0; i < PlayersManager.Instance.AdventurersInstances.Count; i++)
			{
				Character character = PlayersManager.Instance.AdventurersInstances[i];

                if (!character)
					return;

				character.GetTeleported(combatPoints[i].position);
            }
        }

        /// <summary>
        /// Called when the boss fight is reached
        /// </summary>
        public static void BossFightReached()
        {
            GameManager.Instance.GameStatement.CurrentState = GameStatements.Statements.BossFight;
            OnBossFightReached?.Invoke();
        }

        /// <summary>
        /// Starting the fight boss
        /// </summary>
        public static void StartBossFight()
        {
            OnBossFightStarted?.Invoke();
        }
		#endregion
	}
}
