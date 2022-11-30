using UnityEngine;
using _Scripts.UI.Gameplay;
using _Scripts.Managers;
using _Scripts.Utilities.Florian;
using JetBrains.Annotations;

namespace _Scripts.UI
{
	public class GameTimerDisplay : DisplayText
	{
		#region Builts_In
		private void Update()
		{
			UpdateText();
		}
        #endregion

        #region Inherited Methods
        public override void UpdateText()
        {
            float seconds = GameManager.Instance.RemainingGameTime;
            float minuts = (int)TimeFunctions.GetConvertedTime(seconds, TimeFunctions.TimeUnit.Minuts);
            string timer = string.Format("{0:0}:{1:00}", minuts, seconds % 60);
            textMesh.SetText(timer);
        }
        #endregion
    }
}
