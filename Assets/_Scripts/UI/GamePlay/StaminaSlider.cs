using UnityEngine;

namespace _Scripts.UI
{
    public class StaminaSlider : CharacterSlider
    {
        #region Builts_In
        protected override void SetSliderBounds()
        {
            _slider.minValue = 0f;
            _slider.maxValue = Character.CharacterDatas.stamina;
        }
        protected override void UpdateSlider()
        {
            _slider.value = Character.CurrentStamina;
        }
        #endregion
    }
}