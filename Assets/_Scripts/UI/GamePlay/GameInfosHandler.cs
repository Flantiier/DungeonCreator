using TMPro;
using UnityEngine;
using Photon.Pun;
using _Scripts.Characters;

namespace _Scripts.UI.Gameplay
{
    public class GameInfosHandler : MonoBehaviour
    {
        #region Variables
        [SerializeField] private FloatVariable time;
        [SerializeField] private float cleanUpTime = 20f;

        private TextMeshProUGUI _textMesh;
        private float _lastListener;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            ClearText();
            Character.OnCharacterDeath += InfosListener;
        }

        private void OnDisable()
        {
            Character.OnCharacterDeath -= InfosListener;
        }
        #endregion

        #region Methods
        private void InfosListener(Character character)
        {
            _lastListener = Time.time;

            if (Time.time > _lastListener + cleanUpTime)
                ClearText();

            //Get time into string
            string timeText;
            if (time.value > 60)
            {
                float seconds = time.value % 60;
                float minuts = Mathf.Floor(time.value / 60);
                timeText = minuts.ToString("00") + ":" + seconds.ToString("00");
            }
            else
                timeText = time.value.ToString("F2");

            PhotonView view = character.GetComponent<PhotonView>();
            string infos = timeText + " | " + $"{view.Owner.NickName} est mort.";

            if (_textMesh.text.Length <= 0)
                _textMesh.text = infos;
            else
                _textMesh.text = $"\r\n {infos}";
        }

        private void ClearText()
        {
            _textMesh.text = "";
        }
        #endregion
    }
}
