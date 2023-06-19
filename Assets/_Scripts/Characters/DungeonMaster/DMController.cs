using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using Photon.Pun;
using Sirenix.OdinInspector;
using _Scripts.Managers;
using _Scripts.Cameras;
using _Scripts.GameplayFeatures;
using _Scripts.TrapSystem;
using _Scripts.GameplayFeatures.Traps;
using _ScriptableObjects.Characters;

namespace _Scripts.Characters.DungeonMaster
{
    [RequireComponent(typeof(TilingCulling))]
    public class DMController : MonoBehaviour
    {
        #region Variables

        #region References
        [FoldoutGroup("References")]
        [SerializeField] private TilingSO tiling;
        [FoldoutGroup("References")]
        [SerializeField] private Transform projection;
        [FoldoutGroup("References")]
        [SerializeField] private TilingInteractor interactor;
        [FoldoutGroup("References")]
        [SerializeField] private TopCamera cameraPrefab;

        [TitleGroup("References/Variables")]
        [SerializeField] private FloatVariable mana;
        [TitleGroup("References/Events")]
        [SerializeField] private GameEvent dragEvent, dropEvent;
        [TitleGroup("References/Events")]
        [SerializeField] private GameEvent cardZoneEnter;

        private InputsDM _inputs;
        #endregion

        #region Motion
        [TitleGroup("Motion")]
        [SerializeField] private Vector2 horizontalLimit;
        [TitleGroup("Motion")]
        [SerializeField] private Vector2 verticalLimit;

        private Vector2 _inputsVector;
        private Vector3 _currentMovement;
        private Vector2 _rotationInputs;
        private Coroutine _manaRoutine;
        private float _targetCameraHeight;
        #endregion

        #region Raycast & Traps
        [FoldoutGroup("Raycast properties")]
        [SerializeField] private LayerMask raycastMask, collisionMask;
        [FoldoutGroup("Raycast properties")]
        [SerializeField] private Vector3 offMapPosition = new Vector3(0f, -100f, 0f);

        private Transform _hittedTransform;
        private float _currentRotation;
        private GameObject _trapInstance;
        #endregion

        [LabelText("DM Properties")]
        [Required, SerializeField] private DMProperties properties;
        #endregion

        #region Properties
        public static bool IsDragging { get; set; }
        public static DraggableCard SelectedCard { get; set; }
        public TopCamera Camera { get; private set; }
        #endregion

        #region Builts_In
        public void Awake()
        {
            InitializeCharacter();
        }

        private void OnEnable()
        {
            EnableInputs(true);
            SubscribeToInputs();
        }

        private void OnDisable()
        {
            EnableInputs(false);
            UnsubscribeToInputs();
        }

        private void Update()
        {
            HandleMovements();
            UpdateCameraHieght();
            ShootingRaycast();
        }
        #endregion

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
            _inputs.Gameplay.Scrolling.performed += OnScrollAction;

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
            _inputs.Gameplay.Scrolling.performed += OnScrollAction;

            //Rotations
            _inputs.Gameplay.CamRotate_CW.performed -= ctx => _rotationInputs.x = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_ACW.performed -= ctx => _rotationInputs.y = ctx.ReadValue<float>();
            _inputs.Gameplay.CamRotate_CW.canceled -= ctx => _rotationInputs.x = 0f;
            _inputs.Gameplay.CamRotate_ACW.canceled -= ctx => _rotationInputs.y = 0f;

            //Trap Inputs
            _inputs.Gameplay.RotateTrap.started -= RotateTrapClockwise;
        }

        private void OnScrollAction(InputAction.CallbackContext ctx)
        {
            _targetCameraHeight += ctx.ReadValue<float>() * properties.scrollSpeed;
            _targetCameraHeight = Mathf.Clamp(_targetCameraHeight, properties.minCameraHeight, properties.maxCameraHeight);
        }
        #endregion

        #region Methods
        public void DisableCharacter()
        {
            Camera.gameObject. SetActive(false);
            transform.root.gameObject.SetActive(false);
        }

