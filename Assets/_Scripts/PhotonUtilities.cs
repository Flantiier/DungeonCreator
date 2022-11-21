using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace _Scripts.Utilities.Florian
{
	public static class PhotonUtilities
	{
        public abstract class PhotonAdditionals
        {
            /// <summary>
            /// Destroying a gameObject with selected delay
            /// </summary>
            /// <param name="view"> Target view to be destroy </param>
            /// <param name="delay"> Destroy delay </param>
            public static IEnumerator DestroyWithDelay(PhotonView view, float delay)
            {
                yield return new WaitForSecondsRealtime(delay);

                PhotonNetwork.DestroyAll(view);
            }
        }
    }
}
