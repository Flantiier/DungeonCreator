using Photon.Pun;
using System.Collections;
using UnityEngine;
using Photon.Realtime;

namespace _Scripts.TrapSystem.Datas
{
    public class ShootArrow : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _arrowSpawnPosition;
        [SerializeField] private GameObject _arrowPrefab;

        private float _arrowSpeed = 20f;
        private float _timeLeft = 1f;
        private GameObject _arrow;

        void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                StartCoroutine(Shoot());
                _timeLeft = 1f;
            }
        }

        IEnumerator Shoot()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if(player.IsLocal)
                {
                    _arrow = PhotonNetwork.Instantiate(_arrowPrefab.name, _arrowSpawnPosition.position, _arrowSpawnPosition.rotation);
                    _arrow.GetComponent<Rigidbody>().velocity = _arrowSpawnPosition.forward * _arrowSpeed;

                    yield return new WaitForSeconds(3f);
                    Destroy(_arrow);
                }
                else
                {
                    yield break;
                }
            }
            //TODO : Une fois le d�cors mis en place, destroy la fl�che si elle touche un �lement du d�cors (murs, d�co...)
        }
    }
}