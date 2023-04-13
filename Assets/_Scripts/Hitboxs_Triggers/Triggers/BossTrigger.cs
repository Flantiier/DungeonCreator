using UnityEngine;
using Photon.Pun;
using TMPro;
using _Scripts.Characters;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    public class BossTrigger : ListingTrigger<Character>
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private GameEvent reachedBossEvent;

        private void Awake()
        {
            textMesh.gameObject.SetActive(false);
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Character character))
                return;

            //Add player to a list
            base.OnTriggerEnter(other);
            textMesh.gameObject.SetActive(true);

            //Trigger boss fight if possible
            TriggerBossFight();
        }

        public override void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Character character))
                return;

            base.OnTriggerExit(other);
            textMesh.gameObject.SetActive(false);
        }

        [ContextMenu("Trigger boss fight")]
        private void TriggerBossFight()
        {
            /*int playerCount = PhotonNetwork.PlayerList.Length - 1;

            if (List.Count < playerCount)
            {
                textMesh.text = $"En attente des autres joueurs.. {List.Count}/{playerCount}";
                return;
            }*/

            textMesh.text = "Lets go le boss";
            reachedBossEvent.Raise();
        }
    }
}