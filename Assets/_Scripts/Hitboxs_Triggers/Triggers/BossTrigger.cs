using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using _Scripts.Characters;
using _Scripts.Managers;
using _Scripts.NetworkScript;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    public class BossTrigger : NetworkMonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Vector3 size = Vector3.one;
        [SerializeField] private LayerMask mask;

        private List<Character> _characters = new List<Character>();
        private bool _isTriggered;
        #endregion

        #region Builts_In
        private void Awake()
        {
            textMesh.gameObject.SetActive(false);
        }

        private void Update()
        {
            TriggerBossFight();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, size);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Detect player and trigger the boss fight
        /// </summary>
        private void TriggerBossFight()
        {
            if (_isTriggered)
                return;

            //Get players
            HandlePlayerList();
            int playerCount = PhotonNetwork.PlayerList.Length - 1;
            textMesh.gameObject.SetActive(_characters.Count > 0);

            if (_characters.Count <= 0)
                return;

            //Not all player in trigger
            if (_characters.Count < playerCount)
            {
                textMesh.text = $"En attente des autres aventuriers... {_characters.Count}/{playerCount}";
                return;
            }

            //all player are here, then trigger boss fight
            StartCoroutine("StartRoutine");
        }

        private IEnumerator StartRoutine()
        {
            _isTriggered = true;
            textMesh.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.4f);
            GameManager.Instance.StartBossFight();
        }

        /// <summary>
        /// Create a player list currently in trigger
        /// </summary>
        private void HandlePlayerList()
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, mask);
            List<Character> characters = new List<Character>();

            //Get character list
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];

                    if (!collider || !collider.TryGetComponent(out Character character))
                        continue;
                    else
                    {
                        if (character.CurrentHealth <= 0)
                            characters.RemoveAt(i);
                        else
                            characters.Add(character);
                    }
                }
            }

            _characters = characters;
        }
        #endregion
    }
}