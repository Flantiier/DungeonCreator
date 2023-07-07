using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using _Scripts.Characters;
using _Scripts.NetworkScript;
using _Scripts.Managers;

namespace _Scripts.UI.Gameplay
{
    public class GameInfosHandler : NetworkMonoBehaviour
    {
        #region Variables
        [SerializeField] private FloatVariable time;
        [SerializeField] private float cleanUpTime = 20f;
        [SerializeField] private float delayListenerTime = 20f;

        private TextMeshProUGUI _textMesh;
        private float _lastListener;
        private Coroutine _routine;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            ClearText();
            Character.OnCharacterDeath += InfosListenerRPC;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Character.OnCharacterDeath -= InfosListenerRPC;
        }
        #endregion

        #region Methods
        private void InfosListenerRPC(Character character)
        {
            string nickname = character.View.Owner.NickName;
            View.RPC("InfosListener", RpcTarget.All, nickname);
        }

        [PunRPC]
        public void InfosListener(string nickname)
        {
            if (Time.time >= _lastListener + delayListenerTime)
                ClearText();

            string message = GetTextListener(nickname);
            _textMesh.text = _textMesh.text.Length <= 0 ? message : $"\r\n {message}";

            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(CleanUpRoutine());
            _lastListener = Time.time;
        }

        private string GetTextListener(string nickname)
        {
            string timeText;
            if (!GameManager.Instance.BossFightStarted)
            {
                if (time.value > 60)
                {
                    float seconds = time.value % 60;
                    float minuts = Mathf.Floor(time.value / 60);
                    timeText = minuts.ToString("00") + ":" + seconds.ToString("00");
                }
                else
                    timeText = time.value.ToString("F2");

                return timeText + " | " + $"{nickname} est mort.";
            }
            else
                return $"{nickname} est mort.";
        }

        private void ClearText()
        {
            _textMesh.text = "";
        }

        private IEnumerator CleanUpRoutine()
        {
            yield return new WaitForSecondsRealtime(cleanUpTime);
            ClearText();
        }
        #endregion
    }
}
