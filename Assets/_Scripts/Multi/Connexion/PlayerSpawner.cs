using UnityEngine;
using Photon.Pun;

namespace _Scripts.Multi.Connexion
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Tooltip("Attention, les players prefabs doivent être dans le même ordre que les image sprites de la liste des joueurs de la room")]
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private Transform[] spawnPoints;

        void Start()
        {
            SpawnPlayer((int)PhotonNetwork.LocalPlayer.CustomProperties["playerCharacter"], Random.Range(0, spawnPoints.Length));
        }

        /// <summary>
        /// Spawn a character prefab based on the character index
        /// </summary>
        /// <param name="characterIndex"> Prefabs index to instantiate </param>
        /// <param name="positionIndex"> Spawn point index </param>
        private void SpawnPlayer(int characterIndex, int positionIndex)
        {
            Vector3 spawnPoint = !spawnPoints[positionIndex] ? Vector3.zero : spawnPoints[positionIndex].position;

            GameObject playerToSpawn = playerPrefabs[characterIndex];
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint, Quaternion.identity);
        }
    }
}