using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Characters.DungeonMaster;
using _Scripts.Multi.Connexion;
using _Scripts.Characters;

namespace _Scripts.Managers
{
	public class BossFightManager : NetworkMonoBehaviour
	{
		#region Variables
		[SerializeField] private Transform[] spawnPositions;
		[SerializeField] private GameEvent startBossFightEvent;
        #endregion

        #region Methods
		public void StartBossFight()
		{
            ListPlayersRoom.Roles role = PlayersManager.Role;

			switch (role)
			{
				case ListPlayersRoom.Roles.DM:
					SwicthDMToBoss();
					break;
				default:
					HandleAdventurers(role);
					break;
            }

			startBossFightEvent.Raise();
		}

		/// <summary>
		/// Swicth DM controller to Boss controller
		/// </summary>
		private void SwicthDMToBoss()
		{
			//Disable DM
			DMController dm = FindObjectOfType<DMController>();
			dm.DisableCharacter();

			//Enable Boss
			BossController boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
			boss.gameObject.SetActive(true);
			boss.EnableBoss();
		}

		/// <summary>
		/// Teleport the player in the boss area
		/// </summary>
		private void HandleAdventurers(ListPlayersRoom.Roles role)
		{
			Transform spawn = role == ListPlayersRoom.Roles.Warrior ? spawnPositions[0] :
									role == ListPlayersRoom.Roles.Archer ? spawnPositions[1] : spawnPositions[2];

			Character player = FindPlayer();

			//No player
			if (!player)
				return;

			player.TeleportPlayer(spawn);
		}

		/// <summary>
		/// Return the local character
		/// </summary>
		private Character FindPlayer()
		{
			Character[] characters = FindObjectsOfType<Character>();

			foreach (Character character in characters)
			{
				if (!character || !character.ViewIsMine())
					continue;

				return character;
			}

			return null;
		}
		#endregion
	}
}
