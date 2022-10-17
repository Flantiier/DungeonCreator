using UnityEngine;
using Photon.Pun;

namespace _Scripts.TrapSystem.Datas
{
    public class TrapManager : MonoBehaviourPunCallbacks
    {
        [Header("Trap Info")]
        public TrapSO trapSO;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.tag = "Trap";

            if(!trapSO) return;
        }

        //Trigger with Traps
        private void OnTriggerEnter(Collider other)
        {
            //if the trap have the tag Trap
            if(other.gameObject.tag == "Player")
            {
                // if(_trapManager.trapSO)
                // {
                //     _currentHealth -= _trapManager.trapSO.damage;
                // }

                // _healthBarImage.fillAmount = _currentHealth / adventurerDatas.health;

                // if(other.gameObject.name == "flechebaliste")
                // {
                //     Destroy(other.gameObject);
                // }

                // if(_currentHealth <= 0)
                // {
                //     Destroy(gameObject);
                // }
            }

            // [SerializeField] private AdventurerData adventurerDatas;
            // public AdventurerData AdventurerDatas => adventurerDatas;

            // [SerializeField] private Image _healthBarImage;
            // [SerializeField] private GameObject _playerUICanvas;
        }
    }
}