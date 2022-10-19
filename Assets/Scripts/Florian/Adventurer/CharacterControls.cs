using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using UnityEngine.Playables;

namespace Adventurer
{
    public class CharacterControls : MonoBehaviour
    {
        #region References
        [Header("References")]
        [SerializeField, Tooltip("Transform of the playerCamera")]
        private Transform playerCam;
        [SerializeField, Tooltip("Transform of the playerMesh")]
        private Transform playerMesh;
        [SerializeField, Tooltip("Orientation Transform")]
        private Transform orientation;

        //Photon view component
        private PhotonView _view;
        //Player inputs component
        private PlayerInput _inputs;
        //Rigidbody of the player
        private Rigidbody _rb;
        //Animator component on the mesh
        private Animator _animator;
        /// <summary>
        /// Character Fighting component on the player
        /// </summary>
        private CharacterFighting _fighting;
        #endregion

        #region Motion Variables
        [Header("Motion Variables")]
        [SerializeField, Tooltip("Speed while the player is walking")]
        private float walkSpeed = 5f;

        [SerializeField, Tooltip("Speed while the player is aiming")]
        private float aimSpeed = 3f;

        [SerializeField, Tooltip("Speed while the player is running")]
        private float runSpeed = 7f;

        [SerializeField, Range(0f, 1f), Tooltip("Inputs minimum value to run")]
        private float minRunValue = 0.75f;

        [SerializeField, Range(0f, 1f), Tooltip("Lerping value to calculates the motion speed")]
        private float lerpMotion = 0.1f;

        /// <summary>
        /// Movement vector
        /// </summary>
        private Vector3 _movement;
        //Smooth input vector
        private Vector3 _smoothInputs;
        /// <summary>
        /// Movement multiplier applied on the motion speed
        /// </summary>
        private float _movementMultiplier = 1f;
        /// <summary>
        /// Current target velocity
        /// </summary>
        private float _targetVel;
        #endregion

        #region Rotation Variables
        [Header("Rotation Variables")]
        [SerializeField, Range(0f, 1f), Tooltip("Lerping Value to calculates the rotation")]
        private float lerpRotation = 0.1f;

        //Ref to turn the player
        private float _turnVel;
        #endregion

        #region Ground Variables
        [Header("Ground Check Variables")]
        [SerializeField, Tooltip("Transform position of the groundCheck")]
        private Transform checkPosition;

        [SerializeField, Range(0f, 1f), Tooltip("Radius to check if the player is on ground")]
        private float checkRadius = 0.1f;

        [SerializeField, Tooltip("Layer to detect the ground")]
        private LayerMask groundMask;
        #endregion

        #region Slope Variables
        [Header("Slope Check Variables")]
        [SerializeField, Tooltip("Slope detect Position")]
        private Transform slopeCheckPosition;

        [SerializeField, Range(0f, 1f), Tooltip("Max slope detect distance")]
        private float slopeDistance = 0.5f;

        [SerializeField, Range(0f, 1f), Tooltip("Slope check radius")]
        private float slopeRadius = 0.1f;

        [SerializeField, Tooltip("Layer to detect slopes")] private LayerMask slopeMask;
        private RaycastHit _slopeHit;
        #endregion

        #region Gravity Variables
        [Header("Gravity Variables")]
        [SerializeField, Tooltip("Extra gravity value applied on player")]
        private float extraGravity = 3f;

        [SerializeField, Tooltip("Maximum velocity on the World Y Axis")]
        private float maxVerticalVel = 30f;

        [SerializeField, Tooltip("Minimum value to the timeInAir timer")]
        private float minTimeInAir = 1f;

        [SerializeField, Tooltip("Maximum time in air")] private float maxTimeInAir = 3f;
        private float _timeInAir;
        #endregion

        #region Dodge Variables
        [Header("Dodge Variables")]
        [SerializeField, Tooltip("Dodging force add towards the player")] private float dodgeForce = 3f;
        #endregion

        #region Animations Variables
        [Header("Animations Variables")]
        [SerializeField, Range(0f, 1f), Tooltip("Lerping value to lerp inputs vector")]
        private float lerpAnimInputs = 0.05f;

