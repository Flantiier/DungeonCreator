using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    [RequireComponent(typeof(CharacterController))]
    public class AdventurerController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField, Tooltip("Referencing the player camera")]
        private Transform playerCam;

        private CharacterController _cc;
        private PlayerInput _inputs;

        //

        [Header("Motion Parameters")]
        [SerializeField, Tooltip("Walking speed value")]
        private float walkSpeed = 7f;

        [SerializeField, Tooltip("Running speed value")]
        private float runningSpeed = 10f;

        [SerializeField, Range(0.5f, 1f), Tooltip("Lerping value to smooth the current speed of the player")]
        private float minRunValue = 0.8f;
        
        [SerializeField, Range(0f, 1f), Tooltip("Lerping value to smooth the current speed of the player")]
        private float lerpMotion = 0.1f;

        private Vector3 _movement;
        private float _targetVel;
        private float _verticalVel;

        [Header("Player Rotation")]
        [SerializeField, Range(0f, 1f), Tooltip("Smoothing value for the player rotation")] 
        private float lerpRotation = 0.1f;

        private float _rotationVelRef;

        // 

        [Header("Gravity")]
        [SerializeField, Tooltip("Gravity applied on the player")] 
        private float gravityValue = 9.81f;

        [SerializeField, Tooltip("Maximum velocity on the Vertical Axis")]
        private float maxVerticalVelocity = 10f;

        [SerializeField, Tooltip("Maximum value for the InAir timer")]
        private float maxTimeInAir = 4f;

        [SerializeField, Tooltip("TimeReset when the player touch the ground")]
        private float timeInAirReset = 1f;

        private float _timeInAir = 1f;

        //

        [Header("Dodge")]
        [SerializeField, Tooltip("Dodging Curve => Takes the speed during the dodge based on a timer")]
        private AnimationCurve dodgingCurve;

        [SerializeField, Tooltip("Deadzone after the last Curve Key")]
        private float endTime = 0.5f;

        private float _dodgingTimer;
        private bool _isDodging;

        //

        public enum MotionStates
        {
            Standing,
            Walking,
            Running,
            Dodging
        }
        #endregion

        #region Properties
        public MotionStates State { get; set; }

        public Vector2 DirectionInputs { get; private set; }

        public float MoveSpeed { get; private set; }

        public bool CanMove { get; set; }

        public bool CanDodge { get; set; }

        public bool CanAttack { get; set; }

        public PlayerInput _PlayerInput => _inputs;
        #endregion

        #region Builts-In
        private void Awake()
        {
            InitMethod();
        }

        private void OnEnable()
        {
            _inputs.ActivateInput();
            _inputs.actions["Dodge"].started += Dodge;
            _inputs.actions["Motion"].performed += ctx => DirectionInputs = ctx.ReadValue<Vector2>();
            _inputs.actions["Motion"].canceled += ctx => DirectionInputs = ctx.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            _inputs.DeactivateInput();
            _inputs.actions["Dodge"].started -= Dodge;
            _inputs.actions["Motion"].performed -= ctx => DirectionInputs = ctx.ReadValue<Vector2>();
            _inputs.actions["Motion"].canceled -= ctx => DirectionInputs = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            StateMachine();

            ChoseMotion();

            DodgeTime();
        }
        #endregion

        #region Helpers
        private void InitMethod()
        {
            _cc = GetComponent<CharacterController>();
            _inputs = GetComponent<PlayerInput>();

            CanMove = true;
            CanDodge = true;
            _timeInAir = timeInAirReset;
        }

        private bool CheckState(MotionStates targetState)
        {
            if (State != targetState)
                return false;

            return true;
        }
        #endregion

        #region Motion Methods
        /// <summary>
        /// Switching between the Dodging Method and the Motion Method
        /// </summary>
        private void ChoseMotion()
        {
            if (_inputs == null)
                return;

            if (_isDodging)
                DodgeMovement();
            else
            {
                SpeedControl();
                Motion();
                Movement();
            }
        }

        /// <summary>
        /// Moving and Rotating the player based on Inputs and Camera Rotation
        /// </summary>
        private void Motion()
        {
            if (!_cc.isGrounded || !CanMove)
                return;

            Vector3 direction = new Vector3(DirectionInputs.x, 0f, DirectionInputs.y).normalized;

            //Player Rotation
            if (direction.magnitude >= 0.05f)
            {
                //Rotation Angle based on Player Inputs
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelRef, lerpRotation);
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //Setting movementVector
                _movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }

            _movement *= _targetVel * Time.deltaTime;
        }

        /// <summary>
        /// Final Movement
        /// </summary>
        private void Movement()
        {
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
                if(_timeInAir < maxTimeInAir)
                    //Increase the timeInAir timer
                    _timeInAir += Time.deltaTime;

                if(_verticalVel > -maxVerticalVelocity)
                    //Apply Gravity with time spent in air
                    _verticalVel -= gravityValue * Time.deltaTime * _timeInAir;
            }

            if (!_isDodging && !_cc.isGrounded)
                _movement = new Vector3(0f, _verticalVel, 0f);

            //Apply on the Movement Vector Vertical Axis 
            _movement.y = _verticalVel * Time.deltaTime;

            _cc.Move(_movement);
        }

        /// <summary>
        /// Interpolates the moving Speed
        /// </summary>
        private void SpeedControl()
        {
            //Standing Vel => 0
            if (CheckState(MotionStates.Standing))
                _targetVel = 0f;
            //Running Vel
            else if (CheckState(MotionStates.Running))
                _targetVel = runningSpeed;
            //Walking Vel
            else if (CheckState(MotionStates.Walking))
                _targetVel = walkSpeed * DirectionInputs.magnitude;

            //Interpolates the motionSpeed
            MoveSpeed = Mathf.Lerp(MoveSpeed, _targetVel, lerpMotion);

            if (MoveSpeed < 0.1f)
                MoveSpeed = 0f;
        }
        #endregion

        #region StateMachine
        /// <summary>
        /// Indicates the current Motion State
        /// </summary>
        private void StateMachine()
        {
            //Inputs not referenced
            if (_inputs == null)
                return;

            if (_isDodging)
                State = MotionStates.Dodging;
            //Standing => no Inputs
            else if (DirectionInputs == Vector2.zero)
                State = MotionStates.Standing;
            //Running => Shift
            else if (_inputs.actions["Running"].IsPressed() && DirectionInputs.magnitude >= minRunValue)
                State = MotionStates.Running;
            //Walking
            else
                State = MotionStates.Walking;
        }
        #endregion

        #region Dodge
        /// <summary>
        /// Dodging Method
        /// </summary>
        private void Dodge(InputAction.CallbackContext ctx)
        {
            //Ground check
            if (!_cc.isGrounded || !CanDodge)
                return;

            DodgeState(true);

            _movement = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }

        private void DodgeMovement()
        {
            if (!_cc.isGrounded)
                _movement.y += _verticalVel * Time.deltaTime;

            _cc.Move(_movement * dodgingCurve.Evaluate(_dodgingTimer) * Time.deltaTime);
        }

        private void DodgeState(bool state)
        {
            _isDodging = state;
            CanMove = !state;
            CanDodge = !state;
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
            if (_dodgingTimer <= dodgingCurve.keys[dodgingCurve.keys.Length - 1].time + endTime)
                _dodgingTimer += Time.deltaTime;
            //Reset dodge
            else
            {
                //Reset to Motion
                DodgeState(false);
                _dodgingTimer = 0f;
            }
        }
        #endregion
    }
}
