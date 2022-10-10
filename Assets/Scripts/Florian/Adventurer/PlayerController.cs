using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    [SerializeField, Range(0f, 0.3f)] private float speedSmoothing = 0.15f;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float animationSmoothing = 0.075f;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.2f)] private float rotationSmoothing = 0.75f;

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
    private float _currentSpeed;
    private float _speedRef;
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
    /// Current air time ifnot grounded
    /// </summary>
    private float _airTime = 0.2f;

    //Movement vector
    private Vector3 _movement;
    #endregion

    #region GroundStateMachine
    /// <summary>
    /// Differents ground statements
    /// </summary>
    public enum GroundStates { Grounded, Falling }
    //Current ground state
    private GroundStates _currentGroundState;
    public GroundStates CurrentGroundState
    {
        get => _currentGroundState;
        set
        {
            if (_currentGroundState != value)
            {
                _currentGroundState = value;

                if (_currentGroundState == GroundStates.Grounded)
                {
                    _currentInputs = Vector3.zero;
                    _currentSpeed = 0f;
                    _airTime = 0.2f;
                }
            }
        }
    }
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

        //Dodge
        //_inputs.actions["Dodge"].started += Dodge;
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

        //Dodge
        //_inputs.actions["Dodge"].started -= Dodge;
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
        CurrentGroundState = _cc.isGrounded ? GroundStates.Grounded : GroundStates.Falling;

        switch (CurrentGroundState)
        {
            case GroundStates.Grounded:
                Debug.Log("Grounded");
                HandleMotion();
                break;

            case GroundStates.Falling:
                Debug.Log("Falling");
                HandleFall();
                break;
        }

        _cc.Move(_movement * Time.deltaTime);
    }

    /// <summary>
    /// Set player animations
    /// </summary>
    protected virtual void UpdateAnimations()
    {
        if (!_animator)
            return;

        //Grounded
        _animator.SetBool("IsGrounded", _currentGroundState == GroundStates.Grounded);
        //Inputs
        _animator.SetFloat("Inputs", _inputsVector.magnitude);

        //Motion speed
        //current value
        float current = _animator.GetFloat("Motion");
        //Target value
        float target = RunCondition() ? 2f : _currentSpeed >= adventurerDatas.walkSpeed ? 1f : _inputsVector.magnitude >= 0.1f ? _inputsVector.magnitude : 0f;
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
        _inputsVector = _inputs.actions["Move"].ReadValue<Vector2>();
        _currentInputs = Vector2.SmoothDamp(_currentInputs, _inputsVector, ref _smoothInputs, inputsSmoothing);

        //Movement
        _movement = orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x;
        _movement *= _currentSpeed;
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

        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, target, ref _speedRef, speedSmoothing);
    }

    /// <summary>
    /// Handle fall movement
    /// </summary>
    private void HandleFall()
    {
        //Increase air time
        _airTime += Time.deltaTime;

        //Inputs
        _movement = Vector3.Slerp(_movement, Vector3.zero, (fallSmoothing / 10f) * Time.deltaTime);
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

    #endregion

    #endregion
}

#region PlayerStateMachine
public class PlayerStateMachine
{
    /// <summary>
    /// Differents player states
    /// </summary>
    public enum PlayerStates { Walk, Dodge, Attack }
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

