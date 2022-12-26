using System.Collections;
using UnityEngine;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;
using Photon.Pun;

namespace _Scripts.GameplayFeatures
{
	public class ScanArea : NetworkMonoBehaviour
	{
		#region Variables
		[Header("Area properties")]
		[SerializeField] private float remainTime = 2f;
		#endregion

		#region Builts_In
		private void Awake()
		{
			if (!ViewIsMine())
				return;

			StartCoroutine("DurationRoutine");
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out IDetectable detectable))
				return;

			detectable.GetDetected();
		}
        #endregion

        #region Methods
		private IEnumerator DurationRoutine()
		{
			yield return new WaitForSeconds(remainTime);

			PhotonNetwork.Destroy(photonView);
		}
        #endregion
    }
}
