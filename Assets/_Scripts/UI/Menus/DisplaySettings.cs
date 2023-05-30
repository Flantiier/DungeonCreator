using UnityEngine;
using UnityEngine.UI;
using _ScriptableObjects.GameManagement;
using _Scripts.UI.Menus;

namespace _Scripts.UI.Interfaces
{
	public class DisplaySettings : MonoBehaviour
	{
		[SerializeField] private GeneralSettings settings;
		[Header("GUI")]
		[SerializeField] private SliderProperty sensitivitySlider;
		[SerializeField] private SliderProperty gVolumeSlider;
		[SerializeField] private SliderProperty eVolumeSlider;

        private void Awake()
        {
			settings.LoadSettings();
			SetSettingsValue();
        }

        private void OnDisable()
        {
			SaveSettings();
        }

        /// <summary>
        /// Set sliders values based on SO general settings
        /// </summary>
        public void SetSettingsValue()
		{
			sensitivitySlider.slider.maxValue = settings.maxSensitivity;
			sensitivitySlider.SetValue(settings.sensitivity);
			gVolumeSlider.SetValue(settings.globalVolume);
			eVolumeSlider.SetValue(settings.effectsVolume);
        }

		/// <summary>
		/// Set the values of the SO based on the slider values
		/// </summary>
		public void UpdateSettingsValue()
		{
			settings.sensitivity = sensitivitySlider.Value;
			settings.globalVolume = gVolumeSlider.Value;
			settings.effectsVolume = eVolumeSlider.Value;
		}

		public void SaveSettings()
		{
			settings.SaveSettings();
		}
	}
}
