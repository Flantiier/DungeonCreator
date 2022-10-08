using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    #region Global
    [Header("References")]
    //Player Camera
    [SerializeField] private Transform playerCam;
    /// <summary>
    /// Orientation reference
    /// </summary>
    [SerializeField] private Transform orientation;
    /// Player Rigidbody
    /// </summary>
    protected CharacterController _cc;
    #endregion

    #region Motion Variables

    [Header("Inputs Info")]
    /// <summary>
    /// Player Inputs Components
    /// </summary>
    [SerializeField] protected PlayerInput _inputs;
    /// <summary>
    /// Smooth inputs value
    /// </summary>
    [SerializeField, Range(0f, 0.3f)] private float inputsSmoothing = 0.2f;

    /// <summary>
    /// Current input vector
    /// </summary>
    private Vector2 _currentInputs;
    //Reference for inputs smoothDamp
    private Vector2 _smoothInputs;

    [Header("Movement Info")]
    /// <summary>
    /// Move speed value
    /// </summary>
    [SerializeField] private AdventurerData adventurerDatas;
    public AdventurerData AdventurerDatas => adventurerDatas;

    [SerializeField, Range(0f, 0.5f)] private float speedSmoothing = 0.2f;

    //Current movement speed
    private float _currentSpeed;
    private float _speedRef;

    [Header("Gravity Info")]
    //Applied gravity parameters
    [SerializeField] private GravityInfo gravityInfo;
    /// <summary>
    /// Current air time ifnot grounded
    /// </summary>
    private float _airTime = 0.2f;

    //Movement vector
    private Vector3 _movement;

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

                if(_currentGroundState == GroundStates.Grounded)
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
        HandleMotionMachine();
        SetOrientation();
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

    #region StateMachines Methods
    protected void HandleMotionMachine()
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
        Vector2 inputs = _inputs.actions["Move"].ReadValue<Vector2>();
        _currentInputs = Vector2.SmoothDamp(_currentInputs, inputs, ref _smoothInputs, inputsSmoothing);

        //Movement
        _movement = orientation.forward * _currentInputs.y + orientation.right * _currentInputs.x;
        _movement *= _currentSpeed;
        //Gravity
        _movement.y = -gravityInfo.gravityValue;
    }

    /// <summary>
    /// Set the current speed of the player
    /// </summary>
    private void GetMovementSpeed()
    {
        float target = 0f;
        bool moving = _currentInputs.magnitude >= 0.1f;

        if (moving && _inputs.actions["Run"].IsPressed())
            target = adventurerDatas.runSpeed;
        else if (moving)
            target = adventurerDatas.moveSpeed;

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
        _movement = Vector3.Slerp(_movement, Vector3.zero, gravityInfo.fallSmoothing * Time.deltaTime);
        _movement.y = -gravityInfo.gravityValue * _airTime;

    }

    /// <summary>
    /// Setting player orientation
    /// </summary>
    private void SetOrientation()
    {
        if(!orientation || !playerCam)
        {
            Debug.LogError("Missing camera or orientation");
            return;
        }

        orientation.rotation = Quaternion.Euler(0f, playerCam.eulerAngles.y, 0f);
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

#region GravityInfo
[System.Serializable]
public class GravityInfo
{
    /// <summary>
    /// Gravity value
    /// </summary>
    public float gravityValue = 5f;
    /// <summary>
    /// Smooth inputs during fall
    /// </summary>
    public float fallSmoothing = 0.015f;
}
#endregion

