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

        [Tooltip("Le text ou sera affiché le nom du joueur")]
        [SerializeField] private TMP_Text loadedName;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            string defaultName = string.Empty;

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
            loadedName.text = PlayerPrefs.GetString(playerNamePrefKey);
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
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
            loadedName.text = PlayerPrefs.GetString(playerNamePrefKey);
        }

        #endregion
    }
}