using UnityEngine;
using Photon.Pun;
using TMPro;
using _Scripts.Characters;
using _Scripts.Managers;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    public class BossTrigger : ListingTrigger<Character>
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI textMesh;
        #endregion

        #region Builts_In
        private void Awake()
        {
            textMesh.gameObject.SetActive(false);
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Character character))
                return;

            AddItem(character);
            textMesh.gameObject.SetActive(true);

            //Trigger boss fight if possible
            TriggerBossFight();
        }

        public override void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Character character))
                return;

            RemoveItem(character);
            textMesh.gameObject.SetActive(false);
        }
        #endregion

        #region Methods
        [ContextMenu("Trigger boss fight")]
        private void TriggerBossFight()
        {
            int playerCount = PhotonNetwork.PlayerList.Length - 1;

            if (List.Count < playerCount)
            {
                textMesh.text = $"En attente des autres aventuriers... {List.Count}/{playerCount}";
                return;
            }

            textMesh.gameObject.SetActive(false);

            if (PhotonNetwork.IsMasterClient)
                View.RPC("TriggerBossFightRPC", RpcTarget.All);
        }

        [PunRPC]
        public void TriggerBossFightRPC()
        {
            GameManager.Instance.StartBossFight();
        }
        #endregion
    }
}