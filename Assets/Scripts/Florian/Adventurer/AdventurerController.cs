using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    [RequireComponent(typeof(CharacterController))]
    public class AdventurerController : MonoBehaviour
    {
        #region References
        [Header("References")]
        [SerializeField, Tooltip("Referencing the player camera")]
        private Transform playerCam;

        [SerializeField, Tooltip("Referencing the script with animEvents on the mesh")]
        private Transform playerMesh;

        private CharacterController _cc;
        private AdventurerInputManager _inputs;
        private AdventurerAnimEvents _animEvents;
        #endregion

        #region Motion Variables
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
        #endregion

        #region Rotation variables
        [Header("Player Rotation")]
        [SerializeField, Range(0f, 1f), Tooltip("Smoothing value for the player rotation")]
        private float lerpRotation = 0.1f;

        private float _rotationVelRef;
        private float _aimVelRef;
        #endregion

        #region Gravity Variables
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
        private float _verticalVel;
        #endregion

        #region Dodge Variables
        [Header("Dodge")]
        [SerializeField, Tooltip("Dodging Curve => Takes the speed during the dodge based on a timer")]
        private AnimationCurve dodgingCurve;

        [SerializeField, Tooltip("Deadzone after the last Curve Key")]
        private float endTime = 0.5f;

        private float _dodgingTimer;
        #endregion

        #region Properties
        public AdventurerInputManager Input => _inputs;

        public float MoveSpeed { get; private set; }

        public bool IsGrounded { get; private set; }

        public bool CanMove { get; private set; }

        public bool CanDodge { get; private set; }

        public bool IsDodging { get; private set; }

        public bool CanAttack { get; private set; }

        public bool CanAim { get; private set; }

        #endregion

        #region Builts-In
        private void Awake()
        {
            InitMethod();
        }

        private void OnEnable()
        {
            SubscribingToAnimEvent();
        }

        private void OnDisable()
        {
            UnsubscribingToAnimEvent();
        }

        private void Update()
        {
            GroundCheck();

            //Motion
            CalculateMotion();

            //Dodge
            StartDodge();
            DodgeTimer();

            MovePlayer();
        }
        #endregion

        private void InitMethod()
        {
            //Get scripts
            _cc = GetComponent<CharacterController>();
            _inputs = GetComponent<AdventurerInputManager>();
            _animEvents = playerMesh.GetComponent<AdventurerAnimEvents>();

            EnableMovement();
            EnableDodge();
            EnableAttack();
            _timeInAir = timeInAirReset;
        }

        private void GroundCheck()
        {
            if (_cc.isGrounded)
                IsGrounded = true;
            else
                IsGrounded = false;
        }

        private void EnableMovement() { CanMove = true;}
        private void DisableMovement() { CanMove = false;}
        private void EnableDodge() { CanDodge = true;}
        private void DisableDodge() { CanDodge = false;}
        private void EnableAttack() { CanAttack = true;}
        private void DisableAttack() { CanAttack = false;}

        #region Motion Methods
        /// <summary>
        /// Moving and Rotating the player based on Inputs and Camera Rotation
        /// </summary>
        private void CalculateMotion()
        {
            if (IsDodging)
                return;

            CalculateMotionSpeed();

            if (!IsGrounded)
            {
                if (_movement.x > 0.05f)
                    _movement.x = Mathf.Lerp(_movement.x, 0f, 0.05f);

                if (_movement.z > 0.05f)
                    _movement.z = Mathf.Lerp(_movement.z, 0f, 0.05f);

                if (_timeInAir < maxTimeInAir)
                    //Increase the timeInAir timer
                    _timeInAir += Time.deltaTime;

                if (_verticalVel > -maxVerticalVelocity)
                    //Apply Gravity with time spent in air
                    _verticalVel -= gravityValue * Time.deltaTime * _timeInAir;
            }
            else
            {
                if (CanMove)
                    if (!_inputs.IsHoldingWeapon)
                        BasicMotion();
                    else
                        AimingMethod();
                else
                    _movement = Vector3.zero;

                //Apply a tiny gravity
                _verticalVel = -gravityValue * 0.25f;

                //Reset timeInAir timer
                if (_timeInAir != timeInAirReset)
                    _timeInAir = timeInAirReset;
            }
        }

        private void BasicMotion()
        {
            //Player Rotation
            if (_inputs.DirectionInputs.magnitude >= 0.05f)
            {
                //Rotation Angle based on Player Inputs
                float angle = Mathf.Atan2(_inputs.DirectionInputs.x, _inputs.DirectionInputs.y) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, angle, ref _rotationVelRef, lerpRotation);
                playerMesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //Setting movementVector
                _movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            }

            _movement *= MoveSpeed * Time.deltaTime;
        }

        private void AimingMethod()
        {
            float targetRotation = playerCam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, targetRotation, ref _aimVelRef, lerpRotation);
            playerMesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            _movement = playerCam.forward * _inputs.DirectionInputs.y + playerCam.right * _inputs.DirectionInputs.x;

            _movement *= MoveSpeed * Time.deltaTime;
        }

        private void MovePlayer()
        {
            if (IsDodging)
            {
                _movement.y += _verticalVel * Time.deltaTime * 0.25f;
                _cc.Move(_movement * dodgingCurve.Evaluate(_dodgingTimer) * Time.deltaTime);
            }
            else
            {
                _movement.y = _verticalVel * Time.deltaTime;
                _cc.Move(_movement);
            }
        }

        private void CalculateMotionSpeed()
        {
            //Standing Vel => 0
            if (!IsGrounded || _inputs.DirectionInputs == Vector2.zero)
                _targetVel = 0f;
            //Running Vel
            else if (_inputs.isRunning && _inputs.DirectionInputs.magnitude >= minRunValue)
                _targetVel = runningSpeed;
            //Walking Vel
            else
                _targetVel = walkSpeed * _inputs.DirectionInputs.magnitude;

            //Interpolates the motionSpeed
            MoveSpeed = Mathf.Lerp(MoveSpeed, _targetVel, lerpMotion);

            if (MoveSpeed < 0.1f)
                MoveSpeed = 0f;
        }
        #endregion

        #region Dodge Methods
        /// <summary>
        /// Dodging Method
        /// </summary>
        private void StartDodge()
        {
            //Ground check
            if (_inputs.isDodging && _cc.isGrounded && !IsDodging && CanDodge)
            {
                IsDodging = true;

                //playerMesh.rotation = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f);
                _movement = Vector3.ProjectOnPlane(playerMesh.forward, Vector3.up);
            }
        }

        /// <summary>
        /// Control the duration of the dodge
        /// </summary>
        private void DodgeTimer()
        {
            //If he's not dodging
            if (!IsDodging)
                return;

            //The timer is less or equal the ending time
            if (_dodgingTimer <= dodgingCurve.keys[dodgingCurve.keys.Length - 1].time + endTime)
                _dodgingTimer += Time.deltaTime;
            //Reset dodge
            else
            {
                ResetDodge();
                _movement = Vector3.zero;
            }
        }

        private void ResetDodge()
        {
            //Reset to Motion
            IsDodging = false;
            _dodgingTimer = 0f;
        }
        #endregion

        #region Events
        private void SubscribingToAnimEvent()
        {
            //Start Attack
            _animEvents.onStartAttack += DisableMovement;
            _animEvents.onStartAttack += DisableDodge;
            _animEvents.onStartAttack += ResetDodge;

            //Middle Attack
            _animEvents.onMiddleAttack += EnableDodge;

            //End Attack
            _animEvents.onEndAttack += EnableMovement;

            //Start Dodge
            _animEvents.onStartDodge += DisableMovement;
            _animEvents.onStartDodge += DisableDodge;
            _animEvents.onStartDodge += DisableAttack;

            //Middle Dodge
            _animEvents.onMiddleDodge += EnableAttack;

            //EndDodge
            _animEvents.onEndDodge += EnableMovement;
            _animEvents.onEndDodge += EnableDodge;

            //OnFall
            _animEvents.OnFall += DisableMovement;
            _animEvents.OnFall += DisableDodge;
            _animEvents.OnFall += DisableAttack;

            //OnLand
            _animEvents.OnLand += EnableMovement;
            _animEvents.OnLand += EnableDodge;
            _animEvents.OnLand += EnableAttack;
        }
        private void UnsubscribingToAnimEvent()
        {
            //Start Attack
            _animEvents.onStartAttack -= DisableMovement;
            _animEvents.onStartAttack -= DisableDodge;
            _animEvents.onStartAttack -= ResetDodge;

            //Middle Attack
            _animEvents.onMiddleAttack -= EnableDodge;

            //End Attack
            _animEvents.onEndAttack -= EnableMovement;

            //Start Dodge
            _animEvents.onStartDodge -= DisableMovement;
            _animEvents.onStartDodge -= DisableDodge;
            _animEvents.onStartDodge -= DisableAttack;

            //Middle Dodge
            _animEvents.onMiddleDodge -= EnableAttack;

            //EndDodge
            _animEvents.onEndDodge -= EnableMovement;
            _animEvents.onEndDodge -= EnableDodge;

            //OnFall
            _animEvents.OnFall -= DisableMovement;
            _animEvents.OnFall -= DisableDodge;
            _animEvents.OnFall -= DisableAttack;

            //OnLand-
            _animEvents.OnLand -= EnableMovement;
            _animEvents.OnLand -= EnableDodge;
            _animEvents.OnLand -= EnableAttack;
        }
        #endregion

    }
}
