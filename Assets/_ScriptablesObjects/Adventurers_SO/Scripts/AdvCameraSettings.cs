using System;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "New CameraSettings", menuName = "Scriptables/Adventurers/Camera")]
public class AdvCameraSettings : ScriptableObject
{
    #region Variables
    [Header("User sensivities")]
    public float sensivity = 0.03f;
    public float aimSensivity = 0.03f;

    [Header("TPS camera settings")]
    [Range(0f, 10f)] public float minCameraDistance = 2f;
    [Range(0f, 10f)] public float maxCameraDistance = 5f;
    public Vector3 offsetFromTarget = Vector3.zero;
    [Space]

    [Range(0f, 1f)] public float lookTargetTime = 0f;
    [Range(0f, 30f)] public float lookTargetSmooth = 0f;
    [Space]

    [Range(0f, 20f)] public float xDamping = 0f;
    [Range(0f, 20f)] public float yDamping = 0f;
    [Range(0f, 20f)] public float zDamping = 0f;

    [Header("Aim properties")]
    public AxisInfo horizontalAxis = new AxisInfo(-180, 180, false);
    public AxisInfo verticalAxis = new AxisInfo(-70, 70, true);
    #endregion

    #region Methods
    public void SetSensitivity(CinemachinePOV target, float value)
    {
        target.m_HorizontalAxis.m_MaxSpeed = value;
        target.m_VerticalAxis.m_MaxSpeed = value;
    }

    /*public void SetBodyProperties(CinemachineFramingTransposer target)
    {
        target.m_TrackedObjectOffset = offsetFromTarget;
        target.m_CameraDistance = cameraDistance;

        target.m_LookaheadTime = lookTargetTime;
        target.m_LookaheadSmoothing = lookTargetSmooth;

        target.m_XDamping = xDamping;
        target.m_YDamping = yDamping;
        target.m_ZDamping = zDamping;
    }

    public void SetAimProperties(CinemachinePOV target)
    {
        target.m_HorizontalAxis.m_MinValue = horizontalAxis.minValue;
        target.m_HorizontalAxis.m_MaxValue = horizontalAxis.maxValue;
        target.m_HorizontalAxis.m_AccelTime = horizontalAxis.accelTime;
        target.m_HorizontalAxis.m_DecelTime = horizontalAxis.decelTime;
        target.m_HorizontalAxis.m_InvertInput = horizontalAxis.invertAxis;

        target.m_VerticalAxis.m_MinValue = verticalAxis.minValue;
        target.m_VerticalAxis.m_MaxValue = verticalAxis.maxValue;
        target.m_VerticalAxis.m_AccelTime = verticalAxis.accelTime;
        target.m_VerticalAxis.m_DecelTime = verticalAxis.decelTime;
        target.m_VerticalAxis.m_InvertInput = verticalAxis.invertAxis;
    }*/
    #endregion
}

#region AimAxis_Class
[Serializable]
public class AxisInfo
{
    public float minValue, maxValue;
    public float accelTime = 0.1f;
    public float decelTime = 0.1f;
    public bool invertAxis;

    public AxisInfo(float _minValue, float _maxValue, bool _invert)
    {
        minValue = _minValue;
        maxValue = _maxValue;
        invertAxis = _invert;
    }
}
#endregion