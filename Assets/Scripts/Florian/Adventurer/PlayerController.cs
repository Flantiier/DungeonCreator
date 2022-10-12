using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;

public class PlayerController : MonoBehaviour
{
    #region Variables
    #region Global
    [Header("References")]
    /// <summary>
    /// Orientation reference
    /// </summary>
    [SerializeField] protected Transform orientation;
    /// <summary>
    /// Orientation reference
    /// </summary>
    [SerializeField] protected Transform playerMesh;
    //Player Camera
    protected Transform _playerCam;
    /// Player Rigidbody
    /// </summary>
    protected CharacterController _cc;
    //Player animator
    protected Animator _animator;
    #endregion

    #region Motion Variables

    #region Inputs
    [Header("Inputs")]
    /// <summary>
    /// Player Inputs Components
    /// </summary>
    [SerializeField] protected PlayerInput _inputs;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.3f)] private float inputsSmoothing = 0.1f;

    private Vector2 _inputsVector;
    /// <summary>
    /// Current input vector
    /// </summary>
    private Vector2 _currentInputs;
    //Reference to inputs damping
    private Vector2 _smoothInputs;
    //Reference to rotation damping 
    private float _turnVelocity;
    #endregion

    #region Ground
    [Header("Ground")]
    /// <summary>
    /// Ground detection distance
    /// </summary>
    [SerializeField] private float groundDistance = 1f;
    // Walkable layer
    [SerializeField] private LayerMask walkableMask;

    #region GroundStateMachine
    /// <summary>
    /// Differents ground statements
    /// </summary>
    public enum GroundStates { Grounded, Falling }
    //Current ground state
    protected GroundStates _currentGroundState;
    public bool IsLanding { get; set; }
    #endregion

    #endregion

    #region Character
    [Header("Character")]
    /// <summary>
    /// Move speed value
    /// </summary>
    [SerializeField] private AdventurerData adventurerDatas;
    public AdventurerData AdventurerDatas => adventurerDatas;

    /// <summary>
    /// Smoothing inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float speedSmoothing = 0.15f;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float animationSmoothing = 0.075f;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float rotationSmoothing = 0.75f;

    [Space]

    [SerializeField] private AnimationCurve dodgeCurve;
    public AnimationCurve DodgeCurve => dodgeCurve;
    public float dodgeSpeed = 5f;
    //Dodge direction
    private Vector2 _dodgeDir;

    [Header("Camera")]
    /// <summary>
    /// Adventurer Camera prefab
    /// </summary>
    [SerializeField] private AdventurerCameraSetup camPrefab;
    /// <summary>
    ///¨Camera LookAt target
    /// </summary>
    [SerializeField] private Transform lookAt;
    /// <summary>
    /// Camera control overall
    /// </summary>
    [SerializeField] private CameraAxis cameraControls;

    //Current movement speed
    public float CurrentSpeed { get; private set; }
    private float _speedRef;
    private bool _lowGround;

    /// <summary>
    /// Player StateMachine
    /// </summary>
    private PlayerStateMachine _playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;
    #endregion

    #region Gravity
    [Header("Gravity")]
    /// <summary>
    /// Gravity value
    /// </summary>
    [SerializeField] private float gravityValue = 5f;
    /// <summary>
    /// Smooth inputs during fall
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float fallSmoothing = 0.15f;
    /// <summary>
    /// Air time max to trigger the landing animation
    /// </summary>
    [SerializeField] private float airTimeToLand = 1f;
    public float TimeToLand => airTimeToLand;
    /// <summary>
    /// Current air time ifnot grounded
    /// </summary>
    private float _airTime = 0.2f;
    public float AirTime => _airTime;

    //Movement vector
    private Vector3 _movement;
    #endregion

    #endregion

    #endregion

    #region Builts_In
    public virtual void Awake()
    {
        //Get RB Component
        _cc = GetComponent<CharacterController>();
        //Get Inputs Component
        _inputs = GetComponent<PlayerInput>();
        //Get the animator
        _animator = GetComponentInChildren<Animator>();
        //New SM
        _playerStateMachine = new PlayerStateMachine();

        //Create a camera
        InstantiateCamera();
    }

    public virtual void OnEnable()
    {
        SubscribeToInputs();
    }

    public virtual void OnDisable()
    {
        UnsubscribeToInputs();
    }

    public virtual void Update()
    {
        //Camera and player orientation
        cameraControls.RotateCamera(lookAt, _inputs);
        SetOrientation();

        //Movements
        HandleMotionMachine();
        UpdateAnimations();
    }

    private void OnDrawGizmos()
    {
        if (LowGroundDetect())
            Gizmos.color = Color.cyan;
        else
            Gizmos.color = Color.red;
    }
    #endregion

    #region Methods

    #region Inputs Methods
    /// <summary>
    /// Subscribe to inputs events
    /// </summary>
    protected virtual void SubscribeToInputs()
    {
        //No inputs
        if (!_inputs)
            return;

        //Activate inputs map
        _inputs.ActivateInput();

        //Inputs
        _inputs.actions["Move"].performed += ctx => _inputsVector = ctx.ReadValue<Vector2>();
        _inputs.actions["Move"].canceled += ctx => _inputsVector = Vector2.zero;

        //Dodge
        _inputs.actions["Roll"].started += StartDodge;
    }

    /// <summary>
    /// Unsubscribe to inputs events
    /// </summary>
    protected virtual void UnsubscribeToInputs()
    {
        //No inputs
        if (!_inputs)
            return;

        //Deactivate inputs map
        _inputs.DeactivateInput();

        //Inputs
        _inputs.actions["Move"].performed -= ctx => _inputsVector = ctx.ReadValue<Vector2>();
        _inputs.actions["Move"].canceled -= ctx => _inputsVector = Vector2.zero;


        //Dodge
        _inputs.actions["Roll"].started -= StartDodge;
    }
    #endregion

    #region Camera
    private void InstantiateCamera()
    {
        if (!camPrefab)
        {
            Debug.LogError("Camera reference missing");
            return;
        }

        //Create a new Camera
        AdventurerCameraSetup instance = Instantiate(camPrefab);
        instance.SetLookAt(lookAt);
        //Set player camera
        _playerCam = instance.mainCam.transform;
    }
    #endregion

    #region StateMachines Methods
    /// <summary>
    /// Handle motion
    /// </summary>
    private void HandleMotionMachine()
    {
        _currentGroundState = _cc.isGrounded || _lowGround ? GroundStates.Grounded : GroundStates.Falling;

        switch (_currentGroundState)
        {
            case GroundStates.Grounded:

                HandlePlayerSM();
                break;

            case GroundStates.Falling:

                //Debug.Log("Falling");
                HandleFall();
                break;
        }

        _cc.Move(_movement * Time.deltaTime);
    }

    private void HandlePlayerSM()
    {
        switch (_playerStateMachine.CurrentState)
        {
            case PlayerStateMachine.PlayerStates.Walk:

                //Detecting low ground
                _lowGround = LowGroundDetect();

                if (!IsLanding)
                {
                    //Debug.Log("Grounded");
                    HandleMotion();
                }
                break;
        }
    }

    /// <summary>
    /// Set player animations
    /// </summary>
    protected virtual void UpdateAnimations()
    {
        if (!_animator)
            return;

        //Grounded
        _animator.SetBool("IsGrounded", _currentGroundState == GroundStates.Grounded || _lowGround);
        //Inputs
        _animator.SetFloat("Inputs", _inputsVector.magnitude);

        //Motion speed
        //current value
        float current = _animator.GetFloat("Motion");
        //Target value
        float target = RunCondition() ? 2f : CurrentSpeed >= adventurerDatas.walkSpeed ? 1f : _inputsVector.magnitude >= 0.1f ? _inputsVector.magnitude : 0f;
        //Lerp current
        float value = Mathf.Lerp(current, target, animationSmoothing);
        //Set value
        _animator.SetFloat("Motion", value);
    }

    #endregion

    #region Movements Methods

    /// <summary>
    /// Moving player
    /// </summary>
    private void HandleMotion()
    {
        //Set speed
        GetMovementSpeed();

        //Calculate direction Vector
        _currentInputs = Vector2.SmoothDamp(_currentInputs, _inputsVector, ref _smoothInputs, inputsSmoothing);

        //Movement
        _movement = orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x;
        _movement *= CurrentSpeed;
        //Gravity
        _movement.y = -gravityValue;

        //Rotate
        RotatePlayer();
    }

    /// <summary>
    /// Set the current speed of the player
    /// </summary>
    private void GetMovementSpeed()
    {
        float target = 0f;

        if (RunCondition())
            target = adventurerDatas.runSpeed;
        else if (_inputsVector.magnitude >= 0.1f)
            target = adventurerDatas.walkSpeed;

        CurrentSpeed = Mathf.SmoothDamp(CurrentSpeed, target, ref _speedRef, speedSmoothing);
    }

    /// <summary>
    /// Handle fall movement
    /// </summary>
    private void HandleFall()
    {
        //Increase air time
        _airTime += Time.deltaTime;

        //Inputs
        _movement = Vector3.Slerp(_movement, Vector3.zero, fallSmoothing / 10f);
        _movement.y = -gravityValue * _airTime;

    }

    /// <summary>
    /// Setting player orientation
    /// </summary>
    private void SetOrientation()
    {
        if (!orientation || !_playerCam)
        {
            Debug.LogError("Missing camera or orientation");
            return;
        }

        orientation.rotation = Quaternion.Euler(0f, _playerCam.eulerAngles.y, 0f);
    }

    /// <summary>
    /// Handle player mesh rotation
    /// </summary>
    private void RotatePlayer()
    {
        if (!playerMesh)
            return;

        if (_inputsVector.magnitude >= 0.1f)
        {
            float angle = Mathf.Atan2(_inputsVector.x, _inputsVector.y) * Mathf.Rad2Deg + orientation.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(playerMesh.eulerAngles.y, angle, ref _turnVelocity, rotationSmoothing);
            playerMesh.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        }
    }

    /// <summary>
    /// Return if the player can run
    /// </summary>
    /// <returns></returns>
    private bool RunCondition()
    {
        return _inputsVector.magnitude >= 0.8f && _inputs.actions["Run"].IsPressed();
    }

    /// <summary>
    /// Reset player momentum
    /// </summary>
    public void ResetVelocity()
    {
        _currentInputs = Vector2.zero;
        CurrentSpeed = 0f;
        _movement = new Vector3(0f, _movement.y, 0f);
    }

    //Reset player airTime
    public void ResetAirTime()
    {
        _airTime = 0f;
    }

    /// <summary>
    /// Secondary check to ground
    /// </summary>
    public bool LowGroundDetect()
    {
        Ray ray = new Ray(transform.position + new Vector3(0f, 0.25f, 0f), -transform.up);
        Debug.DrawRay(ray.origin, ray.direction * groundDistance);

        if (Physics.Raycast(ray, groundDistance, walkableMask))
            return true;

        return false;
    }

    #endregion

    #region Dodge
    private void StartDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && !DodgeCondition())
            return;

        _dodgeDir = new Vector2(playerMesh.forward.x, playerMesh.forward.z);
        _animator.SetTrigger("Dodging");
    }

    public void HandleDodgeMovement(float speed)
    {
        _movement = new Vector3(_dodgeDir.x, -gravityValue, _dodgeDir.y) * speed;
    }

    private bool DodgeCondition()
    {
        return _playerStateMachine.CurrentState != PlayerStateMachine.PlayerStates.Roll && _currentGroundState == GroundStates.Grounded;
    }
    #endregion

    #endregion
}

