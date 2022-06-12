<<<<<<< HEAD
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
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
<<<<<<< HEAD
        public Vector2 DirectionInputs { get; private set; }
        /// <summary>
        /// Direction Vector
        /// </summary>
        private Vector3 _movement;
=======
        private Vector2 _directionInputs;
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        /// <summary>
        /// Current Motion Speed
        /// </summary>
        public float MoveSpeed { get; private set; }
        /// <summary>
        /// Target Motion Speed
        /// </summary>
<<<<<<< HEAD
        private float _targetVel;
        /// <summary>
        /// Vertical Velocity of the player
        /// </summary>
        private float _verticalVel;
=======
        private float _targetSpeed;
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d

        [Header("Mesh Rotation")]
        [Range(0f, 1f)]
        [Tooltip("Smoothing value for the player rotation")]
        [SerializeField] private float lerpRotation = 0.1f;
        private float _rotationVelRef;

<<<<<<< HEAD
        [Header("Gravity")]
        [Tooltip("Gravity applied on the player")]
        [SerializeField] private float gravityValue = 9.81f;
        [Tooltip("TimeReset when the player touch the ground")]
        [SerializeField] private float timeInAirReset = 1f;
        private float _timeInAir = 1f;

        [Header("Dodge")]
        [Tooltip("Dodging Parameters")]
        [SerializeField] private AnimationCurve dodgingCurve;
        private bool _isDodging;
        private float _dodgingTimer;

=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        public enum MotionStates
        {
            Standing,
            Walking,
            Running,
<<<<<<< HEAD
            Dodging
=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
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
<<<<<<< HEAD
            _inputs.Controls.Dodge.started += Dodge;
=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        }

        private void OnDisable()
        {
            _inputs.Disable();
<<<<<<< HEAD
            _inputs.Controls.Dodge.started -= Dodge;
=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        }

        private void Update()
        {
            StateMachine();
<<<<<<< HEAD
            SpeedControl();

            ChoseMotion();

            DodgeTime();
=======
            Motion();
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        }
        #endregion

        #region Motion Methods
        /// <summary>
<<<<<<< HEAD
        /// Switching between the Dodging Method and the Motion Method
        /// </summary>
        private void ChoseMotion()
        {
            if (!_isDodging)
                Motion();
            else
                DodgeMovement();
        }

        /// <summary>
=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        /// Moving and Rotatinf the player based on Inputs and Camera Rotation
        /// </summary>
        private void Motion()
        {
            //Check if there's no inputs
            if (_inputs == null)
                return;

            //Movement Vector
<<<<<<< HEAD
            Vector3 direction = new Vector3(DirectionInputs.x, 0f, DirectionInputs.y).normalized;
=======
            Vector3 direction = new Vector3(_directionInputs.x, 0f, _directionInputs.y).normalized;
            Vector3 movement = new Vector3();
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d

            //Player Rotation
            if (direction.magnitude >= 0.05f)
            {
                //Rotation Angle based on Player Inputs
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelRef, lerpRotation);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //Setting movementVector
<<<<<<< HEAD
                _movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }

            _movement *= _targetVel * Time.deltaTime;
            //Apply Gravity
            _movement.y = ApplyGravity();

            _cc.Move(_movement);
=======
                movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }

            //Get player speed
            SpeedControl();

            //Moving Player
            _cc.Move(movement * MoveSpeed * Time.deltaTime);
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        }

        /// <summary>
        /// Changing motion speed according to Inputs
        /// </summary>
        private void SpeedControl()
        {
<<<<<<< HEAD
            if (State == MotionStates.Dodging)
                _targetVel = dodgingCurve.Evaluate(_dodgingTimer);
            //Standing Vel => 0
            else if (State == MotionStates.Standing)
                _targetVel = 0f;
            //Running Vel
            else if (State == MotionStates.Running)
                _targetVel = runningSpeed;
            //Walking Vel
            else if (State == MotionStates.Walking)
                _targetVel = walkSpeed;

            //Interpolates the motionSpeed
            MoveSpeed = Mathf.Lerp(MoveSpeed, _targetVel, lerpMotion);
=======
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
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
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
<<<<<<< HEAD
            DirectionInputs = _inputs.Controls.Motion.ReadValue<Vector2>();

            if (_isDodging)
                State = MotionStates.Dodging;
            //Standing => no Inputs
            else if (_inputs.Controls.Motion.ReadValue<Vector2>() == Vector2.zero)
=======
            _directionInputs = _inputs.Controls.Motion.ReadValue<Vector2>();

            //Standing => no Inputs
            if (_inputs.Controls.Motion.ReadValue<Vector2>() == Vector2.zero)
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
                State = MotionStates.Standing;
            //Running => Shift
            else if (_inputs.Controls.Run.IsPressed())
                State = MotionStates.Running;
            //Walking
            else
                State = MotionStates.Walking;
        }
<<<<<<< HEAD

        /// <summary>
        /// Applying Gravity on the player
        /// </summary>
        private float ApplyGravity()
        {
            //On the ground
            if (_cc.isGrounded)
            {
                //Apply a tiny gravity
                _verticalVel = -gravityValue * 0.25f;

                //Reset timeInAir timer
                if (_timeInAir != timeInAirReset)
                    _timeInAir = timeInAirReset;
            }
            //In Air
            else
            {
                //Increase the timeInAir timer
                _timeInAir += Time.deltaTime;

                //Apply Gravity with time spent in air
                _verticalVel -= gravityValue * Time.deltaTime * _timeInAir;
            }

            //Apply on the Movement Vector Vertical Axis 
            return _verticalVel * Time.deltaTime;
        }

        /// <summary>
        /// Dodging Movement Method
        /// </summary>
        private void DodgeMovement()
        {
            //Check if there's no inputs
            if (_inputs == null)
                return;

            //Moving player
            _cc.Move(_movement * _targetVel * Time.deltaTime);
        }

        /// <summary>
        /// Control the duration of the dodge
        /// </summary>
        private void DodgeTime()
        {
            //If he's not dodging
            if (!_isDodging)
                return;

            //The timer is less or equal the ending time
            if (_dodgingTimer <= dodgingCurve.keys[dodgingCurve.keys.Length - 1].time)
                _dodgingTimer += Time.deltaTime;
            //Reset dodge
            else
            {
                _isDodging = false;
                _dodgingTimer = 0f;
            }
        }

        /// <summary>
        /// Dodging Method
        /// </summary>
        private void Dodge(InputAction.CallbackContext ctx)
        {
            //Ground check
            if (_cc.isGrounded)
            {
                _isDodging = true;
                _movement = Vector3.ProjectOnPlane(playerCam.forward, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f);

                if (TryGetComponent(out AdventurerAnimator animator))
                    animator.SetDodgeAnimation();
            }
        }
=======
>>>>>>> 53aa6ee1060ebc88bfba714c58e751aca592e44d
        #endregion
    }
}
