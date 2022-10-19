using UnityEngine;
using Photon.Pun;
using _Scripts.Interfaces;
using _Scripts.Characters;
using _Scripts.Characters.StateMachines;

namespace _Scripts.TrapSystem.Datas
{
    public class TrapManager : MonoBehaviourPunCallbacks
    {
        [Header("Trap Info")]
        public TrapSO trapSO;
        public TrapDamageableSO trapDamageableSO;

        private Animator _animator;
        private ParticleSystem _particle;

        // Start is called before the first frame update
        void Start()
        {
            if(TryGetComponent(out Animator animator))
                _animator = GetComponent<Animator>();

            if (!trapSO) return;
            if (!trapDamageableSO) return;
        }

        private void OnCollisionEnter(Collision collision)
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

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerDamageable player)) return;
            if (trapSO) return;

            if (trapDamageableSO.isPlayerTrapped == true)
            {
                _animator.SetBool("isCaged", true);                
                //TODO : Faire la destruction de l'objet quand il n'a plus de points de vie
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerDamageable player)) return;

            if (trapSO)
            {
                player.DamagePlayer(trapSO.damage);
            }
            else if (trapDamageableSO)
            {
                player.DamagePlayer(trapDamageableSO.damage);
            }
            //se trouve dans player HUD (script)
        }

        private void PlayParticle()
        {
            _particle = GetComponentInChildren<ParticleSystem>();
            _particle.Play();
        }
    }
}