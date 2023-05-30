using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Menus
{
    public class SliderProperty : MonoBehaviour
    {
        public Slider slider;
        public TMP_InputField inputField;
        public float Value { get; private set; }

        public void SetValue(float value)
        {
            Value = value;
            slider.value = value;
            inputField.text = value.ToString();
        }

        public void SetValueBySlider()
        {
            Value = slider.value;
            inputField.text = Value.ToString("0.##");
        }

        public void SetValueByField()
        {
            try
            {
                Value = float.Parse(inputField.text);
                Value = Mathf.Clamp(Value, slider.minValue, slider.maxValue);
                inputField.text = Value.ToString("0.##");
                slider.value = Value;
            }
            catch
            {
                Debug.LogWarning("Counldn't convert string from inputField to a float");
                inputField.text = Value.ToString("0.##");
                slider.value = Value;
            }

        }
    }
}
