using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    public class AdventurerCamera : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField, Tooltip("Referencing here the freelook camera of the player")]
        private CinemachineFreeLook freelookCam;

        [SerializeField, Tooltip("Parameters whilst using Mouse And Keyboard")]
        private CameraParameters mouseParameters;

        [SerializeField, Tooltip("Parameters whilst using a Gamepad")]
        private CameraParameters gamePadParameters;
        private PlayerInput _inputs;
        #endregion

        private void Awake()
        {
            _inputs = GetComponent<PlayerInput>();

            SetCameraParameters(_inputs);
            _inputs.onDeviceLost += SetCameraParameters;
        }

        [ContextMenu("Reset Camera")]
        private void SetCameraParameters(PlayerInput input)
        {
            if (freelookCam == null)
                return;

            if(_inputs.currentControlScheme == gamePadParameters.schemeName)
                AffectParameters(gamePadParameters);
            else
                AffectParameters(mouseParameters);
        }

        private void AffectParameters(CameraParameters parameters)
        {
            freelookCam.m_XAxis.m_MaxSpeed = parameters.xSensitivity;
            freelookCam.m_XAxis.m_AccelTime = parameters.xAccelSpeed;
            freelookCam.m_XAxis.m_DecelTime = parameters.xDecelSpeed;
            freelookCam.m_XAxis.m_InvertInput = parameters.invertXAxis;

            //Y Axis
            freelookCam.m_YAxis.m_MaxSpeed = parameters.ySensitivity;
            freelookCam.m_YAxis.m_AccelTime = parameters.yAccelSpeed;
            freelookCam.m_YAxis.m_DecelTime = parameters.yDecelSpeed;
            freelookCam.m_YAxis.m_InvertInput = parameters.invertYAxis;
        }
    }
}
