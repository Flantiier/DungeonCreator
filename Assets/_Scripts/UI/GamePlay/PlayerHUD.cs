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
        public Character Character { get; private set; }
        #endregion

        #region Builts-In
        private void OnEnable()
        {
            skillImage.fillAmount = 0;
            skillText.gameObject.SetActive(skillCooldown.value > 0);
        }

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
            skillText.gameObject.SetActive(skillCooldown.value > 0);

            //Skill hud
            if (!Character.PlayerSM.SkillUsed)
                return;

            float amount = skillCooldown.value / Character.CharacterDatas.skillCooldown;
            skillImage.fillAmount = amount;
            skillText.gameObject.SetActive(skillCooldown.value > 0);
            skillText.text = Mathf.Ceil(skillCooldown.value).ToString();
        }
        #endregion
    }
}
