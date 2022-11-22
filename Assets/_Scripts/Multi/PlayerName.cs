using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Scripts.Multi
{
    public class PlayerName : MonoBehaviour
    {
        private string nameOfPlayer;
        private string saveName;

        public TMP_Text inputText;
        public TMP_Text loadedName;

        void Update()
        {
            nameOfPlayer = PlayerPrefs.GetString("name", "none");
            loadedName.text = nameOfPlayer;
        }

        public void SetName()
        {
            saveName = inputText.text;
            PlayerPrefs.SetString("name", saveName);
        }
    }
}