using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.UI.Interfaces
{
    public class BossHUD : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private SkillSlider firstSkill;
        [SerializeField] private SkillSlider secondSkill;
        public BossController Boss { get; private set; }
        #endregion

        #region Builts_In
        private void Start()
        {
            Boss = FindObjectOfType<BossController>();
            healthSlider.maxValue = Boss.Datas.health;
            staminaSlider.maxValue = Boss.Datas.stamina;
        }

        private void LateUpdate()
        {
            if (!Boss)
                return;

            healthSlider.value = Boss.CurrentHealth;
            staminaSlider.value = Boss.Stamina;
            HandleSlider(firstSkill, Boss.Datas.firstAbilityRecovery, Boss.FirstAbility.Cooldown);
            HandleSlider(secondSkill, Boss.Datas.secondAbilityRecovery, Boss.SecondAbility.Cooldown);
        }
        #endregion

        #region Methods
        private void HandleSlider(SkillSlider slider, float max, float current)
        {
            float amount = current / max;
            slider.skillImage.fillAmount = 1 - amount;
            Color color = slider.skillImage.color;
            color.a = current > 0 ? slider.alphaValue : 1f;
            slider.skillImage.color = color;

            slider.skillText.gameObject.SetActive(current > 0);
            slider.skillText.text = Mathf.Ceil(current).ToString();
        }
        #endregion
    }

    #region SkillSlider struct
    [System.Serializable]
    public struct SkillSlider
    {
        public Image skillImage;
        public TextMeshProUGUI skillText;
        public float alphaValue;
    }
    #endregion
}
