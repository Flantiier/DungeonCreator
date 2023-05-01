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
        /// <summary>
        /// Reset to the previous player tag
        /// </summary>
        public void GetCurrentTag()
        {
            textMesh.text = GetPlayerTag();
        }

        /// <summary>
        /// Enable or disable the input field
        /// </summary>
        public void EditPlayerTag(bool state)
        {
            textMesh.gameObject.SetActive(!state);
            inputField.gameObject.SetActive(state);
        }

        /// <summary>
        /// Override the previous tag by the sibmitted one
        /// </summary>
        public void SubmitPlayerTag()
        {
            textMesh.text = inputField.text;
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
