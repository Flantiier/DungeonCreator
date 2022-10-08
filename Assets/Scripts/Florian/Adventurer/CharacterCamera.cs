using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Adventurer
{
    public class CharacterCamera : MonoBehaviour
    {
        #region Camera Variables
        //PlayerInputs comp
        private PlayerInput _inputs;
        //PhotonView comp
        private PhotonView _view;

        [Header("Camera Variables")]
        [SerializeField, Tooltip("Look at target of the camera")]
        private Transform lookAt;

        [SerializeField, Tooltip("X Axis variables")]
        private Cinemachine.AxisState x_Axis;

        [SerializeField, Tooltip("Y Axis variables")]
        private Cinemachine.AxisState y_Axis;
        #endregion

        #region Builts-In
        private void Awake()
        {
            //Not local
            /*_view = GetComponent<PhotonView>();
            if (!_view.IsMine)
                return;*/

            //Local
            //Get Inputs 
            _inputs = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            //Local
            /*if (!_view.IsMine)
                return;*/

            //Not local
            //Rotate camera
            RotateCamera();
        }
        #endregion

        #region Rotating Camera Methods
        private void RotateCamera()
        {
            //Update values on X and Y Axis 
            x_Axis.Update(Time.fixedDeltaTime);
            y_Axis.Update(Time.fixedDeltaTime);
            //Reading MouseInput values
            x_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().x;
            y_Axis.m_InputAxisValue = _inputs.actions["Mouse"].ReadValue<Vector2>().y;

            //Setting lookAt rotation
            lookAt.eulerAngles = new Vector3(y_Axis.Value, x_Axis.Value, 0f);
        }
        #endregion
    }
}
