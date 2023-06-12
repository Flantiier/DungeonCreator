using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Managers
{
    public class GameUIManager : NetworkMonoSingleton<GameUIManager>
    {
        #region Variables
        [TitleGroup("Input Actions")]
        [SerializeField] private InputAction menuInput;
        [TitleGroup("References")]
        [SerializeField] private GameObject gameHUD;
        [TitleGroup("References")]
        [SerializeField] private GameObject optionsMenuHUD;

        public static Action<bool> OnMenuOpen;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            EnableGameplayUI(true);
            EnableUIElement(optionsMenuHUD, false);
        }

        public override void OnEnable()
        {
            menuInput.Enable();
            menuInput.started += OnMenuInputPressed;

        }

        public override void OnDisable()
        {
            menuInput.Disable();
            menuInput.started -= OnMenuInputPressed;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Action to enable or disable the options menu
        /// </summary>
        private void OnMenuInputPressed(InputAction.CallbackContext _)
        {
            if (!optionsMenuHUD)
                return;

            OpenOptionsPanel();
        }

        /// <summary>
        /// Enable or disable all the gameplay at once
        /// </summary>
        private void EnableGameplayUI(bool state)
        {
            EnableUIElement(gameHUD, state);
        }

        //Open or close the options panel
        public void OpenOptionsPanel()
        {
            bool enabled = !optionsMenuHUD.activeInHierarchy;
            EnableUIElement(optionsMenuHUD, enabled);
            OnMenuOpen?.Invoke(!enabled);

            //Playing as DM and the boss hasn't started yet
            if (PlayersManager.Role == Role.Master && !GameManager.Instance.BossFightStarted)
                return;

            GameManager.Instance.EnableCursor(enabled);
        }

        /// <summary>
        /// Enable or disable an referenced object
        /// </summary>
        /// <param name="panel"> element to enable/disable </param>
        /// <param name="state"> Set to enable or disable </param>
        private void EnableUIElement(GameObject panel, bool state)
        {
            if (!panel)
                return;

            panel.SetActive(state);
        }
        #endregion

        #region Callbacks
#if UNITY_EDITOR
        public override void OnJoinedRoom()
        {
            EnableGameplayUI(true);
        }
#endif
        #endregion
    }
}
