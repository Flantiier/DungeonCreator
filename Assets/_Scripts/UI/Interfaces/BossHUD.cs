using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.UI.Interfaces
{
	public class BossHUD : MonoBehaviour
	{
        [SerializeField] private Slider slider;
        [SerializeField] private SkillSlider firstSkill;
        [SerializeField] private SkillSlider secondSkill;
        public BossController Boss { get; private set; }

        private void Start()
        {
            Boss = FindObjectOfType<BossController>();
            slider.maxValue = Boss.Datas.health;
        }

        private void LateUpdate()
        {
            if (!Boss)
                return;

            slider.value = Boss.CurrentHealth;
            HandleSlider(firstSkill, Boss.Datas.firstAbilityRecovery, Boss.FirstAbility.Cooldown);
            HandleSlider(secondSkill, Boss.Datas.secondAbilityRecovery, Boss.SecondAbility.Cooldown);
        }

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
    }

    [System.Serializable]
    public struct SkillSlider
    {
        public Image skillImage;
        public TextMeshProUGUI skillText;
        public float alphaValue;
    }
}
