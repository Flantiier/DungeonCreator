using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters;

namespace _Scripts.UI.Interfaces
{
    public class PlayerHUD : MonoBehaviour
    {
        #region Variables/Properties
        [Header("Sliders")]
        [SerializeField] private CharacterSlider healthSlider;
        [SerializeField] private CharacterSlider staminaSlider;

        [Header("Skill GUI")]
        [SerializeField] private FloatVariable skillCooldown;
        [SerializeField] private Image skillImage;
        [SerializeField] private TextMeshProUGUI skillText;
        [SerializeField] private float alphaValue = 0.5f;

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

            //Sliders
            healthSlider.SetPlayer(Character);
            staminaSlider.SetPlayer(Character);

            //Skill hud
            float amount = skillCooldown.value / Character.CharacterDatas.skillCooldown;
            skillImage.fillAmount = 1 - amount;
            Color color = skillImage.color;
            color.a = skillCooldown.value > 0 ? alphaValue : 1f;
            skillImage.color = color; 

            skillText.gameObject.SetActive(skillCooldown.value > 0);
            skillText.text = Mathf.Ceil(skillCooldown.value).ToString();
        }
        #endregion
    }
}
