using System;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using static Cinemachine.AxisState;
using static Cinemachine.CinemachineTransposer;

#region Cinemachine Transposer Properties
[Serializable]
public struct TransposerProperties
{
    public BindingMode m_BindingMode;
    public Vector3 m_FollowOffset;
    [Range(0f, 20f)]
    public float m_XDamping, m_YDamping, m_ZDamping;
    public AngularDampingMode m_AngularDampingMode;
    [Range(0f, 20f)]
    public float m_PitchDamping, m_YawDamping, m_RollDamping;
    [Range(0f, 20f)]
    public float m_AngularDamping;

    public TransposerProperties(int _)
    {
        m_BindingMode = BindingMode.LockToTargetWithWorldUp;
        m_FollowOffset = Vector3.back * 10f;
        m_XDamping = 1f;
        m_YDamping = 1f;
        m_ZDamping = 1f;
        m_AngularDampingMode = AngularDampingMode.Euler;
        m_PitchDamping = 0;
        m_YawDamping = 0;
        m_RollDamping = 0;
        m_AngularDamping = 0f;
    }

    public void SetTranposer(CinemachineTransposer tranposer)
    {
        tranposer.m_BindingMode = m_BindingMode;
        tranposer.m_FollowOffset = m_FollowOffset;
        tranposer.m_XDamping = m_XDamping;
        tranposer.m_YDamping = m_YDamping;
        tranposer.m_ZDamping = m_ZDamping;
        tranposer.m_AngularDamping = m_AngularDamping;
        tranposer.m_PitchDamping = m_PitchDamping;
        tranposer.m_YawDamping = m_YawDamping;
        tranposer.m_RollDamping = m_RollDamping;
        tranposer.m_AngularDamping = m_AngularDamping;
    }
}
#endregion

#region Cinemachine FramingTranposer Properties
[Serializable]
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

    public FramingTransposerProperties(int _)
    {
        m_TrackedObjectOffset = Vector3.zero;
        m_LookaheadTime = 0;
        m_LookaheadSmoothing = 0;
        m_LookaheadIgnoreY = false;
        m_XDamping = 1f;
        m_YDamping = 1f;
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

#region Cinemachine POV Properties
[Serializable]
public struct POVProperties
{
    [GUIColor(1, 1, 0.5f)]
    public AxisProperties horizontalAxis;
    [GUIColor(0.5f, 1, 1)]
    public AxisProperties verticalAxis;

    public POVProperties(int _)
    {
        horizontalAxis = new AxisProperties(-180, 180, false, true);
        verticalAxis = new AxisProperties(-70, 70, true, false);
    }

    public void SetPOV(CinemachinePOV pov)
    {
        horizontalAxis.SetAxisState(pov.m_HorizontalAxis);
        verticalAxis.SetAxisState(pov.m_VerticalAxis);
    }
}
#endregion

#region Cinemachine Composer Properties
[Serializable]
public struct ComposerProperties
{
    public Vector3 m_TrackedObjectOffset;
    [Range(0f, 1f)]
    [Space]
    public float m_LookaheadTime;
    [Range(0, 30)]
    public float m_LookaheadSmoothing;
    public bool m_LookaheadIgnoreY;
    [Range(0f, 20f)]
    public float m_HorizontalDamping, m_VerticalDamping;
    [Range(-0.5f, 1.5f)]
    public float m_ScreenX, m_ScreenY;
    [Space]
    [Range(0f, 2f)]
    public float m_DeadZoneWidth;
    [Range(0f, 2f)]
    public float m_DeadZoneHeight;
    [Range(0f, 2f)]
    public float m_SoftZoneWidth, m_SoftZoneHeight;
    [Range(-0.5f, 0.5f)]
    public float m_BiasX, m_BiasY;
    public bool m_centerOnActivate;

    public ComposerProperties(int _)
    {
        m_TrackedObjectOffset = Vector3.zero;
        m_LookaheadTime = 0;
        m_LookaheadSmoothing = 0;
        m_LookaheadIgnoreY = false;
        m_HorizontalDamping = 0f;
        m_VerticalDamping = 0f;
        m_ScreenX = 0.5f;
        m_ScreenY = 0.5f;
        m_DeadZoneWidth = 0f;
        m_DeadZoneHeight = 0f;
        m_SoftZoneWidth = 0.8f;
        m_SoftZoneHeight = 0.8f;
        m_BiasX = 0f;
        m_BiasY = 0f;
        m_centerOnActivate = false;
    }

    public void SetComposer(CinemachineComposer composer)
    {
        composer.m_TrackedObjectOffset = m_TrackedObjectOffset;
        composer.m_LookaheadTime = m_LookaheadTime;
        composer.m_LookaheadSmoothing = m_LookaheadSmoothing;
        composer.m_LookaheadTime = m_LookaheadTime;
        composer.m_HorizontalDamping = m_HorizontalDamping;
        composer.m_VerticalDamping = m_VerticalDamping;
        composer.m_ScreenX = m_ScreenX;
        composer.m_ScreenY = m_ScreenY;
        composer.m_DeadZoneWidth = m_DeadZoneWidth;
        composer.m_DeadZoneHeight = m_DeadZoneHeight;
        composer.m_SoftZoneWidth = m_SoftZoneWidth;
        composer.m_SoftZoneHeight = m_SoftZoneHeight;
        composer.m_BiasX = m_BiasX;
        composer.m_BiasY = m_BiasY;
        composer.m_CenterOnActivate = m_centerOnActivate;
    }
}
#endregion

#region AxisProperties
[Serializable]
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