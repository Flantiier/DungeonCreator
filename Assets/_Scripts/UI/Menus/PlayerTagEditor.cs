using Photon.Pun;
using TMPro;
using UnityEngine;

namespace _Scripts.Menus
{
    public class PlayerTagEditor : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_InputField inputField;
        private string _currentName;
        #endregion

        #region Builts_In
        private void Start()
        {
            GetCurrentTag();
        }

        private void OnDestroy()
        {
            SubmitPlayerTag();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set Photon nickname to player pseudo
        /// </summary>
        /// <param name="name"></param>
        private void SetPhotonName(string name)
        {
            if (!PhotonNetwork.IsConnected)
                return;

            PhotonNetwork.NickName = name;
        }

        /// <summary>
        /// Reset to the previous player tag
        /// </summary>
        public void GetCurrentTag()
        {
            _currentName = GetPlayerTag();

            inputField.text = _currentName;
            SetPhotonName(_currentName);
        }

        /// <summary>
        /// Override the previous tag by the sibmitted one
        /// </summary>
        public void SubmitPlayerTag()
        {
            if (inputField.text.Length <= 0)
                inputField.text = _currentName;
            else
                _currentName = inputField.text;

            SetPhotonName(_currentName);
            SavePlayerTag(_currentName);
        }
        #region PlayerPrefs
        public string GetPlayerTag()
        {
            if (!PlayerPrefs.HasKey("PlayerTag"))
            {
                string tag = $"Player{Random.Range(0, 1000)}";
                SavePlayerTag(tag);
                return tag;
            }

            return PlayerPrefs.GetString("PlayerTag");
        }

        public void SavePlayerTag(string value)
        {
            PlayerPrefs.SetString("PlayerTag", value);
        }
        #endregion

        #endregion
    }
}
