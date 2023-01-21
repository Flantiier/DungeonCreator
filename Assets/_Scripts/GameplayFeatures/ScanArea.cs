using System.Collections;
using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures
{
	public class ScanArea : NetworkMonoBehaviour
	{
		#region Variables
		[Header("Area properties")]
		[SerializeField] private float remainTime = 2f;
		[SerializeField] private float scanDuration = 5f;

        [Header("Feedback")]
		[SerializeField] private Color feedbackColor = new Color(0f, 255f, 255f, 50f);
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

			detectable.GetDetected(scanDuration);
		}

		private void OnDrawGizmos()
		{
			if (!TryGetComponent(out SphereCollider collider))
				return;

			Gizmos.color = feedbackColor;
			Gizmos.DrawWireSphere(transform.position, collider.radius);
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