        [SerializeField, Range(0f, 1f), Tooltip("Lerping value to lerp animation motion")] private float lerpAnimMotion = 0.05f;
        private float _animMotionSpeed;
        #endregion

        #region Properties
        /// <summary>
        /// Direction inputs vector
        /// </summary>
        public Vector2 InputsVector { get; private set; }
        /// <summary>
        /// Current Moving speed of the player
        /// </summary>
        public float MoveSpeed { get; private set; }
        /// <summary>
        /// Current Movement Multiplier applied on Motion Speed
        /// </summary>
        public float MovementMultiplier
        {
            get { return _movementMultiplier; }
            set { _movementMultiplier = value; }
        }
        /// <summary>
        /// Indicates if the player is grounded or not
        /// </summary>
        public bool IsGrounded { get; private set; }
        /// <summary>
        /// Indicates if the player is on a slope
        /// </summary>
        public bool IsOnSlope { get; private set; }
        /// <summary>
        /// Indicates if the player can move
        /// </summary>
        public bool CanMove { get; private set; }
        /// <summary>
        /// Indicates if the player can run
        /// </summary>
        public bool CanRun { get; private set; }
        /// <summary>
        /// Indicates if the player is currenlty running (read inputs)
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// Indicates of the player can dodge
        /// </summary>
        public bool CanDodge { get; private set; }
        /// <summary>
        /// indicates if the player is dodging (read inputs)
        /// </summary>
        public bool IsDodging { get; private set; }
        #endregion

        #region Builts-In
        private void Awake()
        {
            //Get the viewComponent
            _view = GetComponent<PhotonView>();
            //If this player is not local, don't execute
            if (!_view.IsMine)
                return;

            //Local player
            //Get inputs
            _inputs = GetComponent<PlayerInput>();
            //Get rb
            _rb = GetComponent<Rigidbody>();
            //Get Animator
            _animator = playerMesh.GetComponent<Animator>();
            //Get Fighting Comp
            _fighting = GetComponent<CharacterFighting>();

            //Player can move
            EnableMovement();
            //Player can run
            EnableRun();
            //Player can dodge
            EnableDodge();
        }

        private void OnEnable()
        {
            //Not local player
            if (!_view.IsMine)
                return;

            //Local player
            //Activates inputs
            _inputs.ActivateInput();
            //Subscribing to inputs events
            SubscribeToInputs();
        }

        private void OnDisable()
        {
            //Not local
            if (!_view.IsMine)
                return;

            //Local player
            //Deactivate Inputs
            _inputs.DeactivateInput();
            //Unsubscribe to inputs
            UnsubscribeToInputs();
        }

        private void Update()
        {
            //Not local player
            if (!_view.IsMine)
                return;

            //Local
            //Set player orientation
            SetOrientation();
            //Checking ground and slopes
            GroundCheck();
            SlopeCheck();

            //Add extra gravity
            AdditionnalGravity();
            //InAirTiming (Not grounded/Not onSlopes)
            InAirTime();
            //Moving player
            HandleMotion();
            //Moving player (Dodging)
            DodgeMovement(playerMesh.forward * dodgeForce);

            //Updating animations
            UpdateAnimations();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(checkPosition.position, checkRadius);
            Gizmos.DrawSphere(slopeCheckPosition.position, slopeRadius);
        }
        #endregion

        #region Methods

        #region Motion Methods
        /// <summary>
        /// Calculating player motion
        /// </summary>
        private void HandleMotion()
        {
            //Can't move
            if (!IsGrounded || !CanMove)
                return;

            //Get Motion speed
            MotionSpeed();

            //Calculate direction Vector
            _movement = orientation.forward * InputsVector.y + orientation.right * InputsVector.x;

            //Checking if the player is aiming
            //Looking in the same direction as the camera
            if (_fighting.CanAim && _fighting.IsAiming)
                AimRotate();
            //Free tps view
            else
                StandardRotate();

            //Calculating on slope movement
            //Project the direction vector on the normal of the slope 
            if (IsOnSlope)
                _rb.velocity = Vector3.ProjectOnPlane(_movement, _slopeHit.normal) * MoveSpeed;
            else
                _rb.velocity = _movement * MoveSpeed;
        }

