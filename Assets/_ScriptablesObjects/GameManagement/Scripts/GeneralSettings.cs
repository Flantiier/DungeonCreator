using _ScriptableObjects.GameManagement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ScriptableObjects.GameManagement
{
	[CreateAssetMenu(menuName = "SO/Game Management/General Settings")]
	public class GeneralSettings : ScriptableObject
	{
		public float sensitivity = 10f;
		public float maxSensitivity = 50f;
        public float globalVolume = 0.5f;
		public float effectsVolume = 0.5f;

		[Button("Save")]
		public void SaveSettings()
		{
			SettingsDatas datas = new SettingsDatas(this);
			SaveSystem.Save(datas, "_settings");
		}

        [Button("Load")]
        public void LoadSettings()
		{
			SettingsDatas datas = new SettingsDatas();
			SaveSystem.Load(ref datas, "_settings");

			sensitivity = datas.sensitivity;
			maxSensitivity = datas.maxSensitivity;
			globalVolume = datas.globalVolume;
			effectsVolume = datas.effectsVolume;
		}
	}
}

public class SettingsDatas
{
	public float sensitivity;
	public float maxSensitivity;
    public float globalVolume;
	public float effectsVolume;

	public SettingsDatas() { }

	public SettingsDatas(GeneralSettings settings)
	{
		sensitivity = settings.sensitivity;
		maxSensitivity = settings.maxSensitivity;
        globalVolume = settings.globalVolume;
		effectsVolume = settings.effectsVolume;
	}
}
