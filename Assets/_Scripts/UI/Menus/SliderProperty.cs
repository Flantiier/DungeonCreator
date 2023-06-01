using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace _Scripts.UI.Menus
{
    public class SliderProperty : MonoBehaviour
    {
        #region Variables/Properties
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_InputField inputField;
        private float _value;

        public float Value
        {
            get => _value;
            private set
            {
                _value = value;
                inputField.text = value.ToString();
                slider.value = value;
            }
        }
        public float MaxValue { get; private set; }
        public Slider Slider => slider;
        #endregion

        #region Methods
        /// <summary>
        /// Set slider and inputField value
        /// </summary>
        public void SetValue(float value, float maxValue)
        {
            slider.maxValue = maxValue;
            Value = value;
        }

        /// <summary>
        /// Set value by using a slider
        /// </summary>
        public void SetValueBySlider()
        {
            float value = (float)Math.Round(slider.value, 2);
            Value = value;
        }

        /// <summary>
        /// Set value by using a input field 
        /// </summary>
        public void SetValueByField()
        {
            try
            {
                float value = float.Parse(inputField.text);
                value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
                Math.Round(value, 2);
                Value = value;
            }
            catch
            {
                Debug.LogWarning("Counldn't convert string from inputField to a float");
                inputField.text = Value.ToString("0.##");
                slider.value = Value;
            }
        }
        #endregion
    }
}
