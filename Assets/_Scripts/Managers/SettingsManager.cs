using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Managers;
using _Scripts.UI.Menus;
using _ScriptableObjects.GameManagement;

namespace _Scripts.UI.Interfaces
{
    public class SettingsManager : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Settings infos")]
        [SerializeField] private GeneralSettings settings;

        [TitleGroup("GUI references")]
        [SerializeField] private SliderProperty sensitivitySlider;
        [SerializeField] private SliderProperty masterSlider;
        [SerializeField] private SliderProperty effectsSlider;
        [SerializeField] private SliderProperty musicSlider;
        [SerializeField] private SliderProperty ambientSlider;

        private bool _initialized = false;
        #endregion

        #region Builts_In
        private void Awake()
        {
            settings.LoadSettings();
            InitializeSettings();
        }

        private void OnDisable()
        {
            SaveSettings();
        }

        private void OnDestroy()
        {
            SaveSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize settings values
        /// </summary>
        private void InitializeSettings()
        {
            _initialized = false;

            sensitivitySlider.InitializeValue(settings.sensitivity);
            masterSlider.InitializeValue(settings.globalVolume);
            effectsSlider.InitializeValue(settings.effectsVolume);
            musicSlider.InitializeValue(settings.musicVolume);
            ambientSlider.InitializeValue(settings.ambientVolume);
            _initialized = true;
        }

        /// <summary>
        /// Update volume properties
        /// </summary>
        public void UpdateVolumeSettings()
        {
            if (!_initialized)
                return;

            settings.globalVolume = masterSlider.Value;
            settings.effectsVolume = effectsSlider.Value;
            settings.musicVolume= musicSlider.Value;
            settings.ambientVolume = ambientSlider.Value;

            AudioManager.Instance.SetAudioProperties();
        }

        /// <summary>
        /// Update input parameters values
        /// </summary>
        public void UpdateInputSettings()
        {
            if (!_initialized)
                return;

            settings.sensitivity = sensitivitySlider.Value;
        }

        /// <summary>
        /// Saving settings
        /// </summary>
        public void SaveSettings()
        {
            settings.SaveSettings();
        }
        #endregion
    }
}
