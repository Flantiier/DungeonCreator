using Utils;
using UnityEngine;
using Cinemachine;

namespace _Scripts.Cameras
{
    public class CinemachineShaker : MonoBehaviour
    {
        #region Variables
        private CinemachineBasicMultiChannelPerlin _perlin;
        private float _shakeTime;
        private float _totalShakeTime;
        private float _shakeIntensity;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            HandleShakeDuration();
        }
        #endregion

        [ContextMenu("Shake")]
        public void Test()
        {
            ShakeCamera(3f, 3f);
        }

        #region Methods
        /// <summary>
        /// Shaking the camera with a certain intensity
        /// </summary>
        /// <param name="intensity"> Shake intensity </param>
        /// <param name="duration"> Shaking duration </param>
        public void ShakeCamera(float intensity, float duration)
        {
            if (!_perlin)
                return;

            _perlin.m_AmplitudeGain = intensity;
            _shakeIntensity = intensity;
            _totalShakeTime = duration;
            _shakeTime = duration;
        }

        /// <summary>
        /// Shaking duration method
        /// </summary>
        private void HandleShakeDuration()
        {
            if (_shakeTime > 0)
            {
                _shakeTime -= Time.deltaTime;

                //Approximately equal to 0
                if (Utilities.Math.ApproximationRange(_perlin.m_AmplitudeGain, 0f, 0.1f))
                    _perlin.m_AmplitudeGain = 0f;
                else
                    _perlin.m_AmplitudeGain = Mathf.Lerp(_shakeIntensity, 0f, 1 - (_shakeTime / _totalShakeTime));
            }
        }
        #endregion
    }
}
