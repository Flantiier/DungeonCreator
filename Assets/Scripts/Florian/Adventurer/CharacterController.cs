using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    #region Variables
    #region Global
    /// <summary>
    /// Player Rigidbody
    /// </summary>
    protected Rigidbody _rb;
    /// <summary>
    /// Player Inputs Components
    /// </summary>
    protected PlayerInput _inputs;
    #endregion

    #region Motion Variables
    [Header("Motion Variables")]
    /// <summary>
    /// Informations to detect the ground
    /// </summary>
    [SerializeField] private GroundStatement groundStatement;
    /// <summary>
    /// Direction inputs vector
    /// </summary>
    public Vector2 DirectionInputs { get; private set; }
    /// <summary>
    /// RaycastHit of the motion check
    /// </summary>
    private RaycastHit _slopeHit;
    /// <summary>
    /// Player StateMachine
    /// </summary>
    protected PlayerStateMachine _stateMachine;
    public PlayerStateMachine StateMachine => _stateMachine;
    #endregion
    #endregion

    #region Builts_In
    public virtual void Awake()
    {
        //Get RB Component
        _rb = GetComponent<Rigidbody>();
        //Get Inputs Component
        _inputs = GetComponent<PlayerInput>();
        //Create a new StateMachine
        _stateMachine = new PlayerStateMachine();
    }

    public virtual void OnEnable()
    {
        SubscribeToInputs();
    }

    public virtual void OnDisable()
    {
        UnsubscribeToInputs();
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

        //Motion
        _inputs.actions["Motion"].performed += ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;
        _inputs.actions["Motion"].canceled += ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;

        //Run
        //_inputs.actions["Run"].started += ctx => IsRunning = ctx.ReadValueAsButton();
        //_inputs.actions["Run"].canceled += ctx => IsRunning = ctx.ReadValueAsButton();

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

        //Motion
        _inputs.actions["Motion"].performed -= ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;
        _inputs.actions["Motion"].canceled -= ctx => DirectionInputs = ctx.ReadValue<Vector2>().normalized;

        //Run
        //_inputs.actions["Run"].started -= ctx => IsRunning = ctx.ReadValueAsButton();
        //_inputs.actions["Run"].canceled -= ctx => IsRunning = ctx.ReadValueAsButton();

        //Dodge
        //_inputs.actions["Dodge"].started -= Dodge;
    }
    #endregion

    #region StateMachine Methods
    protected void HandleStateMachine()
    {
        switch (_stateMachine.currentState)
        {
            case PlayerStateMachine.PlayerStates.Grounded:
                break;

            case PlayerStateMachine.PlayerStates.Dodging:
                break;

            case PlayerStateMachine.PlayerStates.Falling:
                break;
        }
    }
    #endregion

    #region Motion Methods
    /// <summary>
    /// Checking if the player touches the ground
    /// </summary>
    private bool IsOnGround()
    {
        //Casting a sphere on the ground
        //Grounded
        if (Physics.CheckSphere(groundStatement.GroundCheck.position, groundStatement.GroundRadius, groundStatement.WalkableMask))
            return true;

        //Nor grounded
        return false;
    }

    /// <summary>
    /// Checking if the player touches the ground
    /// </summary>
    private bool IsOnSlope()
    {
        //Casting a sphere on the ground
        //OnSlope
        if (Physics.SphereCast(groundStatement.SlopeCheck.position, groundStatement.SlopeRadius, Vector3.down, out _slopeHit, groundStatement.SlopeDistance, groundStatement.WalkableMask) && _slopeHit.normal != Vector3.zero)
        {
            Debug.DrawRay(_slopeHit.point, _slopeHit.normal, Color.cyan);
            return true;
        }

        //Not onSlope
        return false;
    }
    #endregion

    #endregion
}

#region MotionClasses
[System.Serializable]
public class GroundStatement
{
    [SerializeField] private LayerMask walkableMask;
    public LayerMask WalkableMask => walkableMask;

    [Space]

    [SerializeField] private Transform groundCheck;
    public Transform GroundCheck => groundCheck;

    [SerializeField, Range(0f, 1f)] private float groundRadius = 0.1f;
    public float GroundRadius => groundRadius;

    [Space]

    [SerializeField] private Transform slopeCheck;
    public Transform SlopeCheck => slopeCheck;

    [SerializeField, Range(0f, 1f)] private float slopeRadius = 0.1f;
    public float SlopeRadius => slopeRadius;

    [SerializeField, Range(0f, 1f)] private float slopeDistance = 0.5f;
    public float SlopeDistance => slopeDistance;
}
#endregion
