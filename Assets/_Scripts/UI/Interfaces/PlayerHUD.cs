using UnityEngine;
using _Scripts.Characters;
using UnityEngine.Rendering;

namespace _Scripts.UI.Interfaces
{
    public class PlayerHUD : MonoBehaviour
    {
        #region Variables/Properties
        [SerializeField] private CharacterSlider healthSlider;
        [SerializeField] private CharacterSlider staminaSlider;

        public Character Character { get; private set; }
        #endregion

        #region Builts-In
        private void LateUpdate()
        {
            SetHUD();
        }
        #endregion

        #region Methods
        public void SetTargetCharacter(Character character)
        {
            Character = character;
        }

        /// <summary>
        /// Set sliders character reference
        /// </summary>
        private void SetHUD()
        {
            if (!Character)
                return;

            healthSlider.SetPlayer(Character);
            staminaSlider.SetPlayer(Character);
        }
        #endregion
    }
}
