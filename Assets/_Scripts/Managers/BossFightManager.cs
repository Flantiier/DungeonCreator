using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Characters.DungeonMaster;
using _Scripts.Characters;

namespace _Scripts.Managers
{
	public class BossFightManager : NetworkMonoBehaviour
	{
		#region Variables
		[SerializeField] private GameObject bossUI;
		[SerializeField] private GameObject advBossUI;
        [SerializeField] private Transform[] spawnPositions;
		[SerializeField] private GameEvent startBossFightEvent;

        private Character[] _adventurers;
		private BossController _boss;
		bool stop;
		bool started = false;
        #endregion

        private void LateUpdate()
        {
			if (!started)
				return;

			if (_adventurers.Length <= 0 || !_boss)
				return;

			if (stop)
				return;

			//Adventurer win
			if(_boss.CurrentHealth <= 0)
			{
				Debug.Log("Win Adventurer");
				GameManager.Instance.EndGame(EndGameReason.AdventurerWin);
                stop = true;
            }

            if (CheckBossWin())
            {
				Debug.Log("Win Master");
				GameManager.Instance.EndGame(EndGameReason.MasterWin);
                stop = true;
			}
        }

		private bool CheckBossWin()
		{
            foreach (Character item in _adventurers)
            {
				if (item.CurrentHealth > 0)
					return false;
				else 
					continue;
            }

			return true;
        }

        #region Methods
        [ContextMenu("Start Boss Fight")]
		public void StartBossFight()
		{
            Role role = PlayersManager.Role;

			switch (role)
			{
				case Role.Master:
					SwicthDMToBoss();
					Instantiate(bossUI);
					break;
				default:
					HandleAdventurers(role);
					Instantiate(advBossUI);
					break;
            }

			_adventurers = FindObjectsOfType<Character>();
			_boss = FindObjectOfType<BossController>();

			startBossFightEvent.Raise();
            started = true;
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
			BossController boss = FindObjectOfType<BossController>();
			boss.EnableBoss();
		}

		/// <summary>
		/// Teleport the player in the boss area
		/// </summary>
		private void HandleAdventurers(Role role)
		{
			Transform spawn = role == Role.Warrior ? spawnPositions[0] :
									role == Role.Archer ? spawnPositions[1] : spawnPositions[2];

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
