using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Adventurer 
{
	public class AdventurerAiming : MonoBehaviour
	{
        private PlayerInput _inputs;

        [SerializeField] private Transform lookAt;
		[SerializeField] private AxisState x_Axis;
		[SerializeField] private AxisState y_Axis;

        [SerializeField] private CinemachineVirtualCamera tpsCam;
        [SerializeField] private CinemachineVirtualCamera aimCam;

        public bool CanAim { get; private set; }
        public bool IsAiming { get; private set; }

        private void Awake()
        {
            _inputs = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _inputs.actions["Aim"].started += StartAim;
            _inputs.actions["Aim"].canceled += CancelAim;
        }

        private void OnDisable()
        {
            _inputs.actions["Aim"].started -= StartAim;
            _inputs.actions["Aim"].canceled -= CancelAim;
        }

        private void FixedUpdate()
        {
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            x_Axis.Update(Time.fixedDeltaTime);
            y_Axis.Update(Time.fixedDeltaTime);
            x_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().x;
            y_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().y;

            lookAt.eulerAngles = new Vector3(y_Axis.Value, x_Axis.Value, 0f);
        }

        private void StartAim(InputAction.CallbackContext ctx)
        {
            aimCam.Priority += tpsCam.Priority;
        }

        private void CancelAim(InputAction.CallbackContext ctx)
        {
            aimCam.Priority -= tpsCam.Priority;
        }
    }
}
