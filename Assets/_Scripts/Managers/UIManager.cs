using InputsMaps;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Managers
{
	public class UIManager : MonoBehaviourSingleton<UIManager>
	{
		#region Variables
		[Header("UI references")]
		[SerializeField] private GameObject gameHUD;
		[SerializeField] private GameObject optionsMenuHUD;

		private UIInputs _uiInputs;
		#endregion

		#region Events
		public event Action<bool> OnOptionsMenuChanged;
        #endregion

        #region Builts_In
        public override void Awake()
        {
			_uiInputs = new UIInputs();

			EnableGameplayUI(false);
            EnableUIElement(optionsMenuHUD, false);
        }

		public override void OnEnable()
		{
			_uiInputs.Enable();

			_uiInputs.InGameUI.Escape.started += HandleEscapeAction;
		}

		public override void OnDisable()
		{
            _uiInputs.Disable();

            _uiInputs.InGameUI.Escape.started -= HandleEscapeAction;
        }
        #endregion

        #region Methods

        #region Inputs
		/// <summary>
		/// Action to enable or disable the options menu
		/// </summary>
        private void HandleEscapeAction(InputAction.CallbackContext _)
		{
			if (!optionsMenuHUD)
				return;

            EnableUIElement(optionsMenuHUD, !optionsMenuHUD.activeInHierarchy);
		}
        #endregion

        #region UIEvents
		/// <summary>
		/// Event to send the option menu state
		/// </summary>
		/// <param name="state"> Enable or disable </param>
		public void InvokeOptionsMenuEvent(bool state)
		{
			OnOptionsMenuChanged?.Invoke(state);
		}
        #endregion

        /// <summary>
        /// Enable or disable an referenced object
        /// </summary>
        /// <param name="ui"> element to enable/disable </param>
        /// <param name="state"> Set to enable or disable </param>
        private void EnableUIElement(GameObject ui, bool state)
		{
			if (!ui)
				return;

			ui.SetActive(state);
		}

		/// <summary>
		/// Enable or disable all the gameplay at once
		/// </summary>
		private void EnableGameplayUI(bool state)
		{
			EnableUIElement(gameHUD, state);
		}
		#endregion

		#region Callbacks
		public override void OnJoinedRoom()
		{
			EnableGameplayUI(true);
		}
		#endregion
	}
}