        /// <summary>
        /// Player looking in the same direction as the camera
        /// </summary>
        private void AimRotate()
        {
            playerMesh.rotation = Quaternion.Euler(0f, orientation.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Free movement, free rotation, free life
        /// </summary>
        private void StandardRotate()
        {
            if (InputsVector.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(InputsVector.x, InputsVector.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, angle, ref _turnVel, lerpRotation);
                playerMesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        /// <summary>
        /// Calculate the motion speed
        /// </summary>
        private void MotionSpeed()
        {
            //Check all differents speeds
            if (IsIdled())
                _targetVel = 0f;
            else if (_fighting.CanAim && _fighting.IsAiming)
                _targetVel = aimSpeed;
            else if (CanRun && IsRunning && InputsVector.magnitude >= minRunValue)
                _targetVel = runSpeed;
            else
                _targetVel = walkSpeed;

            //Apply the movement multiplier on the speed
            _targetVel *= _movementMultiplier;

            //Lerping the currentSpeed Value with targetSpeed Value
            if (!Mathf.Approximately(MoveSpeed, _targetVel))
                MoveSpeed = Mathf.Lerp(MoveSpeed, _targetVel, lerpMotion);
            else
                MoveSpeed = _targetVel;

            if (MoveSpeed < 0.1f)
                MoveSpeed = 0f;
        }

        /// <summary>
        /// Impulsing the player in the direction
        /// </summary>
        /// <param name="direction">Impulse direction</param>
        public void ImpulsePlayer(Vector3 direction)
        {
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);

            if (IsOnSlope)
                _rb.AddForce(Vector3.ProjectOnPlane(direction, _slopeHit.normal), ForceMode.VelocityChange);
            else
                _rb.AddForce(direction, ForceMode.VelocityChange);
        }
        #endregion

        #region Dodge Methods
        /// <summary>
        /// Trigger Dodge Method
        /// </summary>
        private void Dodge(InputAction.CallbackContext ctx)
        {
            //Can't Dodge
            if (!CanDodge || !IsGrounded || IsDodging)
                return;

            DodgeAnimationRPC();
        }

        [PunRPC]
        private void DodgeAnimationRPC()
        {
            //Can Dodge
            _animator.SetTrigger("Dodge");
        }

        /// <summary>
        /// Dodging Movement Method
        /// </summary>
        /// <param name="direction">Direction of the dodge</param>
        private void DodgeMovement(Vector3 direction)
        {
            //Not grounded
            if (!IsDodging || !IsGrounded)
                return;

            //Apply on slopes
            if (IsOnSlope)
                _rb.velocity = Vector3.ProjectOnPlane(direction, _slopeHit.normal);
            else
                _rb.velocity = direction;
        }
        #endregion

        #region Gravity Methods
        /// <summary>
        /// Applying extra gravity on the player
        /// </summary>
        private void AdditionnalGravity()
        {
            //Add extra gravity to the player in air
            if (!IsGrounded && _rb.velocity.y > -maxVerticalVel)
                _rb.AddForce(-transform.up * extraGravity * _timeInAir, ForceMode.Force);

            if (IsOnSlope && (_fighting.IsAttacking && _fighting.CanAttack))
                _rb.AddForce(-transform.up * extraGravity * 0.1f, ForceMode.Force);
        }

        /// <summary>
        /// Increasing gravity by the time the player stay inAir
        /// </summary>
        private void InAirTime()
        {
            //Grounded => Reset timer
            if (IsGrounded)
                if (_timeInAir < maxTimeInAir)
                    _timeInAir += Time.deltaTime;
                //Not grounded => Increase timer
                else
                if (_timeInAir != minTimeInAir)
                    _timeInAir = minTimeInAir;
        }
        #endregion

        #region Checking Methods
        /// <summary>
        /// Setting player orientation
        /// </summary>
        private void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f);
        }

        /// <summary>
        /// Checking if the player touches the ground
        /// </summary>
        private void GroundCheck()
        {
            //Casting a sphere on the ground
            //Grounded
            if (Physics.CheckSphere(checkPosition.position, checkRadius, groundMask))
                IsGrounded = true;
            //Nor grounded
            else
                IsGrounded = false;
        }

        /// <summary>
        /// Checking if the player touches the ground
        /// </summary>
        private void SlopeCheck()
        {
            //Casting a sphere on the ground
            //OnSlope
            if (Physics.SphereCast(slopeCheckPosition.position, slopeRadius, Vector3.down, out _slopeHit, slopeDistance, slopeMask) && _slopeHit.normal != Vector3.zero)
            {
                Debug.DrawRay(_slopeHit.point, _slopeHit.normal, Color.cyan);
                IsOnSlope = true;
            }
            //Not onSlope
            else
                IsOnSlope = false;
        }

        /// <summary>
        /// Indicates if the player is not moving
        /// </summary>
        private bool IsIdled()
        {
            if (InputsVector == Vector2.zero)
                return true;

            return false;
        }
        #endregion

        #region Animations Methods
        /// <summary>
        /// Updating player animations
        /// </summary>
        private void UpdateAnimations()
        {
            //Grounded
            _animator.SetBool("IsGrounded", IsGrounded);


            if (IsIdled())
                _animator.SetBool("Inputs", true);
            else
                _animator.SetBool("Inputs", false);

            //Inputs
            _smoothInputs = Vector3.Lerp(_smoothInputs, InputsVector, lerpAnimInputs).normalized;

            if (_smoothInputs.x <= 0.05f)
                _smoothInputs.x = 0f;
            if (_smoothInputs.y <= 0.05f)
                _smoothInputs.y = 0f;

            _animator.SetFloat("DirX", _smoothInputs.x);
            _animator.SetFloat("DirY", _smoothInputs.y);

            //Motions Parameters
            if (IsIdled())
                _animMotionSpeed = Mathf.Lerp(_animMotionSpeed, 0f, lerpAnimMotion);
            else if (CanRun && IsRunning)
                _animMotionSpeed = Mathf.Lerp(_animMotionSpeed, 2f, lerpAnimMotion);
            else
                _animMotionSpeed = Mathf.Lerp(_animMotionSpeed, InputsVector.magnitude, lerpAnimMotion);

            if (IsIdled() && _animMotionSpeed < 0.1f)
                _animMotionSpeed = 0f;

            _animator.SetFloat("MotionSpeed", _animMotionSpeed);
        }
        #endregion

        #region Events Listening
        /// <summary>
        /// Subscribe to inputs events
        /// </summary>
        private void SubscribeToInputs()
        {
            //Motion
            _inputs.actions["Motion"].performed += ctx => InputsVector = ctx.ReadValue<Vector2>().normalized;
            _inputs.actions["Motion"].canceled += ctx => InputsVector = ctx.ReadValue<Vector2>().normalized;

            //Run
            _inputs.actions["Run"].started += ctx => IsRunning = ctx.ReadValueAsButton();
            _inputs.actions["Run"].canceled += ctx => IsRunning = ctx.ReadValueAsButton();

            //Dodge
            _inputs.actions["Dodge"].started += Dodge;
        }

        /// <summary>
        /// Unsubscribe to inputs events
        /// </summary>
        private void UnsubscribeToInputs()
        {
            //Motion
            _inputs.actions["Motion"].performed -= ctx => InputsVector = ctx.ReadValue<Vector2>().normalized;
            _inputs.actions["Motion"].canceled -= ctx => InputsVector = ctx.ReadValue<Vector2>().normalized;

            //Run
            _inputs.actions["Run"].started -= ctx => IsRunning = ctx.ReadValueAsButton();
            _inputs.actions["Run"].canceled -= ctx => IsRunning = ctx.ReadValueAsButton();

            //Dodge
            _inputs.actions["Dodge"].started -= Dodge;
        }
        public void EnableMovement() { CanMove = true; }
        public void DisableMovement() { CanMove = false; }
        public void SpeedReset() { _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f); }
        public void EnableRun() { CanRun = true; }
        public void DisableRun() { CanRun = false; }
        public void EnableDodge() { CanDodge = true; }
        public void StartDodgeAction() { IsDodging = true; }
        public void DisableDodge() { CanDodge = false; }
        public void EndDodgeAction() { IsDodging = false; }
        #endregion
    }
    #endregion
}
