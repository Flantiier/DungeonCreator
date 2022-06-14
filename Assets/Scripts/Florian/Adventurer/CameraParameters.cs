using UnityEngine;

[System.Serializable]
public struct CameraParameters
{
    [Tooltip("Indicates the name of the input scheme")]
    public string schemeName;

    [Header("Horizontal Axis")]
    [Tooltip("Invert value of the horizontal axis")]
    public bool invertXAxis;
    [Tooltip("X Sensitivity")]
    public float xSensitivity;
    [Tooltip("X Acceleration speed")]
    public float xAccelSpeed;
    [Tooltip("X Deceleration speed")]
    public float xDecelSpeed;

    [Space]

    [Header("Vertical Axis")]
    [Tooltip("Camera max speed value on the Y axis")]
    public float ySensitivity;
    [Tooltip("Y Acceleration speed")]
    public float yAccelSpeed;
    [Tooltip("Y Deceleration speed")]
    public float yDecelSpeed;
    [Tooltip("Ivert value of the vertical axis")]
    public bool invertYAxis;
}
