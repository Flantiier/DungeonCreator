using UnityEngine;
using _Scripts.Characters;

namespace _Scripts.UI.Interfaces
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private CharacterSlider healthSlider;
        [SerializeField] private CharacterSlider staminaSlider;

        /// <summary>
        /// Set sliders character reference
        /// </summary>
        /// <param name="character"> Referenced character </param>
        public void SetHUD(Characters.Character character)
        {
            healthSlider.SetPlayer(character);
            staminaSlider.SetPlayer(character);
        }
    }
}
