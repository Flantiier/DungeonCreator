using System.Collections;
using UnityEngine;
using _Scripts.Characters;

namespace _Scripts.Managers
{
    public class RespawnManager : MonoBehaviour
    {
        [SerializeField] private FloatVariable respawnTime;
        [SerializeField] private Vector3Variable respawnPosition;

        /// <summary>
        /// Start a coroutine to respawn the player
        /// </summary>
        public void StartRespawnDelay(Character character)
        {
            StartCoroutine(RespawnRoutine(character));
        }

        /// <summary>
        /// Set the player position to a target position
        /// </summary>
        private void RespawnPlayer(Character character)
        {
            if (!character)
                return;

            character.gameObject.SetActive(false);
            character.transform.position = respawnPosition.value;
            character.gameObject.SetActive(true);
        }

        /// <summary>
        /// Respawn coroutine
        /// </summary>
        /// <param name="character"> Character to respawn </param>
        private IEnumerator RespawnRoutine(Character character)
        {
            respawnTime.value = 5f;

            while (respawnTime.value > 0)
            {
                respawnTime.value -= Time.deltaTime;
                yield return null;
            }

            respawnTime.value = 0;
            RespawnPlayer(character);
        }
    }
}
