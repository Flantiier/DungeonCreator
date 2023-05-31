using UnityEngine;
using UnityEngine.UI;
using _ScriptableObjects.GameManagement;
using _Scripts.UI.Menus;
using Sirenix.OdinInspector;

namespace _Scripts.UI.Interfaces
{
    public class DisplaySettings : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Settings infos")]
        [SerializeField] private GeneralSettings settings;
        [TitleGroup("GUI references")]
        [SerializeField] private SliderProperty sensitivitySlider;
        [SerializeField] private SliderProperty gVolumeSlider;
        [SerializeField] private SliderProperty eVolumeSlider;
        #endregion

        #region Builts_In
        private void Awake()
        {
            settings.LoadSettings();
            sensitivitySlider.slider.maxValue = settings.maxSensitivity;
        }

        private void OnEnable()
        {
            SetSettingsValue();
        }

        private void OnDisable()
        {
            SaveSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set sliders values based on SO general settings
        /// </summary>
        public void SetSettingsValue()
        {
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
        #endregion
    }
}
