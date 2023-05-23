using System.Collections;
using UnityEngine;
using Utils;
using _Scripts.Characters;
using _ScriptableObjects.GameManagement;
using UnityEngine.Assertions.Must;

namespace _Scripts.Managers
{
    public class RespawnManager : MonoBehaviour
    {
        [SerializeField] private GameProperties properties;
        [SerializeField] private FloatVariable timeVariable;
        [SerializeField] private FloatVariable respawnTime;
        [SerializeField] private Vector3Variable respawnPosition;

        private void OnEnable()
        {
            Character.OnCharacterDeath += StartRespawnDelay;
        }

        private void OnDisable()
        {
            Character.OnCharacterDeath -= StartRespawnDelay;
        }

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
            character.transform.rotation = Quaternion.identity;
            character.gameObject.SetActive(true);
        }

        /// <summary>
        /// Respawn coroutine
        /// </summary>
        /// <param name="character"> Character to respawn </param>
        private IEnumerator RespawnRoutine(Character character)
        {
            respawnTime.value = GetRespawnTime();

            while (respawnTime.value > 0)
            {
                respawnTime.value -= Time.deltaTime;
                yield return null;
            }

            respawnTime.value = 0;
            RespawnPlayer(character);
        }

        [ContextMenu("Get time")]
        private float GetRespawnTime()
        {
            float time = 5f;
            foreach (RespawnUnit unit in properties.respawnInfos)
            {
                float max = Utilities.Time.ConvertTime(unit.maxTime, unit.timeUnit, Utilities.Time.TimeUnit.Seconds);
                float min = Utilities.Time.ConvertTime(unit.minTime, unit.timeUnit, Utilities.Time.TimeUnit.Seconds);

                if (timeVariable.value > min && timeVariable.value <= max)
                {
                    time = unit.respawnDelay;
                    break;
                }
                else
                    continue;
            }

            Debug.Log("Respawn time : " + time);
            return time;
        }
    }
}
