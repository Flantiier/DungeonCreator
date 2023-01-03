
using System.Collections;
using UnityEngine;

namespace _Scripts.Managers
{
    public class RespawnManager : MonoBehaviourSingleton<RespawnManager>
    {
        #region Properties
        public float RespawnTime { get; private set; }
        #endregion

        #region Builts_In
        public override void OnEnable()
        {
            Characters.Character.OnCharacterDeath += StartRespawnDelay;
        }

        public override void OnDisable()
        {
            Characters.Character.OnCharacterDeath -= StartRespawnDelay;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start a coroutine to respawn the player
        /// </summary>
        private void StartRespawnDelay()
        {
            float duration = GetRespawnTime();
            Vector3 respawnPosition = Vector3.zero;

            StartCoroutine(RespawnRoutine(duration, respawnPosition));
        }

        /// <summary>
        /// Set the player position to a target position
        /// </summary>
        /// <param name="position"> Respawn position </param>
        private void RespawnPlayer(Vector3 position)
        {
            if (!PlayersManager.Instance.PlayerInstance)
                return;

            GameObject player = PlayersManager.Instance.PlayerInstance;

            player.SetActive(false);
            player.transform.position = position;
            player.SetActive(true);
        }

        /// <summary>
        /// Respawn coroutine
        /// </summary>
        /// <param name="duration"> Coroutine duration </param>
        /// <param name="position"> Respawn position </param>
        private IEnumerator RespawnRoutine(float duration, Vector3 position)
        {
            yield return new WaitForSecondsRealtime(duration);

            Debug.LogWarning("Coucou");
            RespawnPlayer(position);
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
