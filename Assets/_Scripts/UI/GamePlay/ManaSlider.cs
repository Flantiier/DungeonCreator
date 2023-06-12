using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Gameplay
{
	public class ManaSlider : SliderElement
	{
        #region Variables
        [SerializeField] private Slider slicedSlider;
        #endregion

        #region Builts_In
        protected override void Start()
        {
            slider.maxValue = variable.value;
            slicedSlider.maxValue = variable.value;
            SetSliderValue();
        }
        #endregion

        #region Methods
        protected override void SetSliderValue()
        {
            base.SetSliderValue();
            slicedSlider.value = Mathf.Floor(variable.value);
        }
		#endregion
	}
}