        /// <summary>
        /// Create the inputs, the camera and sets variables
        /// </summary>
        private void InitializeCharacter()
        {
            _inputs = new InputsDM();
            InstantiateCamera();
            mana.value = properties.manaAmount;
            _targetCameraHeight = Camera.CameraHeight;
        }

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

            Camera = Instantiate(cameraPrefab);
            Camera.SetLookAt(transform);
        }

        #region Movements
        /// <summary>
        /// Movements control method
        /// </summary>
        private void HandleMovements()
        {
            //Rotation
            float rotation = (_rotationInputs.x - _rotationInputs.y) * properties.rotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, rotation, 0f);

            //Motion
            Vector3 movement = Quaternion.Euler(0f, -transform.eulerAngles.y, 0f) * (transform.forward * _inputsVector.y + transform.right * _inputsVector.x);
            _currentMovement = Vector3.Lerp(_currentMovement, movement.normalized, properties.smoothingMotion);

            //Clamped position
            Vector3 finalMovement = properties.motionSpeed * Time.deltaTime * _currentMovement;
            Vector3 finalPosition = transform.position + transform.forward * finalMovement.z + transform.right * finalMovement.x;
            finalPosition.x = Mathf.Clamp(finalPosition.x, horizontalLimit.x, horizontalLimit.y);
            finalPosition.z = Mathf.Clamp(finalPosition.z, verticalLimit.x, verticalLimit.y);

            //Apply movement
            transform.position = finalPosition;
        }

        /// <summary>
        /// Modify the height of the camera when using mouse scroll
        /// </summary>
        private void UpdateCameraHieght()
        {
            float value = Mathf.Lerp(Camera.CameraHeight, _targetCameraHeight, properties.scrollSmooth);
            Camera.SetCameraHeight(value);
        }
        #endregion

        #region Mana Methods
        /// <summary>
        /// Decrease the amount from the current mana
        /// </summary>
        /// <param name="amount"> Used mana amount </param>
        public void UseMana(float amount)
        {
            if (!HasMuchMana(amount))
                return;

            mana.value -= amount;

            if (_manaRoutine == null)
                _manaRoutine = StartCoroutine("ManaRecoveryRoutine");
        }

        /// <summary>
        /// Returns if the amount can be decreased from current mana
        /// </summary>
        /// <param name="amount"> Used mana </param>
        public bool HasMuchMana(float amount)
        {
            return mana.value - amount >= 0;
        }

        /// <summary>
        /// Increase the current mana coroutine
        /// </summary>
        private IEnumerator ManaRecoveryRoutine()
        {
            while (mana.value < properties.manaAmount)
            {
                mana.value += properties.manaRecovery * Time.deltaTime;
                yield return null;
            }

            mana.value = properties.manaAmount;
            _manaRoutine = null;
        }
        #endregion

        #endregion

        #region Drag&Drop
        /// <summary>
        /// Set the trap preview position
        /// </summary>
        public void HandleStartDrag()
        {
            IsDragging = true;

            //Set the position of the projection transform off map
            projection.position = offMapPosition;

            //Set required amount of tiles
            interactor.Amount = SelectedCard.TrapReference.xAmount * SelectedCard.TrapReference.yAmount;

            //Set collider size
            float amountX = SelectedCard.TrapReference.xAmount - 2f;
            float amountY = SelectedCard.TrapReference.yAmount - 2f;
            float X = amountX <= 0f ? tiling.lengthX / 2f : amountX * tiling.lengthX + 0.5f;
            float Y = amountY <= 0f ? tiling.lengthY / 2f : amountY * tiling.lengthX + 0.5f;
            interactor.SetColliderSize(X, Y);

            //Instantiate the selected trap reference
            GetSelectedTrap();
        }

        /// <summary>
        /// Puts the trap is possible, reset the projection transform and the interactor
        /// </summary>
        public void HandleDragEnd()
        {
            IsDragging = false;

            //Place trap if it's possible
            if (IsPossibleToPlaceTrap())
                PlaceTrapOnGrid();

            if (_trapInstance)
                Destroy(_trapInstance);

            //Reset projection transform
            _hittedTransform = null;
            projection.position = offMapPosition;
            _currentRotation = 0f;
            //Refresh interactor
            interactor.RefreshInteractor();
        }

        /// <summary>
        /// Indicates if a trap can be place when drag is ending
        /// </summary>
        private bool IsPossibleToPlaceTrap()
        {
            if (!HasMuchMana(SelectedCard.TrapReference.manaCost))
                return false;

            //Shoot a ray to konw if the cursor is on a tile
            if (Physics.Raycast(GetRayFromScreenPoint(), out RaycastHit hit, Mathf.Infinity, raycastMask))
            {
                //Hit something else that a tile
                if (Utils.Utilities.Layers.LayerMaskContains(collisionMask, hit.transform.gameObject.layer))
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

        /// <summary>
        /// Executed when entering in the card pointer zone
        /// </summary>
        public void EnterPointerZone()
        {
            _hittedTransform = null;
            projection.position = offMapPosition;
        }
        #endregion

        #region Trap Interactions Methods

        #region RayShooting/Tiling Interactions
        /// <summary>
        /// Shooting a ray to place the selected trap
        /// </summary>
        private void ShootingRaycast()
        {
            //If there is a trap card selected
            if (!IsDragging || !SelectedCard || CardZone.CursorOnZone)
                return;

            Ray ray = GetRayFromScreenPoint();
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
            {
                //Hitting something else that the tiling
                if (Utils.Utilities.Layers.LayerMaskContains(collisionMask, hit.transform.gameObject.layer))
                    return;

                //Hitting the same tile that the last one
                //Check if the tiling type is correct
                if (_hittedTransform == hit.transform || !GetTileType(hit.collider.GetComponent<Tile>()))
                    return;

                //Update tiling on new tile selected
                _hittedTransform = hit.transform;

                //Update tiling
                SetProjectionPosition();
                UpdateTiling();
            }
        }

        /// <summary>
        /// Get a ray which starts from the center of the camera to the mouse screen position
        /// </summary>
        private Ray GetRayFromScreenPoint()
        {
            return UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        }

        /// <summary>
        /// Indicates if the tile is the same tiling type that the trap
        /// </summary>
        private bool GetTileType(Tile tile)
        {
            return SelectedCard.TrapReference.type == Tile.TilingType.Both || SelectedCard.TrapReference.type == tile.TileType;
        }

        /// <summary>
        /// Set the last object hitted and the projection position and rotation
        /// </summary>
        /// <param name="hittedObj"></param>
        private void SetProjectionPosition()
        {
            //Set position and rotation of the projection transform
            projection.position = _hittedTransform.position;

            //Hit a different oriented tiling
            if (projection.up != _hittedTransform.up)
            {
                interactor.RefreshInteractor();
                projection.rotation = _hittedTransform.rotation * Quaternion.Euler(0f, _currentRotation, 0f);
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
            _trapInstance.transform.position = trapPosition;
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
            _trapInstance = Instantiate(SelectedCard.TrapReference.previewPrefab, projection);
        }

        /// <summary>
        /// PUts a trap on current tiled selected
        /// </summary>
        private void PlaceTrapOnGrid()
        {
            if (!SelectedCard || !_trapInstance)
                return;

            GameObject instance = PhotonNetwork.Instantiate(SelectedCard.TrapReference.trapPrefab.name, _trapInstance.transform.position, _trapInstance.transform.rotation);
            instance.GetComponent<TrapClass1>().OccupedTiles = interactor.Tiles.ToArray();

            //Set tiles that will be occuped
            interactor.SetAllTiles(Tile.TileState.Used);
            //Decrease Mana
            UseMana(SelectedCard.TrapReference.manaCost);
            //Rotate the card
            DeckManager.Instance.SendToStorageZone(SelectedCard.transform);
        }

        /// <summary>
        /// Rotate the trap projection clockwisely
        /// </summary>
        public void RotateTrapClockwise(InputAction.CallbackContext _)
        {
            if (!IsDragging)
                return;

            _currentRotation = _currentRotation + 90f >= 360f ? 0f : _currentRotation + 90f;
            projection.rotation = _hittedTransform.rotation * Quaternion.Euler(0f, _currentRotation, 0f);

            UpdateTiling();
        }
        #endregion

        #endregion
    }
}
