using UnityEngine;
using _Scripts.UI;
using _Scripts.Characters;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private CharacterSlider healthSlider;
    [SerializeField] private CharacterSlider staminaSlider;

    public void SetHUD(Character character)
    {
        healthSlider.SetPlayer(character);
        staminaSlider.SetPlayer(character);
    }
}
