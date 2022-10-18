using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;
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
            if(!trapSO) return;
            if(!trapDamageableSO) return;
        }

        private void OnColliderEnter(Collision collision)
        {
/*            if (!collision.TryGetComponent(out IDamageable damage))
            {
                return;
            }
            else
            {
                trapDamageableSO.health -= damage.TakeDamage();

                if (trapDamageableSO.health <= 0)
                {
                    Destroy(gameObject);
                }
            }*/
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerDamageable player))
            {
                return;
            }
            else
            {
                if (trapSO)
                {
                    player.DamagePlayer(trapSO.damage);
                }
                else if (trapDamageableSO)
                {
                    player.DamagePlayer(trapDamageableSO.damage);
                }
                else
                {
                    return;
                }
                //se trouve dans player HUD (script)
                // player._healthBarImage.fillAmount = _currentHealth / adventurerDatas.health;
            }
        }
    }
}