using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Photon.Pun;

public class SkyCamera : MonoBehaviour
{
    #region References Variables
    [Header("Camera References")]
    [SerializeField, Tooltip("Referencing the virtualCam of the player")]
    private CinemachineVirtualCamera vCam;

    [SerializeField, Tooltip("Referencing the orientation transform")]
    private Transform orientation;

    //PView Comp
    private PhotonView _view;
    //ThirdPersonFollow Comp on vCam
    private Cinemachine3rdPersonFollow _thirdPerson;
    //Player Inputs Comp
    private PlayerInput _inputs;
    #endregion

    #region Camera Motion Variables
    [Header("Camera Movements")]
    [SerializeField, Tooltip("Camera Moving speed")]
    private float movingSpeed = 10f;

    [SerializeField, Tooltip("Rotate speed value")]
    private float rotateSpeed = 75f;

    [Header("Camera Repositionning")]
    [SerializeField, Range(1f, 2f), Tooltip("Speed Scroll multiplier")]
    private float scrollingMultilier = 1.25f;

    [SerializeField, Tooltip("Maximum distance on the Y Axis")]
    private float maxUpDistance = 40f;

    [SerializeField, Tooltip("Minimum zoom angle (90 - value)")]
    private float minZoomValue = 10f;

    [SerializeField, Tooltip("Maximum zoom angle (90 - value)")]
    private float maxZoomValue = 50f;

    /// <summary>
    /// Indicates the direction of the rotation on inputs
    /// </summary>
    private Vector2 _inputsRotation;
    #endregion

    #region Properties
    /// <summary>
    /// Direction Inputs vector
    /// </summary>
    public Vector2 DirectionInputs { get; private set; }
    #endregion

    #region Builts-In Methods
    private void Awake()
    {
        //Get View
        _view = transform.root.GetComponent<PhotonView>();

        //Not local
        if (!_view.IsMine)
            return;

        //Local
        //Get Inputs
        _inputs = GetComponent<PlayerInput>();
        //Get ThirdPersonFollow Comp on the vCam
        _thirdPerson = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void OnEnable()
    {
        //Not local
        if (!_view.IsMine)
            return;

        //Local
        SubscribeToInputs();
    }

    private void OnDisable()
    {
        //Not local
        if (!_view.IsMine)
            return;

        //Local
        UnsubscribeToInputs();
    }

    private void Update()
    {
        //Not local
        if (!_view.IsMine)
            return;

        //Local
        //Move Camera
        CameraMotion();
    }
    #endregion

    #region Camera Methods
    /// <summary>
    /// Moving the orientation point of the camera (Camera follows orientation movements and rotations)
    /// </summary>
    private void CameraMotion()
    {
        //Setting the orientation rotation
        orientation.rotation = Quaternion.Euler(0f, orientation.eulerAngles.y + (-_inputsRotation.y + _inputsRotation.x) * rotateSpeed * Time.deltaTime, 0f);
        //Calculating the direction with inputs
        Vector3 direction = Quaternion.Euler(0f, -orientation.eulerAngles.y, 0f) * (orientation.forward * DirectionInputs.y + orientation.right * DirectionInputs.x);

        //Moving orientation point
        orientation.Translate((direction.normalized) * movingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Repositionning the camera behind and up distance
    /// </summary>
    private void CameraRepositionning(InputAction.CallbackContext ctx)
    {
        //Calcultating camera distance with scroll value
        float scroll = _thirdPerson.CameraDistance + (ctx.ReadValue<Vector2>().y / 120f) * scrollingMultilier;
        //Clamping the rotation between bounds
        scroll = Mathf.Clamp(scroll, minZoomValue, maxZoomValue);
        //Setting the cameraDistance
        _thirdPerson.CameraDistance = scroll;

        //Setting Rig shouder Y 
        //Calculating the percentage of the actual angle on the y Axis
        float scrollPercents = ((scroll - minZoomValue) * 100f) / (maxZoomValue - minZoomValue) / 2f;
        //Bounds reached
        if (scroll == minZoomValue || scroll == maxZoomValue)
            //Return
            return;

        //Bounds not reached
        //Setting the position by multiplying the maxDistance with percentage of angle
        _thirdPerson.ShoulderOffset.y = maxUpDistance * (1f - (scrollPercents / 100f));
    }
    #endregion

    #region Events Subscribing
    /// <summary>
    /// Suscribe to inputs events
    /// </summary>
    private void SubscribeToInputs()
    {
        //Directions Inputs
        _inputs.actions["Motion"].performed += ctx => DirectionInputs = ctx.ReadValue<Vector2>();
        _inputs.actions["Motion"].canceled += ctx => DirectionInputs = ctx.ReadValue<Vector2>();

        //Inputs Rotations => X
        _inputs.actions["RotateCamCW"].started += ctx => _inputsRotation.x = ctx.ReadValue<float>();
        _inputs.actions["RotateCamCW"].canceled += ctx => _inputsRotation.x = ctx.ReadValue<float>();

        //Inputs Rotations => Y
        _inputs.actions["RotateCamACW"].started += ctx => _inputsRotation.y = ctx.ReadValue<float>();
        _inputs.actions["RotateCamACW"].canceled += ctx => _inputsRotation.y = ctx.ReadValue<float>();

        //Scrolling
        _inputs.actions["Scrolling"].performed += CameraRepositionning;
    }

    /// <summary>
    /// Unsubscribe to inputs events
    /// </summary>
    private void UnsubscribeToInputs()
    {
        //Directions Inputs
        _inputs.actions["Motion"].performed -= ctx => DirectionInputs = ctx.ReadValue<Vector2>();
        _inputs.actions["Motion"].canceled -= ctx => DirectionInputs = ctx.ReadValue<Vector2>();

        //Inputs Rotations => X
        _inputs.actions["RotateCamCW"].started -= ctx => _inputsRotation.x = ctx.ReadValue<float>();
        _inputs.actions["RotateCamCW"].canceled -= ctx => _inputsRotation.x = ctx.ReadValue<float>();

        //Inputs Rotations => Y
        _inputs.actions["RotateCamACW"].started -= ctx => _inputsRotation.y = ctx.ReadValue<float>();
        _inputs.actions["RotateCamACW"].canceled -= ctx => _inputsRotation.y = ctx.ReadValue<float>();

        //Scrolling
        _inputs.actions["Scrolling"].performed -= CameraRepositionning;
    }
    #endregion
}
