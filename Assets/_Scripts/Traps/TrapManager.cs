using UnityEngine;
using Photon.Pun;
using _Scripts.Characters;

namespace _Scripts.TrapSystem.Datas
{
    public class TrapManager : MonoBehaviourPunCallbacks
    {
        [Header("Trap Info")]
        public TrapSO trapSO;
        public TrapDamageableSO trapDamageableSO;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.tag = "Trap";

            if(!trapSO) return;
            if(!trapDamageableSO) return;
        }

        //Trigger with Traps
        private void OnTriggerEnter(Collider other)
        {
            //if the trap have the tag Trap
            if(other.gameObject.tag == "Player")
            {
                Character _player = other.gameObject.GetComponent<Character>();

                if (trapSO)
                {
                    _player.DamagePlayer(trapSO.damage);
                }
                if (trapDamageableSO)
                {
                    _player.DamagePlayer(trapDamageableSO.damage);
                }

                if (_player.PlayerStateMachine.CurrentState == Characters.StateMachines.PlayerStateMachine.PlayerStates.Attack && trapDamageableSO)
                {
                    trapDamageableSO.health -= 1f;

                    if (trapDamageableSO.health <= 0)
                    {
                        Destroy(trapDamageableSO);
                    }
                }
                // [SerializeField] private Image _healthBarImage;
                // [SerializeField] private GameObject _playerUICanvas;
                // _healthBarImage.fillAmount = _currentHealth / adventurerDatas.health;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Character _player = other.gameObject.GetComponent<Character>();

                if (trapSO && trapSO.isContinuous)
                {
                    _player.DamagePlayer(trapSO.damage);
                }
                else if (trapDamageableSO && trapDamageableSO.isContinuous)
                {
                    _player.DamagePlayer(trapDamageableSO.damage);
                }
                else
                {
                    return;
                }
            }
        }
    }
}