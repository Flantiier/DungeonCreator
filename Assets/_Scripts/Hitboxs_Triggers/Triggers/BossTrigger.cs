using UnityEngine;
using Photon.Pun;
using _Scripts.Managers;
using _Scripts.Characters;

namespace _Scripts.Hitboxs_Triggers.Triggers
{
    public class BossTrigger : ListingTrigger<Character>
    {
        [Header("Trigger properties")]
        [SerializeField] private int requiredNumb = 3;

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            TriggerBossFight();
        }

        private void TriggerBossFight()
        {
            if (_triggerList.Count < requiredNumb)
                return;

            gameObject.SetActive(false);

            if (!PhotonNetwork.IsMasterClient)
                return;

            Debug.Log("Boss can be trigered");
            GameManager.Instance.BossFightReached();
        }
    }
}