#region PlayerStateMachine
[System.Serializable]
public class PlayerStateMachine
{
    /// <summary>
    /// Differents player states
    /// </summary>
    public enum PlayerStates { Walk, Roll, Attack }
    /// <summary>
    /// Current player state
    /// </summary>
    public PlayerStates CurrentState { get; set; }
}
#endregion

#region CameraAxisValues
[System.Serializable]
public class CameraAxis
{
    [SerializeField, Tooltip("X Axis variables")]
    private AxisState x_Axis;
    public AxisState X_Axis => x_Axis;

    [SerializeField, Tooltip("Y Axis variables")]
    private AxisState y_Axis;
    public AxisState Y_Axis => y_Axis;

    public void RotateCamera(Transform target, PlayerInput inputs)
    {
        if (!target || !inputs)
            return;

        //Update values on X and Y Axis 
        x_Axis.Update(Time.fixedDeltaTime);
        y_Axis.Update(Time.fixedDeltaTime);
        //Reading MouseInput values
        x_Axis.m_InputAxisValue = inputs.actions["Mouse"].ReadValue<Vector2>().x;
        y_Axis.m_InputAxisValue = inputs.actions["Mouse"].ReadValue<Vector2>().y;

        //Setting lookAt rotation
        target.eulerAngles = new Vector3(y_Axis.Value, x_Axis.Value, 0f);
    }
}
#endregion

