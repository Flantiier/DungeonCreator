using Utils;
using UnityEngine;
using UnityEngine.Audio;
using _ScriptableObjects.GameManagement;

namespace _Scripts.Managers
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {
        #region Variables
        [SerializeField] private GeneralSettings settings;
        [SerializeField] private AudioMixer masterMixerGroup;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            SetAudioProperties();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set AudioMixer volume values
        /// </summary>
        public void SetAudioProperties()
        {
            masterMixerGroup.SetFloat("MasterVol", Utilities.Math.ValueToVolume(settings.globalVolume));
            masterMixerGroup.SetFloat("EffectsVol", Utilities.Math.ValueToVolume(settings.effectsVolume));
            masterMixerGroup.SetFloat("MusicsVol", Utilities.Math.ValueToVolume(settings.musicVolume));
            masterMixerGroup.SetFloat("AmbientVol", Utilities.Math.ValueToVolume(settings.ambientVolume));
        }
        #endregion
    }
}
