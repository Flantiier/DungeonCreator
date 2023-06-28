using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Managers;
using _Scripts.UI.Menus;
using _ScriptableObjects.GameManagement;
using Unity.VisualScripting;
using System.Collections;

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
        #endregion

        #region Builts_In
        private void Awake()
        {
            settings.LoadSettings();
        }

        private void Start()
        {
            InitializeSettings();
        }

        private void OnDisable()
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
            sensitivitySlider.SetValue(settings.sensitivity);
            masterSlider.SetValue(settings.globalVolume);
            effectsSlider.SetValue(settings.effectsVolume);
            musicSlider.SetValue(settings.musicVolume);
            ambientSlider.SetValue(settings.ambientVolume);
        }

        /// <summary>
        /// Update volume properties
        /// </summary>
        public void UpdateVolumeSettings()
        {
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
