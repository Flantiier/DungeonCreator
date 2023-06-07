using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Gameplay
{
	public class SliderElement : MonoBehaviour
	{
        #region Variables
		[SerializeField] protected FloatVariable variable;
        [SerializeField] protected Slider slider;
        #endregion

        #region Builts_In
        protected virtual void Start()
		{
			slider.maxValue = variable.value;
			SetSliderValue();
		}

		protected virtual void Update()
		{
			SetSliderValue();
		}
        #endregion

        #region Methods
        protected virtual void SetSliderValue()
		{
			slider.value = variable.value;
		}
        #endregion
    }
}
