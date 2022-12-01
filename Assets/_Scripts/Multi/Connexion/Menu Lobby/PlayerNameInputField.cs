using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace _Scripts.Multi.Connexion
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "Player Name";

        [Tooltip("Le text ou sera affich� le nom du joueur")]
        [SerializeField] private TMP_Text loadedName;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            string defaultName = "Player " + Random.Range(0, 100);

            InputField _inputField = GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
            loadedName.text = defaultName;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        /// <param name="value">The name of the Player</param>
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                string randomName = "Player " + Random.Range(0, 100);
                PhotonNetwork.NickName = randomName;
            }

            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
            loadedName.text = PlayerPrefs.GetString(playerNamePrefKey);
        }

        #endregion
    }
}