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
        [SerializeField] private bool networkAudio = false;
        [SerializeField] private float localVolume = 0.25f;
        [SerializeField] private float othersVolume = 0.1f;

        private AudioSource _audioSource;
		private PhotonView _view;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _view = GetComponent<PhotonView>();

            if (!networkAudio)
                return;

            bool isLocal = _view.IsMine;
            _audioSource.spatialBlend = isLocal ? 0f : 1f; 
            _audioSource.volume = isLocal ? localVolume : othersVolume;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Play an audio clip in one shot
        /// </summary>
        public void PlayClip(int index)
		{
			if (index < 0 || index >= clips.Length)
				return;

            PlaySoundRPC(index);
		}

        /// <summary>
        /// Play an audio clip in one shot over the network
        /// </summary>
        public void PlaySyncClip(int index)
		{
            if (index < 0 || index >= clips.Length)
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
