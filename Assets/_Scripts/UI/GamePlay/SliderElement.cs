using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Gameplay
{
	public class SliderElement : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private ClampedFloatVariable variable;

		private void Start()
		{
			slider.maxValue = variable.maxValue;
			SetSliderValue();
		}

		private void Update()
		{
			SetSliderValue();
		}

		private void SetSliderValue()
		{
			slider.value = variable.value;
		}
	}
}
