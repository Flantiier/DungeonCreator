using Photon.Pun;
using UnityEngine;

namespace _Scripts.Characters
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PhotonView))]
	public class CharacterAudio : MonoBehaviour
	{
        #region Variables
        [SerializeField] private AudioClip[] clips;

        private AudioSource _audioSource;
		private PhotonView _view;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _view = GetComponent<PhotonView>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Play an audio clip in one shot
        /// </summary>
        public void PlayClip(int index)
		{
			if (index <= 0 || index >= clips.Length)
				return;

            PlaySoundRPC(index);
		}

        /// <summary>
        /// Play an audio clip in one shot over the network
        /// </summary>
        public void PlaySyncClip(int index)
		{
            if (index <= 0 || index >= clips.Length)
                return;

            _view.RPC("PlaySoundRPC", RpcTarget.All, index);
        }

        [PunRPC]
        private void PlaySoundRPC(int index)
        {
            AudioClip clip = clips[index];
            _audioSource.PlayOneShot(clip);
        }
		#endregion
	}
}
