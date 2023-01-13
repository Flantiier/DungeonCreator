using System;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using _Scripts.NetworkScript;
using _Scripts.Characters.Cameras;
using _Scripts.GameplayFeatures;

namespace _Scripts.Characters.DungeonMaster
{
    public class DMController_Test : MonoBehaviourSingleton<DMController_Test>
    {
        #region Variables
        public Transform ghostTrap;

        #region References
        [Header("References")]
        [SerializeField] private TilingSO tiling;
        [SerializeField] private Transform projection;
        [SerializeField] private TilingInteractor interactor;
        [SerializeField] private SkyCameraSetup cameraPrefab;

        private InputsDM _inputs;
        private SkyCameraSetup _camSetup;
        #endregion

        #region Motion
        [Header("Motion properties")]
        [SerializeField] private float moveSpeed = 25f;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField, Range(0f, 0.2f)] private float smoothingMovements = 0.1f;

        private Vector2 _inputsVector;
        private Vector3 _currentMovement;
        private Vector2 _rotationInputs;
        #endregion

        #region RayShooting/Traps
        [Header("Raycast properties")]
        [SerializeField] private string tilingLayer = "Tiling";
        [SerializeField] private LayerMask raycastMask;
        [SerializeField] private LayerMask trapPositioningMask;

        [Header("Positionning properties")]
        [SerializeField] private Vector3 offMapPosition = new Vector3(0f, -100f, 0f);

        private Transform _hittedTile;
        private float _currentRotation;
        private Transform _trapInstance;
        #endregion

        #region Drag References
        public event Action OnStartDrag;
        public event Action OnEndDrag;
        #endregion

        #endregion

        #region Properties
        public bool IsDragging { get; set; }
        public DraggableCard SelectedCard { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            _inputs = new InputsDM();
            InstantiateCamera();
        }

        private void OnEnable()
        {
            EnableInputs(true);
            SubscribeToInputs();

            OnStartDrag += StartDragListener;
            OnEndDrag += EndDragListener;
            OnStartDrag += GetSelectedTrap;
        }

        private void OnDisable()
        {
            EnableInputs(false);
            UnsubscribeToInputs();

            OnStartDrag -= StartDragListener;
            OnEndDrag -= EndDragListener;
            OnStartDrag -= GetSelectedTrap;
        }

        private void Update()
        {
            HandleMovements();
            ShootingRaycast();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Instantiate sky camera
        /// </summary>
        private void InstantiateCamera()
        {
            if (!cameraPrefab)
            {
                Debug.LogWarning("Missing camera prefab");
                return;
            }

            _camSetup = Instantiate(cameraPrefab);
            _camSetup.SetLookAtTarget(transform);
        }

        private void StartDragListener()
        {
            //Set the position of  the projection transform off map
            projection.position = offMapPosition;

            //Set required amount of tiles
            interactor.Amount = SelectedCard.TrapReference.xAmount * SelectedCard.TrapReference.yAmount;

            //Set collider size
            float amountX = SelectedCard.TrapReference.xAmount - 2f;
            float amountY = SelectedCard.TrapReference.yAmount - 2f;
            float X = amountX <= 0f ? tiling.lengthX / 2f : amountX * tiling.lengthX + 0.5f;
            float Y = amountY <= 0f ? tiling.lengthY / 2f : amountY * tiling.lengthX + 0.5f;
            interactor.SetColliderSize(X, Y);
        }

        private void EndDragListener()
        {
            //Reset last tile hitted
            _hittedTile = null;

            //Reset projection values
            projection.position = offMapPosition;
            _currentRotation = 0f;

            //Reset interactor
            interactor.RefreshInteractor();
        }

        #region Inputs
        /// <summary>
        /// Enable or disable inputs based on given parameter
        /// </summary>
        /// <param name="enabled"></param>
        public void EnableInputs(bool enabled)
        {
            if (enabled)
                _inputs.Enable();
            else
                _inputs.Disable();
        }

        /// <summary>
        /// Subscribing to inputs events
        /// </summary>
        private void SubscribeToInputs()
        {
            //Movements
            _inputs.Gameplay.Move.performed += ctx => _inputsVector = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Move.canceled += ctx => _inputsVector = Vector2.zero;

            //Rotations
            _inputs.Gameplay.CamRotate_CW.performed += ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_ACW.performed += ctx => _rotationInputs.y = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_CW.canceled += ctx => _rotationInputs.x = 0f;
            _inputs.Gameplay.CamRotate_ACW.canceled += ctx => _rotationInputs.y = 0f;

            //Trap Inputs
            _inputs.Gameplay.RotateTrap.started += RotateTrapClockwise;
        }

        /// <summary>
        /// Unsubscribing to inputs events
        /// </summary>
        private void UnsubscribeToInputs()
        {
            //Movements
            _inputs.Gameplay.Move.performed -= ctx => _inputsVector = ctx.ReadValue<Vector2>();
            _inputs.Gameplay.Move.canceled -= ctx => _inputsVector = Vector2.zero;

            //Rotations
            _inputs.Gameplay.CamRotate_CW.performed -= ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_ACW.performed -= ctx => _rotationInputs.y = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_CW.canceled -= ctx => _rotationInputs.x = 0f;
            _inputs.Gameplay.CamRotate_ACW.canceled -= ctx => _rotationInputs.y = 0f;

            //Trap Inputs
            _inputs.Gameplay.RotateTrap.started -= RotateTrapClockwise;
        }
        #endregion

        #region Movements
        /// <summary>
        /// Movements control method
        /// </summary>
        private void HandleMovements()
        {
            //Rotation
            float rotation = (_rotationInputs.x - _rotationInputs.y) * rotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, rotation, 0f);

            //Motion
            Vector3 movement = Quaternion.Euler(0f, -transform.eulerAngles.y, 0f) * (transform.forward * _inputsVector.y + transform.right * _inputsVector.x);
            _currentMovement = Vector3.Lerp(_currentMovement, movement.normalized, smoothingMovements);
            transform.Translate(_currentMovement * moveSpeed * Time.deltaTime);
        }
        #endregion

