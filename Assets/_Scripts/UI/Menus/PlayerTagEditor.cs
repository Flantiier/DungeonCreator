using Photon.Pun;
using TMPro;
using UnityEngine;

namespace _Scripts.Menus
{
    public class PlayerTagEditor : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TMP_InputField inputField;
        #endregion

        #region Builts_In
        private void Awake()
        {
            GetCurrentTag();
        }
        #endregion

        #region Methods
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
            string m_name = GetPlayerTag();
            inputField.text = m_name;
            SetPhotonName(m_name);
        }

        /// <summary>
        /// Override the previous tag by the sibmitted one
        /// </summary>
        public void SubmitPlayerTag()
        {
            SetPhotonName(inputField.text);
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
