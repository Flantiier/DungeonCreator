using UnityEngine;

namespace Assets._Scripts.Multi.Gameplay
{
    [System.Serializable]
    public class Timer
    {
        #region Properties
        public bool TimerIsBlocked { get; set; }
        public float CurrentTime { get; private set; }
        #endregion

        #region Builts_In
        /// <summary>
        /// Set the initial time in the constructor
        /// </summary>
        /// <param name="initialValue"> Initial time </param>
        public Timer(float initialValue)
        {
            CurrentTime = initialValue;
        }

        /// <summary>
        /// Decrease or Increase the timer value based on the clockwise parameter
        /// </summary>
        public void SetTimer(bool increase)
        {
            if (TimerIsBlocked)
                return;

            if (increase)
            {
                CurrentTime += Time.deltaTime;
                return;
            }

            CurrentTime -= Time.deltaTime;
        }
        #endregion
    }
}