        #region RayShooting/Tiling Interactions
        /// <summary>
        /// Shooting a ray to place the selected trap
        /// </summary>
        private void ShootingRaycast()
        {
            //If there is a trap card selected
            if (!IsDragging || !SelectedCard)
                return;

            Ray ray = GetRayFromScreenPoint();
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
            {
                //Hitting something else that the tiling
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer(tilingLayer))
                    return;

                //Hitting the same tile that the last one
                if (_hittedTile == hit.transform)
                    return;

                //Update tiling on new tile selected
                SetProjectionPosition(hit.transform);
                UpdateTiling();
            }
        }

        /// <summary>
        /// Get a ray which starts from the center of the camera to the mouse screen position
        /// </summary>
        private Ray GetRayFromScreenPoint()
        {
            return _camSetup.MainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        }

        /// <summary>
        /// Set the last object hitted and the projection position and rotation
        /// </summary>
        /// <param name="hittedObj"></param>
        private void SetProjectionPosition(Transform hittedObj)
        {
            //Set position and rotation of the projection transform
            _hittedTile = hittedObj;
            projection.position = _hittedTile.position;

            //Hit a different oriented tiling
            if (projection.up != _hittedTile.up)
            {
                interactor.RefreshInteractor();
                projection.rotation = _hittedTile.rotation * Quaternion.Euler(0f, _currentRotation, 0f);
            }
        }

        /// <summary>
        /// Update the grid color based on the selected trap
        /// </summary>
        private void UpdateTiling()
        {
            //Get the trap position based on 
            Vector3 Xpos = projection.right * ((1 - SelectedCard.TrapReference.xAmount % 2) * tiling.lengthX * 0.5f);
            Vector3 Ypos = projection.forward * ((1 - SelectedCard.TrapReference.yAmount % 2) * tiling.lengthY * 0.5f);
            Vector3 trapPosition = projection.position + (Xpos + Ypos);

            interactor.transform.position = trapPosition;
            _trapInstance.position = trapPosition;
        }
        #endregion

        #region Dragging Methods
        /// <summary>
        /// Invoking start drag event
        /// </summary>
        public void StartDrag(DraggableCard cardRef)
        {
            //Set the dragged card
            IsDragging = true;
            SelectedCard = cardRef;

            //StartDrag event call
            OnStartDrag?.Invoke();
        }

        /// <summary>
        /// Invoking start drag event
        /// </summary>
        public void EndDrag()
        {
            //End drag
            IsDragging = false;

            //Place trap if it's possible
            if (IsPossibleToPlaceTrap())
                PlaceTrapOnGrid();
            else
                Destroy(_trapInstance.gameObject);

            OnEndDrag?.Invoke();
        }

        /// <summary>
        /// Indicates if a trap can be place when drag is ending
        /// </summary>
        private bool IsPossibleToPlaceTrap()
        {
            //Shoot a ray to konw if the cursor is on a tile
            if (Physics.Raycast(GetRayFromScreenPoint(), out RaycastHit hit, Mathf.Infinity, raycastMask))
            {
                //Hit something else that a tile
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer(tilingLayer))
                    return false;

                //Check if all tiles are free
                if (!interactor.EnoughTilesAmount() || !interactor.IsTilingFree())
                    return false;

                //Hit a tile
                return true;
            }

            //Hitting nothing
            return false;
        }
        #endregion

        #region Trap Positioning
        /// <summary>
        /// Preview the trap on the grid before placing it
        /// </summary>
        private void GetSelectedTrap()
        {
            if (!SelectedCard)
                return;

            //Get ghost prefab
            _trapInstance = Instantiate(SelectedCard.TrapReference.trapPrefab, projection).transform;
            //Set material to preview mat
            _trapInstance.GetComponentInChildren<Renderer>().material = SelectedCard.TrapReference.GetPreviewMaterial();
        }

        /// <summary>
        /// PUts a trap on current tiled selected
        /// </summary>
        private void PlaceTrapOnGrid()
        {
            if (!SelectedCard || !_trapInstance)
                return;
            
            //Set trap position
            _trapInstance.SetParent(null);
            //Set its material to base material
            _trapInstance.GetComponentInChildren<Renderer>().material = SelectedCard.TrapReference.GetDefaultMaterial();
            //Set all tiles on used
            interactor.SetAllTiles(TrapSystem.Tile.TileState.Used);
        }

        /// <summary>
        /// Rotate the trap projection clockwisely
        /// </summary>
        public void RotateTrapClockwise(InputAction.CallbackContext _)
        {
            if (!IsDragging)
                return;

            _currentRotation = _currentRotation + 90f >= 360f ? 0f : _currentRotation + 90f;
            projection.rotation = _hittedTile.rotation * Quaternion.Euler(0f, _currentRotation, 0f);

            UpdateTiling();
        }
        #endregion

        #endregion
    }
}
