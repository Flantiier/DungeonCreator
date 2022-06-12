using UnityEngine;
using Cinemachine;

namespace Adventurer
{
    public class AdventurerCamera : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [Tooltip("Referencing here the freelook camera of the player")]
        [SerializeField] private CinemachineFreeLook freelookCam;

        [Header("Camera Parameters")]
        [Header("Horizontal Axis")]
        [Tooltip("X Sensitivity")]
        [SerializeField] private float xSensitivity = 10f;
        [Tooltip("X Acceleration speed")]
        [SerializeField] private float xAccelSpeed = 0.2f;
        [Tooltip("X Deceleration speed")]
        [SerializeField] private float xDecelSpeed = 0.1f;
        [Tooltip("Invert value of the horizontal axis")]
        [SerializeField] private bool invertXAxis = false;

        [Space]

        [Header("Vertical Axis")]
        [Tooltip("Camera max speed value on the Y axis")]
        [SerializeField] private float ySensitivity = 10f;
        [Tooltip("Y Acceleration speed")]
        [SerializeField] private float yAccelSpeed = 0.1f;
        [Tooltip("Y Deceleration speed")]
        [SerializeField] private float yDecelSpeed = 0.1f;
        [Tooltip("Ivert value of the vertical axis")]
        [SerializeField] private bool invertYAxis = true;
        #endregion

        private void Awake()
        {
            SetCameraParameters();
        }

        private void OnValidate()
        {
            SetCameraParameters();
        }

        [ContextMenu("Reset Camera")]
        private void SetCameraParameters()
        {
            if (freelookCam == null)
                return;

            //X Axis
            freelookCam.m_XAxis.m_MaxSpeed = xSensitivity;
            freelookCam.m_XAxis.m_InvertInput = invertXAxis;
            freelookCam.m_XAxis.m_AccelTime = xAccelSpeed;
            freelookCam.m_XAxis.m_DecelTime = xDecelSpeed;

            //Y Axis
            freelookCam.m_YAxis.m_MaxSpeed = ySensitivity;
            freelookCam.m_YAxis.m_InvertInput = invertYAxis;
            freelookCam.m_YAxis.m_AccelTime = yAccelSpeed;
            freelookCam.m_YAxis.m_DecelTime = yDecelSpeed;
        }
    }
}
