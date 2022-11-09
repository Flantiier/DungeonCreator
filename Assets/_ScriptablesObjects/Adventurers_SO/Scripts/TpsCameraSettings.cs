using System;
using UnityEngine;
using Cinemachine;

namespace _SciptablesObjects.Settings.Adventurer.Camera
{
    [CreateAssetMenu(fileName = "New CameraSettings", menuName = "Scriptables/Adventurers/Camera")]
    public class TpsCameraSettings : ScriptableObject
    {
        #region Variables

        #region TPS Camera Properties
        [Header("User settings")]
        public float sensivity = 0.03f;
        public float aimSensivity = 0.03f;

        [Header("TPS camera properties")]
        [Range(0f, 10f)] public float tpsMinCameraDistance = 2f;
        [Range(0f, 10f)] public float tpsMaxCameraDistance = 5f;
        public CameraProperties tpsCamProperties;

        [Space] public float maxRecenteringDuration = 0.5f;
        public float recenteringTime = 0.1f;
        #endregion

        #region Aim Camera Properties
        [Header("AIM camera properties")]
        [Range(0f, 10f)] public float aimCameraDistance = 2f;
        public CameraProperties aimCamProperties;
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Set the max speed of a cinemachine POV component
        /// </summary>
        /// <param name="target"> target POV component </param>
        /// <param name="value"> maxSpeed value </param>
        public void SetSensitivity(CinemachinePOV target, float value)
        {
            target.m_HorizontalAxis.m_MaxSpeed = value;
            target.m_VerticalAxis.m_MaxSpeed = value;
        }

        /// <summary>
        /// Set Adventurer camera distance
        /// </summary>
        /// <param name="target"> Transposer component of the targeted camera</param>
        /// <param name="value"> Distance value </param>
        public void SetCameraDistance(CinemachineFramingTransposer target, float value)
        {
            target.m_CameraDistance = value;
        }

        /// <summary>
        /// Set Adventurer's VirtualCamera Body properties 
        /// </summary>
        /// <param name="target"> Transposer component of the targeted camera </param>
        /// <param name="properties"> Incoming values </param>
        public void SetBodyProperties(CinemachineFramingTransposer target, float cameraDistance, CameraProperties properties)
        {
            SetCameraDistance(target, cameraDistance);
            target.m_TrackedObjectOffset = properties.offsetFromTarget;

            target.m_LookaheadTime = properties.lookTargetTime;
            target.m_LookaheadSmoothing = properties.lookTargetSmooth;

            target.m_XDamping = properties.xDamping;
            target.m_YDamping = properties.yDamping;
            target.m_ZDamping = properties.zDamping;
        }

        /// <summary>
        /// Set Adventurer's VirtualCamera Aim properties 
        /// </summary>
        /// <param name="target"> POV component of the targeted camera </param>
        /// <param name="properties"> Incoming values </param>
        public void SetAimProperties(CinemachinePOV target, CameraProperties properties)
        {
            target.m_HorizontalAxis.m_MinValue = properties.horizontalAxis.minValue;
            target.m_HorizontalAxis.m_MaxValue = properties.horizontalAxis.maxValue;
            target.m_HorizontalAxis.m_AccelTime = properties.horizontalAxis.accelTime;
            target.m_HorizontalAxis.m_DecelTime = properties.horizontalAxis.decelTime;
            target.m_HorizontalAxis.m_InvertInput = properties.horizontalAxis.invertAxis;

            target.m_VerticalAxis.m_MinValue = properties.verticalAxis.minValue;
            target.m_VerticalAxis.m_MaxValue = properties.verticalAxis.maxValue;
            target.m_VerticalAxis.m_AccelTime = properties.verticalAxis.accelTime;
            target.m_VerticalAxis.m_DecelTime = properties.verticalAxis.decelTime;
            target.m_VerticalAxis.m_InvertInput = properties.verticalAxis.invertAxis;
        }

        /// <summary>
        /// Set recenter properties
        /// </summary>
        /// <param name="target"></param>
        public void SetRecenteringProperties(CinemachinePOV target)
        {
            target.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
            target.m_VerticalRecentering.m_RecenteringTime = recenteringTime;
        }
        #endregion
    }

    #region BodyProperties_Class
    [Serializable]
    public class CameraProperties
    {
        [Header("Body parameters")]
        public Vector3 offsetFromTarget = Vector3.zero;
        [Space]

        [Range(0f, 1f)] public float lookTargetTime = 0f;
        [Range(0f, 30f)] public float lookTargetSmooth = 0f;
        [Space]

        [Range(0f, 20f)] public float xDamping = 0f;
        [Range(0f, 20f)] public float yDamping = 0f;
        [Range(0f, 20f)] public float zDamping = 0f;

        [Space]

        [Header("Aim parameters")]
        public AimProperties horizontalAxis = new AimProperties(-180, 180, false);
        public AimProperties verticalAxis = new AimProperties(-70, 70, true);
    }
    #endregion

    #region AimProperties_Class
    [Serializable]
    public class AimProperties
    {
        public float minValue, maxValue;
        public float accelTime = 0.1f;
        public float decelTime = 0.1f;
        public bool invertAxis;

        public AimProperties(float _minValue, float _maxValue, bool _invert)
        {
            minValue = _minValue;
            maxValue = _maxValue;
            invertAxis = _invert;
        }
    }
    #endregion
}