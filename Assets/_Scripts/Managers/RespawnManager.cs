
using System.Collections;
using UnityEngine;
using _Scripts.Characters;

namespace _Scripts.Managers
{
    public class RespawnManager : NetworkMonoSingleton<RespawnManager>
    {
        #region Properties
        public float RespawnTime { get; private set; }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            Character.OnCharacterDeath += StartRespawnDelay;
        }

        public override void OnDisable()
        {
            Character.OnCharacterDeath -= StartRespawnDelay;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start a coroutine to respawn the player
        /// </summary>
        private void StartRespawnDelay(Character character)
        {
            //float duration = GetRespawnTime();
            float duration = 5f;
            Vector3 respawnPosition = Vector3.zero;

            StartCoroutine(RespawnRoutine(character, respawnPosition, duration));
        }

        /// <summary>
        /// Set the player position to a target position
        /// </summary>
        /// <param name="position"> Respawn position </param>
        private void RespawnPlayer(Character character, Vector3 position)
        {
            if (!character)
                return;

            character.gameObject.SetActive(false);
            character.transform.position = position;
            character.gameObject.SetActive(true);
        }

        /// <summary>
        /// Respawn coroutine
        /// </summary>
        /// <param name="character"> Character to respawn </param>
        /// <param name="position"> Respawn position </param>
        /// <param name="duration"> Respawn duration </param>
        private IEnumerator RespawnRoutine(Character character, Vector3 position, float duration)
        {
            yield return new WaitForSecondsRealtime(duration);

            RespawnPlayer(character, position);
        }

        /// <summary>
        /// Get the corresponding respawn delay between time bounds set inthe game settings
        /// </summary>
        /// <returns></returns>
        private float GetRespawnTime()
        {
            RespawnUnit[] units = GameManager.Instance.GameSettings.respawnUnits;

            if (units.Length <= 0)
            {
                Debug.LogWarning("No respawn units set in game settings scriptable object, applied time to respawn (10s)");
                return 10f;
            }

            float minuts = GameManager.Instance.GameTime.RemainingMinuts;

            for (int i = 0; i < units.Length; i++)
            {
                if (minuts >= units[i].minBound && minuts < units[i].maxBound)
                    return units[i].respawnDelay;
                else
                    continue;
            }

            return units[0].respawnDelay;
        }
        #endregion
    }
}
