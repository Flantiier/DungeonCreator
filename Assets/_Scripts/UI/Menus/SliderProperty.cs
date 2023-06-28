using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Menus
{
    public class SliderProperty : MonoBehaviour
    {
        #region Variables/Properties
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private float minValue = 0f, maxValue = 1f;
        [SerializeField] private float multiplicator = 1f;

        public Slider Slider => slider;
        public float Value => Slider.value;
        #endregion

        #region Methods
        public void InitializeValue(float value)
        {
            slider.enabled = false;
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = value;
            UpdateText();

            slider.enabled = true;
        }

        public void UpdateText()
        {
            textMesh.text = ((int)(slider.value * multiplicator)).ToString();
        }
        #endregion
    }
}
