using System.Collections;
using System.Collections.Generic;
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
            int randomPoint = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomPoint];

            GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerCharacter"]];
            PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        }
    }
}