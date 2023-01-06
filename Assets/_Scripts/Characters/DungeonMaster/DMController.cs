using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using _Scripts.Characters.Cameras;
using _Scripts.TrapSystem;
using _Scripts.TrapSystem.Datas;

namespace _Scripts.Characters.DungeonMaster
{
    public class DMController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private SkyCameraSetup cameraPrefab;
        [SerializeField] private GameObject dmHUD;
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform projectedTransform;

        [SerializeField] private float manaDM = 100;
        [SerializeField] private float manaRegen = 1;

        private GameObject _manaBar;
        private float _currentMana;
        private PlayerInput _inputs;
        private SkyCameraSetup _myCamera;
        private CinemachineTransposer _transposer;
        private PhotonView _view;

        #region Motion
        [Header("Inputs")]
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float rotateSpeed = 75f;

        private Vector2 _rotationInputs;
        private Vector3 _targetAngle;
        private float _scrollStep;

        [Header("Camera angle")]
        [SerializeField] private float scrollSteps = 10f;
        [SerializeField, Range(0f, 0.1f)] private float scrollSmoothing = 0.05f;
        [SerializeField] private Vector3 zoomRatio = new Vector3(20f, 35f, 50f);
        [SerializeField] private Vector3 distanceRatio = new Vector3(5f, 10f, 20f);
        #endregion

        #region RayCasting Variables
        [Header("Tiling Infos")]
        [SerializeField] private TilingSO tilingInfos;
        [SerializeField] private float checkRadius = 0.25f;
        [SerializeField] private float maxCheckDistance = 2f;
        [SerializeField] private LayerMask tilesMask;

        public static TrapSO selectedTrap;
        public static Transform _selectedTrapInstance;
        private Transform _lastTileHitted;
        private List<Tile> _reachedTiles = new List<Tile>();
        private RaycastHit _rayHit;
        private float _selectedTrapRotation;
        private bool _isHitting;
        private bool _canPlaceTrap;
        #endregion

        #endregion

        #region Properties
        public Vector2 InputsVector { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            _view = GetComponent<PhotonView>();

            if (!_view.IsMine)
                return;

            _inputs = GetComponent<PlayerInput>();

            _myCamera = PhotonNetwork.Instantiate(cameraPrefab.name, transform.position, Quaternion.identity).GetComponent<SkyCameraSetup>();
            _myCamera.SetCameraInfos(orientation);
            _transposer = _myCamera.VCam.GetCinemachineComponent<CinemachineTransposer>();
            _currentMana = manaDM;

            InstantiateUI();
        }

        private void Start()
        {
            _manaBar = GameObject.Find("ManaBarImage");
        }

        private void OnEnable()
        {
            if (!_view.IsMine)
                return;

            SubscribeToInputs();
        }

        private void OnDisable()
        {
            if (!_view.IsMine)
                return;

            UnsubscribeToInputs();
        }

        private void Update()
        {
            if (!_view.IsMine)
                return;

            HandleCameraMovements();
            ShootTileRay();
            UpdateCameraAngle();
            RegenMana();
        }
        #endregion

        #region Methods
        private void InstantiateUI()
        {
            if (!dmHUD)
                return;

            Instantiate(dmHUD);
        }

        #region Inputs
        /// <summary>
        /// Subscribe to inputs actions
        /// </summary>
        private void SubscribeToInputs()
        {
            _inputs.ActivateInput();

            _inputs.actions["Move"].performed += ctx => InputsVector = ctx.ReadValue<Vector2>();
            _inputs.actions["Move"].canceled += ctx => InputsVector = Vector2.zero;

            _inputs.actions["RotateCamCW"].started += ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.actions["RotateCamCW"].canceled += ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.actions["RotateCamACW"].started += ctx => _rotationInputs.y = ctx.ReadValue<float>();
            _inputs.actions["RotateCamACW"].canceled += ctx => _rotationInputs.y = ctx.ReadValue<float>();

            _inputs.actions["VerticalAngle"].started += SetScrollValue;

            _inputs.actions["Interact"].performed += InstantiateTrap;
            _inputs.actions["RotateCW"].performed += RotateClockwise;
            _inputs.actions["RotateACW"].performed += RotateAntiClockwise;
        }

        /// <summary>
        /// Unsubscribe to inputs actions
        /// </summary>
        private void UnsubscribeToInputs()
        {
            _inputs.DeactivateInput();

            _inputs.actions["Move"].performed -= ctx => InputsVector = ctx.ReadValue<Vector2>();
            _inputs.actions["Move"].canceled -= ctx => InputsVector = Vector2.zero;

            _inputs.actions["RotateCamCW"].started -= ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.actions["RotateCamCW"].canceled -= ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.actions["RotateCamACW"].started -= ctx => _rotationInputs.y = ctx.ReadValue<float>();
            _inputs.actions["RotateCamACW"].canceled -= ctx => _rotationInputs.y = ctx.ReadValue<float>();

            _inputs.actions["VerticalAngle"].started -= SetScrollValue;

            _inputs.actions["Interact"].performed -= InstantiateTrap;
            _inputs.actions["RotateCW"].performed -= RotateClockwise;
            _inputs.actions["RotateACW"].performed -= RotateAntiClockwise;
        }

