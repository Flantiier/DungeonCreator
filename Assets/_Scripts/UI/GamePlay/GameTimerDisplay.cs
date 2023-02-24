using UnityEngine;
using _Scripts.UI.Gameplay;
using _Scripts.Managers;

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
        {/*
            float seconds = GameManager.Instance.GameTime.ClampedSeconds;
            float minuts = GameManager.Instance.GameTime.RemainingMinuts;
            string timer = string.Format("{0:0}:{1:00}", minuts, seconds);
            textMesh.SetText(timer);*/
        }
        #endregion
    }
}
