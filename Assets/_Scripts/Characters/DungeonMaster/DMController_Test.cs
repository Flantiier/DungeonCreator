using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using Photon.Pun;
using Sirenix.OdinInspector;
using Personnal.Florian;
using _Scripts.Characters.Cameras;
using _Scripts.GameplayFeatures;
using _Scripts.TrapSystem;
using _Scripts.GameplayFeatures.Traps;
using _ScriptableObjects.DM;

namespace _Scripts.Characters.DungeonMaster
{
    [RequireComponent(typeof(TilingCulling))]
    public class DMController_Test : MonoBehaviourSingleton<DMController_Test>
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
        [SerializeField] private SkyCameraSetup cameraPrefab;

        private InputsDM _inputs;
        private SkyCameraSetup _camSetup;
        #endregion

        [FoldoutGroup("Stats")]
        [Required, SerializeField] private DMDatas datas;
        //Motion
        private Vector2 _inputsVector;
        private Vector3 _currentMovement;
        private Vector2 _rotationInputs;
        private Coroutine _manaRoutine;
        //RAYCAST
        [FoldoutGroup("Raycast properties")]
        [SerializeField] private LayerMask raycastMask;
        [FoldoutGroup("Raycast properties")]
        [SerializeField] private LayerMask collisionMask;
        [FoldoutGroup("Raycast properties")]
        [SerializeField] private Vector3 offMapPosition = new Vector3(0f, -100f, 0f);

        private Transform _hittedTransform;
        private float _currentRotation;
        private GameObject _trapInstance;
        //DRAG & DROP
        public event Action OnStartDrag;
        public event Action OnEndDrag;
        public event Action<Tile.TilingType> OnSelectedCard;

        #endregion

        #region Properties
        public DMDatas Datas => datas;
        public bool IsDragging { get; set; }
        public DraggableCard SelectedCard { get; private set; }
        public float CurrentMana { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            _inputs = new InputsDM();
            CurrentMana = datas.manaAmount;
            InstantiateCamera();
        }

        private void OnEnable()
        {
            EnableInputs(true);
            SubscribeToInputs();

            PointerZone.OnEnterPointerZone += EnterPointerZone;
        }

        private void OnDisable()
        {
            EnableInputs(false);
            UnsubscribeToInputs();

            PointerZone.OnEnterPointerZone -= EnterPointerZone;
        }

        private void Update()
        {
            HandleMovements();
            ShootingRaycast();
        }

        private void LateUpdate()
        {
            if (CurrentMana >= datas.manaAmount || _manaRoutine != null)
                return;

            _manaRoutine = StartCoroutine("RecoveryRoutine");
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

        private void HandleStartDrag()
        {
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

            //StartDrag event call
            OnStartDrag?.Invoke();
            OnSelectedCard?.Invoke(SelectedCard.TrapReference.type);
        }

        private void HandleDragEnd()
        {
            _hittedTransform = null;
            projection.position = offMapPosition;
            _currentRotation = 0f;

            interactor.RefreshInteractor();
            OnEndDrag?.Invoke();
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
            float rotation = (_rotationInputs.x - _rotationInputs.y) * datas.rotationSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0f, rotation, 0f);

            //Motion
            Vector3 movement = Quaternion.Euler(0f, -transform.eulerAngles.y, 0f) * (transform.forward * _inputsVector.y + transform.right * _inputsVector.x);
            _currentMovement = Vector3.Lerp(_currentMovement, movement.normalized, datas.smoothingMotion);
            transform.Translate(_currentMovement * datas.motionSpeed * UnityEngine.Time.deltaTime);
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

            CurrentMana -= amount;
        }

        /// <summary>
        /// Returns if the amount can be decreased from current mana
        /// </summary>
        /// <param name="amount"> Used mana </param>
        public bool HasMuchMana(float amount)
        {
            return CurrentMana - amount >= 0;
        }

        /// <summary>
        /// Increase the current mana coroutine
        /// </summary>
        private IEnumerator RecoveryRoutine()
        {
            while (CurrentMana < datas.manaAmount)
            {
                CurrentMana += datas.manaRecovery * Time.deltaTime;
                yield return null;
            }

            CurrentMana = datas.manaAmount;
            _manaRoutine = null;
        }
        #endregion

        #endregion

        #region Trap Interactions Methods

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
                if (PersonnalUtilities.Layers.LayerMaskContains(collisionMask, hit.transform.gameObject.layer))
                    return;

                Debug.Log("Layer OK");

                //Hitting the same tile that the last one
                //Check if the tiling type is correct
                if (_hittedTransform == hit.transform || !GetTileType(hit.collider.GetComponent<Tile>()))
                    return;

                //Update tiling on new tile selected
                _hittedTransform = hit.transform;

                Debug.Log("Tiles OK");

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
            return _camSetup.MainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
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

        #region Dragging Methods
        /// <summary>
        /// Invoking start drag event
        /// </summary>
        public void StartDrag(DraggableCard cardRef)
        {
            //Set the dragged card
            IsDragging = true;
            SelectedCard = cardRef;

            HandleStartDrag();
            GetSelectedTrap();
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

            if(_trapInstance)
                Destroy(_trapInstance);

            HandleDragEnd();
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
                if (PersonnalUtilities.Layers.LayerMaskContains(collisionMask, hit.transform.gameObject.layer))
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
        private void EnterPointerZone()
        {
            _hittedTransform = null;
            projection.position = offMapPosition;
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
