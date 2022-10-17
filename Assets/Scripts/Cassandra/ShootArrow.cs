using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ShootArrow : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _arrowSpawnPosition;

    [SerializeField] private GameObject _arrowPrefab;
    private float _arrowSpeed = 20f;
    private float _timeLeft = 1f;

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if(_timeLeft <= 0)
        {
            StartCoroutine(Shoot());
            _timeLeft = 1f;
        }
    }

    IEnumerator Shoot()
    {
        GameObject _arrow = PhotonNetwork.Instantiate(_arrowPrefab.name, _arrowSpawnPosition.position, _arrowSpawnPosition.rotation, 0);
        _arrow.GetComponent<Rigidbody>().velocity = _arrowSpawnPosition.forward * _arrowSpeed;

        yield return new WaitForSeconds(3f);

        Destroy(_arrow);
        //TODO : Une fois le d�cors mis en place, destroy la fl�che si elle touche un �lement du d�cors (murs, d�co...)
    }
}
