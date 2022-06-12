using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    [RequireComponent(typeof(CharacterController))]
    public class AdventurerController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [Tooltip("Referencing the player camera")]
        [SerializeField] private Transform playerCam;
        private CharacterController _cc;
        private PlayerInputs _inputs;

        [Header("Motion Parameters")]
        [Tooltip("Walking speed value")]
        [SerializeField] private float walkSpeed = 7f;
        [Tooltip("Running speed value")]
        [SerializeField] private float runningSpeed = 10f;
        [Range(0f, 1f)]
        [Tooltip("Lerping value to smooth the current speed of the player")]
        [SerializeField] private float lerpMotion = 0.1f;

        /// <summary>
        /// Inputs Vector
        /// </summary>
        private Vector2 _directionInputs;
        /// <summary>
        /// Current Motion Speed
        /// </summary>
        public float MoveSpeed { get; private set; }
        /// <summary>
        /// Target Motion Speed
        /// </summary>
        private float _targetSpeed;

        [Header("Mesh Rotation")]
        [Range(0f, 1f)]
        [Tooltip("Smoothing value for the player rotation")]
        [SerializeField] private float lerpRotation = 0.1f;
        private float _rotationVelRef;

        public enum MotionStates
        {
            Standing,
            Walking,
            Running,
        }
        /// <summary>
        /// Current State of the player
        /// </summary>
        public MotionStates State { get; private set; }
        #endregion

        #region Builts-In
        private void Awake()
        {
            PlayerInputs playerInputs = new PlayerInputs();
            _inputs = playerInputs;

            _cc = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _inputs.Enable();
        }

        private void OnDisable()
        {
            _inputs.Disable();
        }

        private void Update()
        {
            StateMachine();
            Motion();
        }
        #endregion

        #region Motion Methods
        /// <summary>
        /// Moving and Rotatinf the player based on Inputs and Camera Rotation
        /// </summary>
        private void Motion()
        {
            //Check if there's no inputs
            if (_inputs == null)
                return;

            //Movement Vector
            Vector3 direction = new Vector3(_directionInputs.x, 0f, _directionInputs.y).normalized;
            Vector3 movement = new Vector3();

            //Player Rotation
            if (direction.magnitude >= 0.05f)
            {
                //Rotation Angle based on Player Inputs
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelRef, lerpRotation);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //Setting movementVector
                movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }

            //Get player speed
            SpeedControl();

            //Moving Player
            _cc.Move(movement * MoveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Changing motion speed according to Inputs
        /// </summary>
        private void SpeedControl()
        {
            //Standing Vel => 0
            if (State == MotionStates.Standing)
                _targetSpeed = 0f;
            //Running Vel
            else if (State == MotionStates.Running)
                _targetSpeed = runningSpeed;
            //Walking Vel
            else if (State == MotionStates.Walking)
                _targetSpeed = walkSpeed;

            //Interpolates the motionSpeed
            MoveSpeed = Mathf.Lerp(MoveSpeed, _targetSpeed, lerpMotion);
        }

        /// <summary>
        /// Indicates the current Motion State
        /// </summary>
        private void StateMachine()
        {
            //Inputs not referenced
            if (_inputs == null)
                return;

            //Get Inputs
            _directionInputs = _inputs.Controls.Motion.ReadValue<Vector2>();

            //Standing => no Inputs
            if (_inputs.Controls.Motion.ReadValue<Vector2>() == Vector2.zero)
                State = MotionStates.Standing;
            //Running => Shift
            else if (_inputs.Controls.Run.IsPressed())
                State = MotionStates.Running;
            //Walking
            else
                State = MotionStates.Walking;
        }
        #endregion
    }
}
