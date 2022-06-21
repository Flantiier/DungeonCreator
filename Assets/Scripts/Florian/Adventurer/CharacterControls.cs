using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    public class CharacterControls : MonoBehaviour
    {
        #region References
        [Header("References")]
        [SerializeField] private Transform playerCam;
        [SerializeField] private Transform playerMesh;
        [SerializeField] private Transform orientation;
        private PlayerInput _inputs;
        private Rigidbody _rb;
        private Animator _animator;
        private EventsListener _events;
        private CharacterFighting _fighting;
        #endregion

        #region Motion Variables
        [Header("Moton Variables")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float aimSpeed = 3f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField, Range(0f, 1f)] private float minRunValue = 0.75f;
        [SerializeField, Range(0f, 1f)] private float lerpMotion = 0.1f;
        private Vector3 _movement;
        private Vector3 _smoothInputs;
        private float _movementMultiplier = 1f;
        private float _targetVel;
        #endregion

        #region Rotation Variables
        [Header("Rotation Variables")]
        [SerializeField, Range(0f, 1f)] private float lerpRotation = 0.1f;
        private float _turnVel;
        #endregion

        #region Ground Variables
        [Header("Ground Check Variables")]
        [SerializeField] private Transform checkPosition;
        [SerializeField, Range(0f, 1f)] private float checkRadius = 0.1f;
        [SerializeField] private LayerMask groundMask;
        #endregion

        #region Slope Variables
        [Header("Slope Check Variables")]
        [SerializeField, Range(0f, 1f)] private float slopeDistance = 0.5f;
        [SerializeField] private LayerMask slopeMask;
        private RaycastHit _slopeHit;
        #endregion

        #region Gravity Variables
        [Header("Gravity Variables")]
        [SerializeField] private float extraGravity = 3f;
        [SerializeField] private float maxVerticalVel = 30f;
        [SerializeField] private float minTimeInAir = 1f;
        [SerializeField] private float maxTimeInAir = 3f;
        private float _timeInAir;
        #endregion

        #region Dodge Variables
        [Header("Dodge Variables")]
        [SerializeField] private float dodgeForce = 3f;
        #endregion

        #region Animations Variables
        [Header("Animations Variables")]
        [SerializeField, Range(0f, 1f)] private float lerpAnimInputs = 0.05f;
        [SerializeField, Range(0f, 1f)] private float lerpAnimMotion = 0.05f;
        private float _animMotionSpeed;
        #endregion

        #region Properties
        public Vector2 DirectionInputs { get; private set; }
        public float MoveSpeed { get; private set; }
        public float MovementMultiplier
        {
            get { return _movementMultiplier; }
            set { _movementMultiplier = value; }
        }
        public bool IsGrounded { get; private set; }
        public bool IsOnSlope { get; private set; }
        public bool CanMove { get; private set; }
        public bool CanRun { get; private set; }
        public bool IsRunning { get; private set; }
        public bool CanDodge { get; private set; }
        public bool IsDodging { get; private set; }
        #endregion

        #region Builts-In
        private void Awake()
        {
            _inputs = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody>();
            _animator = playerMesh.GetComponent<Animator>();
            _events = GetComponent<EventsListener>();
            _fighting = GetComponent<CharacterFighting>();


            EnableMovement();
            EnableRun();
            EnableDodge();
        }

        private void OnEnable()
        {
            _inputs.ActivateInput();

            SubscribeToInputs();
        }

        private void OnDisable()
        {
            _inputs.DeactivateInput();

            UnsubscribeToInputs();
        }

        private void Update()
        {
            SetOrientation();
            GroundCheck();
            SlopeCheck();

            AdditionnalGravity();
            InAirTime();
            HandleMotion();
            DodgeMovement(playerMesh.forward * dodgeForce);

            UpdateAnimations();
        }
        #endregion

        #region Motion Methods
        private void HandleMotion()
        {
            if (!IsGrounded || !CanMove)
                return;

            MotionSpeed();

            _movement = orientation.forward * DirectionInputs.y + orientation.right * DirectionInputs.x;

            if (_fighting.CanAim && _fighting.IsAiming)
                AimRotate();
            else
                StandardRotate();

            if (IsOnSlope)
                _rb.velocity = Vector3.ProjectOnPlane(_movement, _slopeHit.normal) * MoveSpeed;
            else
                _rb.velocity = _movement * MoveSpeed;
        }

        private void AimRotate()
        {
            playerMesh.rotation = Quaternion.Euler(0f, orientation.eulerAngles.y, 0f);
        }

        private void StandardRotate()
        {
            if (DirectionInputs.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(DirectionInputs.x, DirectionInputs.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, angle, ref _turnVel, lerpRotation);
                playerMesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        private void MotionSpeed()
        {
            if (IsIdled())
                _targetVel = 0f;
            else if (_fighting.CanAim && _fighting.IsAiming)
                _targetVel = aimSpeed;
            else if (CanRun && IsRunning && DirectionInputs.magnitude >= minRunValue)
                _targetVel = runSpeed;
            else
                _targetVel = walkSpeed;

            _targetVel *= _movementMultiplier;

            if (!Mathf.Approximately(MoveSpeed, _targetVel))
                MoveSpeed = Mathf.Lerp(MoveSpeed, _targetVel, lerpMotion);
            else
                MoveSpeed = _targetVel;

            if (MoveSpeed < 0.1f)
                MoveSpeed = 0f;
        }

        public void ImpulsePlayer(Vector3 movement)
        {
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);

            if (IsOnSlope)
                _rb.AddForce(Vector3.ProjectOnPlane(movement, _slopeHit.normal), ForceMode.VelocityChange);
            else
                _rb.AddForce(movement, ForceMode.VelocityChange);
        }
        #endregion

        #region Dodge Methods
        private void Dodge(InputAction.CallbackContext ctx)
        {
            if (!CanDodge || !IsGrounded || IsDodging)
                return;

            _animator.SetTrigger("Dodge");
        }

        private void DodgeMovement(Vector3 movement)
        {
            if (!IsDodging || !IsGrounded)
                return;

            if (IsOnSlope)
                _rb.velocity = Vector3.ProjectOnPlane(movement, _slopeHit.normal);
            else
                _rb.velocity = movement;
        }
        #endregion

        #region Gravity Methods
        private void AdditionnalGravity()
        {
            //Add extra gravity to the player in air
            if (!IsGrounded && _rb.velocity.y > -maxVerticalVel)
                _rb.AddForce(-transform.up * extraGravity * _timeInAir, ForceMode.Force);
        }

        private void InAirTime()
        {
            if (IsGrounded)
            {
                if (_timeInAir < maxTimeInAir)
                    _timeInAir += Time.deltaTime;
            }
            else
            {
                if (_timeInAir != minTimeInAir)
                    _timeInAir = minTimeInAir;
            }
        }
        #endregion

        #region Checking Methods
        private void SetOrientation()
        {
            orientation.rotation = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f);
        }

        private void GroundCheck()
        {
            if (Physics.CheckSphere(checkPosition.position, checkRadius, groundMask))
                IsGrounded = true;
            else
                IsGrounded = false;
        }

        private void SlopeCheck()
        {
            if (Physics.Raycast(checkPosition.position, Vector3.down, out _slopeHit, slopeDistance, slopeMask) && _slopeHit.normal == Vector3.up)
            {
                IsOnSlope = false;
                _rb.useGravity = true;
            }
            else
            {
                IsOnSlope = true;
                _rb.useGravity = false;
            }
        }

        private bool IsIdled()
        {
            if (DirectionInputs == Vector2.zero)
                return true;

            return false;
        }
        #endregion

        #region Animations Methods
        private void UpdateAnimations()
        {
            _animator.SetBool("IsGrounded", IsGrounded);

            if (IsIdled())
                _animator.SetBool("Inputs", true);
            else
                _animator.SetBool("Inputs", false);

            //Inputs
            _smoothInputs = Vector3.Lerp(_smoothInputs, DirectionInputs, lerpAnimInputs).normalized;

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
                _animMotionSpeed = Mathf.Lerp(_animMotionSpeed, DirectionInputs.magnitude, lerpAnimMotion);

            if (IsIdled() && _animMotionSpeed < 0.1f)
                _animMotionSpeed = 0f;

            _animator.SetFloat("MotionSpeed", _animMotionSpeed);
        }

        public void EnableMovement() { CanMove = true;}
        public void DisableMovement() { CanMove = false; }
        public void SpeedReset() { _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f); }
        public void EnableRun() { CanRun = true; }
        public void DisableRun() { CanRun = false; }
        public void EnableDodge() { CanDodge = true; }
        public void StartDodgeAction() { IsDodging = true; }
        public void DisableDodge() { CanDodge = false; }
        public void EndDodgeAction() { IsDodging = false; }
        #endregion

        #region Events Listening
        private void SubscribeToInputs()
        {
            _inputs.actions["Motion"].performed += ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;
            _inputs.actions["Motion"].canceled += ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;

            _inputs.actions["Run"].started += ctx => IsRunning = ctx.ReadValueAsButton();
            _inputs.actions["Run"].canceled += ctx => IsRunning = ctx.ReadValueAsButton();

            _inputs.actions["Dodge"].started += Dodge;
        }

        private void UnsubscribeToInputs()
        {
            _inputs.actions["Motion"].performed -= ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;
            _inputs.actions["Motion"].canceled -= ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;

            _inputs.actions["Run"].started -= ctx => IsRunning = ctx.ReadValueAsButton();
            _inputs.actions["Run"].canceled -= ctx => IsRunning = ctx.ReadValueAsButton();

            _inputs.actions["Dodge"].started -= Dodge;
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(checkPosition.position, checkRadius);

            Debug.DrawRay(checkPosition.position, Vector3.down * slopeDistance, Color.red);
        }
    }
}
