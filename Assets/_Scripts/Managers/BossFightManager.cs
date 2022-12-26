using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Characters;

namespace _Scripts.Managers
{
	public class BossFightManager : NetworkMonoBehaviour
	{
		#region Variables
		[Header("Fight references")]
		[SerializeField] private Transform[] combatPoints;
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
		#endregion
	}
}
