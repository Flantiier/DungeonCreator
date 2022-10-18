using UnityEngine;

namespace _Scripts.UI
{
    public class HealthSlider : CharacterSlider
    {
        #region Methods
        protected override void SetSliderBounds()
        {
            _slider.minValue = 0f;
            _slider.maxValue = Character.CharacterDatas.health;
        }

        protected override void UpdateSlider()
        {
            _slider.value = Character.CurrentHealth;
        }
        #endregion
    }
}