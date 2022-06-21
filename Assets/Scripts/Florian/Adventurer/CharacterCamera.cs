using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    public class CharacterCamera : MonoBehaviour
    {
        private PlayerInput _inputs;

        [SerializeField] private Transform lookAt;
        [SerializeField] private Cinemachine.AxisState x_Axis;
        [SerializeField] private Cinemachine.AxisState y_Axis;

        private void Awake()
        {
            _inputs = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            RotateCamera();
        }

        private void RotateCamera()
        {
            x_Axis.Update(Time.fixedDeltaTime);
            y_Axis.Update(Time.fixedDeltaTime);
            x_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().x;
            y_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().y;

            lookAt.eulerAngles = new Vector3(y_Axis.Value, x_Axis.Value, 0f);
        }
    }
}