        /// <summary>
        /// Set the scrolling step
        /// </summary>
        private void SetScrollValue(InputAction.CallbackContext ctx)
        {
            _scrollStep += ctx.ReadValue<Vector2>().y / 120f;
            _scrollStep = Mathf.Clamp(_scrollStep, -scrollSteps, scrollSteps);
        }
        #endregion

        #region Motion
        /// <summary>
        /// Moving the orientation point of the camera (Camera follows orientation movements and rotations)
        /// </summary>
        private void HandleCameraMovements()
        {
            orientation.rotation = Quaternion.Euler(0f, orientation.eulerAngles.y + (-_rotationInputs.y + _rotationInputs.x) * rotateSpeed * Time.deltaTime, 0f);

            Vector3 direction = Quaternion.Euler(0f, -orientation.eulerAngles.y, 0f) * (orientation.forward * InputsVector.y + orientation.right * InputsVector.x);
            orientation.Translate((direction.normalized) * moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Update camera YZ position based on scroll steps
        /// </summary>
        private void UpdateCameraAngle()
        {
            float stepPerc = _scrollStep / scrollSteps;
            float minAngle = zoomRatio.y - zoomRatio.x;
            float maxAngle = zoomRatio.z - zoomRatio.y;
            float _minDistance = distanceRatio.z - distanceRatio.y;
            float _maxDistance = distanceRatio.y - distanceRatio.x;

            _targetAngle.y = _scrollStep >= 0 ? stepPerc * maxAngle : stepPerc * minAngle;
            _targetAngle.y += zoomRatio.y;

            _targetAngle.z = -distanceRatio.y;
            _targetAngle.z += _scrollStep >= 0 ? -stepPerc * _maxDistance : stepPerc * _minDistance;

            _transposer.m_FollowOffset = Vector3.Slerp(_transposer.m_FollowOffset, _targetAngle, scrollSmoothing);
        }
        #endregion

        #region TrapCreation

        #region RayShooting
        /// <summary>
        /// Shooting a ray to place the selected trap
        /// </summary>
        private void ShootTileRay()
        {
            Ray ray = _myCamera.MainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan);

            if (selectedTrap == null)
                return;

            if (Physics.Raycast(ray, out _rayHit, Mathf.Infinity, tilesMask))
            {
                _isHitting = true;
                Transform hittedTile = !_lastTileHitted ? _rayHit.transform : _lastTileHitted == _rayHit.transform ? _lastTileHitted : _rayHit.transform;

                if (hittedTile == _lastTileHitted)
                    return;

                _lastTileHitted = hittedTile;
                UpdateTiling();
            }
            else
            {
                _isHitting = false;
            }
        }
        #endregion

        #region Tiling Logics Methods
        /// <summary>
        /// Differents steps to place a trap
        /// </summary>
        public void UpdateTiling()
        {
            //Step 1, D�finir la position du piege
            GetPivotPosition(_rayHit.transform.position, _rayHit.normal);
        }

        /// <summary>
        /// Calculating the pivotPosition
        /// </summary>
        public void GetPivotPosition(Vector3 hitPosition, Vector3 hitNormal)
        {
            Quaternion targetRotation;

            if (hitNormal == Vector3.up)
                targetRotation = Quaternion.Euler(0f, _selectedTrapRotation, 0f);
            else if (hitNormal == -Vector3.forward)
                targetRotation = Quaternion.Euler(_selectedTrapRotation, -90f, 90f);
            else if (hitNormal == Vector3.forward)
                targetRotation = Quaternion.Euler(_selectedTrapRotation, 90f, 90f);
            else if (hitNormal == Vector3.right)
                targetRotation = Quaternion.Euler(_selectedTrapRotation, 0f, -90f);
            else
                targetRotation = Quaternion.Euler(_selectedTrapRotation, 0f, 90f);

            float pivotX = Mathf.FloorToInt(selectedTrap.xAmount / 2) * tilingInfos.lengthX;
            float pivotZ = Mathf.FloorToInt(selectedTrap.yAmount / 2) * tilingInfos.lengthY;

            projectedTransform.position = hitPosition;
            Vector3 pivotPosition = projectedTransform.position + projectedTransform.right * pivotX + projectedTransform.forward * pivotZ;

            projectedTransform.rotation = targetRotation;
            _selectedTrapInstance.rotation = projectedTransform.rotation;

            //Step 2, PreVisualisation du piege a sa position
            SetTrapPosition(pivotPosition);
        }

        /// <summary>
        /// Place the Trap at the calculated pivot position
        /// </summary>
        public void SetTrapPosition(Vector3 pivotPosition)
        {
            Vector3 trapPosition = pivotPosition - projectedTransform.right * ((selectedTrap.xAmount - 1) * tilingInfos.lengthX * 0.5f) -
                                        projectedTransform.forward * ((selectedTrap.yAmount - 1) * tilingInfos.lengthY * 0.5f);

            _selectedTrapInstance.position = trapPosition;

            //Step 3, Mettre en couleur les tiles utilisees
            ShowRangedTiles(pivotPosition);
        }

        /// <summary>
        /// Showing Trap Tiling
        /// </summary>
        private void ShowRangedTiles(Vector3 pivotPosition)
        {
            ResetTilesList();
            bool placeTrap = true;

            for (int i = 0; i < selectedTrap.xAmount; i++)
            {
                for (int j = 0; j < selectedTrap.yAmount; j++)
                {
                    Vector3 castOrigin = pivotPosition - projectedTransform.right * (i * tilingInfos.lengthX) - projectedTransform.forward * (j * tilingInfos.lengthY) + _selectedTrapInstance.up;

                    if (Physics.SphereCast(castOrigin, checkRadius, -_selectedTrapInstance.up, out RaycastHit hit, maxCheckDistance, tilesMask))
                    {
                        if (hit.collider.TryGetComponent(out Tile tile) && !tile.IsUsed)
                            _reachedTiles.Add(tile);
                        else
                            placeTrap = false;
                    }
                    else
                        placeTrap = false;
                }
            }

            _canPlaceTrap = placeTrap;

            if (!placeTrap)
            {
                ChangeTilesStates(Tile.TileState.Waiting);
                return;
            }

            ChangeTilesStates(Tile.TileState.Selected);

        }
        #endregion

        #region Tiles Selection
        /// <summary>
        /// Changing the state of each tile in the reached tiles List
        /// </summary>
        /// <param name="newState"> Tile's new state </param>
        private void ChangeTilesStates(Tile.TileState newState)
        {
            foreach (Tile tile in _reachedTiles)
                tile.NewTileState(newState);
        }

        /// <summary>
        /// Clearing the hitted tiles List
        /// </summary>
        private void ResetTilesList()
        {
            ChangeTilesStates(Tile.TileState.Deselected);

            _reachedTiles.Clear();
        }
        #endregion

        #region Trap Methods
        /// <summary>
        /// Selecting a new trap
        /// </summary>
        /// <param name="targetTrap"> Selected trap </param>
        public static void SelectingTrap(TrapSO targetTrap)
        {
            selectedTrap = targetTrap;

            if (_selectedTrapInstance != null)
                Destroy(_selectedTrapInstance.gameObject);

            _selectedTrapInstance = Instantiate(selectedTrap.trapGhostPrefab, Vector3.up * 200f, Quaternion.identity).transform;
        }

        /// <summary>
        /// Instantiate a trap
        /// </summary>
        private void InstantiateTrap(InputAction.CallbackContext _)
        {
            if (!_canPlaceTrap || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            if (_currentMana >= selectedTrap.manaCost)
            {
                PhotonNetwork.Instantiate(selectedTrap.trapPrefab.name, _selectedTrapInstance.position, projectedTransform.rotation);
                ChangeTilesStates(Tile.TileState.Used);

                _currentMana -= selectedTrap.manaCost;
                _manaBar.GetComponent<Image>().fillAmount = _currentMana / manaDM;

                _selectedTrapRotation = 0f;
                _reachedTiles.Clear();
            }
        }

        private void RegenMana()
        {
            if(_currentMana >= manaDM)
                return;

            _currentMana += Time.deltaTime * manaRegen;
            _manaBar.GetComponent<Image>().fillAmount = _currentMana / manaDM;
        }

        /// <summary>
        /// Rotating the trap clockwise (angle between 0 and 360)
        /// </summary>
        public void RotateClockwise(InputAction.CallbackContext _)
        {
            if (!_isHitting)
                return;

            _selectedTrapRotation = Mathf.RoundToInt((((_selectedTrapRotation + 90) * Mathf.Deg2Rad) % (2 * Mathf.PI)) * Mathf.Rad2Deg);
            _lastTileHitted = null;
            UpdateTiling();
        }

        /// <summary>
        /// Rotating the trap antiClockwise (angle between -360 and 0)
        /// </summary>
        public void RotateAntiClockwise(InputAction.CallbackContext _)
        {
            if (!_isHitting)
                return;

            _selectedTrapRotation = Mathf.RoundToInt((((_selectedTrapRotation - 90) * Mathf.Deg2Rad) % (2 * Mathf.PI)) * Mathf.Rad2Deg);
            _lastTileHitted = null;
            UpdateTiling();
        }
        #endregion

        #endregion

        #endregion
    }
}
