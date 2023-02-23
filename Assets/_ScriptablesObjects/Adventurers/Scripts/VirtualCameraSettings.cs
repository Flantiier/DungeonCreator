using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using static Cinemachine.AxisState;
using UnityEngine.Serialization;

namespace _ScriptableObjects.Settings.Adventurer.Camera
{
    [CreateAssetMenu(fileName = "New VirtualCam Settings", menuName = "Cinemachine/VirtualCamera Settings")]
    public class VirtualCameraSettings : ScriptableObject
    {
        [FoldoutGroup("Body Properties"), HideLabel, GUIColor(1.5f, 1, 0.5f)]
        public FramingTransposerProperties framingTranposer;
        [FoldoutGroup("Aim", GroupName = "Aim Properties"), TitleGroup("Aim/X Axis"), HideLabel, GUIColor(1, 1, 0.5f)]
        public AxisProperties xAxis = new AxisProperties(-180, 180, false, true);
        [FoldoutGroup("Aim", GroupName = "Aim Properties"), TitleGroup("Aim/Y Axis"), HideLabel, GUIColor(0.5f, 1, 1)]
        public AxisProperties yAxis = new AxisProperties(-70, 70, true, false);

        public void SetVirtualCameraProperties(CinemachineVirtualCamera vCam)
        {
            framingTranposer.SetFramingTranposer(vCam.GetCinemachineComponent<CinemachineFramingTransposer>());
            CinemachinePOV pov = vCam.GetCinemachineComponent<CinemachinePOV>();
            xAxis.SetAxisState(pov.m_HorizontalAxis);
            yAxis.SetAxisState(pov.m_VerticalAxis);
        }
    }

    #region FramingTransposerProperties
    [System.Serializable]
    public struct FramingTransposerProperties
    {
        public Vector3 m_TrackedObjectOffset;
        [Range(0f, 1f)]
        [Space]
        public float m_LookaheadTime;
        [Range(0, 30)]
        public float m_LookaheadSmoothing;
        public bool m_LookaheadIgnoreY;
        [Range(0f, 20f)]
        public float m_XDamping, m_YDamping, m_ZDamping;
        public bool m_TargetMovementOnly;
        [Range(-0.5f, 1.5f)]
        public float m_ScreenX, m_ScreenY;
        public float m_CameraDistance;
        [Space]
        [Range(0f, 2f)]
        public float m_DeadZoneWidth;
        [Range(0f, 2f)]
        public float m_DeadZoneHeight;
        public float m_DeadZoneDepth;
        [Range(0f, 2f)]
        public float m_SoftZoneWidth, m_SoftZoneHeight;
        [Range(-0.5f, 0.5f)]
        public float m_BiasX, m_BiasY;
        public bool m_centerOnActivate;

        public FramingTransposerProperties(float _)
        {
            m_TrackedObjectOffset = Vector3.zero;
            m_LookaheadTime = 0;
            m_LookaheadSmoothing = 0;
            m_LookaheadIgnoreY = false;
            m_XDamping = 1f;
            m_YDamping= 1f;
            m_ZDamping = 1f;
            m_TargetMovementOnly = true;
            m_ScreenX = 0.5f;
            m_ScreenY = 0.5f;
            m_CameraDistance = 10f;
            m_DeadZoneWidth = 0f;
            m_DeadZoneHeight = 0f;
            m_DeadZoneDepth = 0;
            m_SoftZoneWidth = 0.8f;
            m_SoftZoneHeight = 0.8f;
            m_BiasX = 0f;
            m_BiasY = 0f;
            m_centerOnActivate = false;
        }

        public void SetFramingTranposer(CinemachineFramingTransposer tranposer)
        {
            tranposer.m_TrackedObjectOffset = m_TrackedObjectOffset;
            tranposer.m_LookaheadTime = m_LookaheadTime;
            tranposer.m_LookaheadSmoothing = m_LookaheadSmoothing;
            tranposer.m_LookaheadTime = m_LookaheadTime;
            tranposer.m_XDamping = m_XDamping;
            tranposer.m_YDamping = m_YDamping;
            tranposer.m_ZDamping = m_ZDamping;
            tranposer.m_TargetMovementOnly = m_TargetMovementOnly;
            tranposer.m_ScreenX = m_ScreenX;
            tranposer.m_ScreenY = m_ScreenY;
            tranposer.m_CameraDistance = m_CameraDistance;
            tranposer.m_DeadZoneWidth = m_DeadZoneWidth;
            tranposer.m_DeadZoneHeight = m_DeadZoneHeight;
            tranposer.m_DeadZoneDepth = m_DeadZoneDepth;
            tranposer.m_SoftZoneWidth = m_SoftZoneWidth;
            tranposer.m_SoftZoneHeight = m_SoftZoneHeight;
            tranposer.m_BiasX = m_BiasX;
            tranposer.m_BiasY = m_BiasY;
            tranposer.m_CenterOnActivate = m_centerOnActivate;
        }
    }
    #endregion

    #region AxisProperties
    [System.Serializable]
    public struct AxisProperties
    {
        public SpeedMode m_SpeedMode;
        public float m_MaxSpeed;
        public float m_AccelTime;
        public float m_DecelTime;
        public bool m_InvertInput;
        public float m_MinValue;
        public float m_MaxValue;
        public bool m_Wrap;
        public Recentering m_Recentering;

        public AxisProperties(float min, float max, bool invert, bool wrap)
        {
            m_SpeedMode = SpeedMode.InputValueGain;
            m_MaxSpeed = 0.05f;
            m_AccelTime = 0.1f;
            m_DecelTime = 0.1f;
            m_InvertInput = invert;
            m_MinValue = min;
            m_MaxValue = max;
            m_Wrap = wrap;
            m_Recentering = new Recentering();
        }

        public void SetAxisState(AxisState axis)
        {
            axis.m_SpeedMode = m_SpeedMode;
            axis.m_MaxSpeed = m_MaxSpeed;
            axis.m_AccelTime = m_AccelTime;
            axis.m_DecelTime = m_DecelTime;
            axis.m_InvertInput = m_InvertInput;
            axis.m_MinValue = m_MinValue;
            axis.m_MaxValue = m_MaxValue;
            axis.m_Recentering = m_Recentering;
        }
    }
    #endregion
}