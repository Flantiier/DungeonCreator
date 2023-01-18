using UnityEngine;
using Photon.Pun;
using _Scripts.Managers;

namespace _Scripts.GameplayFeatures
{
    public class BossTrigger : Trigger
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
            if (_charactersInTrigger.Count < requiredNumb)
                return;

            gameObject.SetActive(false);

            if (!PhotonNetwork.IsMasterClient)
                return;

            Debug.Log("Boss can be trigered");
            GameManager.Instance.BossFightReached();
        }
    }
}