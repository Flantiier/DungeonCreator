using Photon.Pun;
using TMPro;
using UnityEngine;

namespace _Scripts.Menus
{
    public class PlayerTagEditor : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI textMesh;
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
            textMesh.text = m_name;
            SetPhotonName(m_name);
        }

        /// <summary>
        /// Enable or disable the input field
        /// </summary>
        public void EditPlayerTag()
        {
            textMesh.gameObject.SetActive(!textMesh.gameObject.activeSelf);
            inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
            inputField.text = GetPlayerTag();
        }

        /// <summary>
        /// Override the previous tag by the sibmitted one
        /// </summary>
        public void SubmitPlayerTag()
        {
            textMesh.text = inputField.text;
            SetPhotonName(textMesh.text);
            SavePlayerTag(textMesh.text);
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
