using UnityEngine;
using UnityEngine.SceneManagement;
using _Scripts.Managers;
using Photon.Pun;

namespace _Scripts.UI.Interfaces
{
    public class PauseMenu : MonoBehaviour
    {
        #region Variables
        [Header("Menus info")]
        [SerializeField] private GameObject mainMenu;
        #endregion

        #region Builts_In
        private void OnEnable()
        {
            GameUIManager.Instance.InvokeOptionsMenuEvent(true);
            mainMenu.SetActive(true);
        }

        private void OnDisable()
        {
            GameUIManager.Instance.InvokeOptionsMenuEvent(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method assigned to the resume button
        /// </summary>
        public void ResumeMethod()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Method assigned to the leave button
        /// </summary>
        public void LeaveMethod(string SceneToLoad)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(SceneToLoad);
        }
        #endregion
    }
}
