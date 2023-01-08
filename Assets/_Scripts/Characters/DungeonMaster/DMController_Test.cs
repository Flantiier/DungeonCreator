using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InputsMaps;
using _Scripts.NetworkScript;
using _Scripts.Characters.Cameras;
using _Scripts.TrapSystem;
using _Scripts.TrapSystem.Datas;

namespace _Scripts.Characters.DungeonMaster
{
    public class DMController_Test : MonoBehaviour
    {
        #region Variables

        #region References
        [Header("References")]
        [SerializeField] private TilingSO tiling;
        [SerializeField] private Transform orientation;
        [SerializeField] private SkyCameraSetup cameraPrefab;

        private InputsDM _inputs;
        private SkyCameraSetup _camSetup;
        #endregion

        #region Motion
        [Header("Motion properties")]
        [SerializeField] private float moveSpeed = 25f;
        [SerializeField] private float rotationSpeed = 100f;
        [Space, SerializeField, Range(0f, 0.2f)] private float smoothingMovements = 0.1f;

        [Header("Test")]
        [SerializeField] private Transform mark;
        [SerializeField] private Transform mark2;
        [SerializeField] private int X = 2;
        [SerializeField] private int Y = 2;

        private Vector2 _inputsVector;
        private Vector3 _currentMovement;
        private Vector2 _rotationInputs;
        #endregion

        #region RayShooting
        [Header("Raycast properties")]
        [SerializeField] private string tilingLayer = "Tiling";
        [SerializeField] private LayerMask raycastMask;
        [SerializeField] private LayerMask trapPositioningMask;
        [SerializeField, Range(0f, 1f)] private float checkDistance = 0.5f;
        [SerializeField, Range(0f, 1f)] private float correctiveOffset = 0.6f;
        [SerializeField, Range(0f, 0.1f)] private float gridCorrection = 0.05f;

        public static TrapSO selectedTrap;
        public static Transform _selectedTrapInstance;
        private Transform _hittedTile;
        private List<Tile> _reachedTiles = new List<Tile>();
        private RaycastHit _rayHit;
        private float _selectedTrapRotation;
        private bool _isHitting;
        private bool _canPlaceTrap;
        #endregion

        #endregion

        #region Builts_In
        private void Awake()
        {
            _inputs = new InputsDM();

            InitializeMethod();
            InstantiateCamera();
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
            ShootingRaycast();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method runned at awake
        /// </summary>
        private void InitializeMethod()
        {

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

            _camSetup = Instantiate(cameraPrefab);
            _camSetup.SetLookAtTarget(transform);
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

        #region RayShooting
        /// <summary>
        /// Shooting a ray to place the selected trap
        /// </summary>
        private void ShootingRaycast()
        {
            Ray ray = _camSetup.MainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan);

            if (Physics.Raycast(ray, out _rayHit, Mathf.Infinity, raycastMask))
            {
                //Hitting something else that the tiling
                if (_rayHit.collider.gameObject.layer != LayerMask.NameToLayer(tilingLayer))
                    return;

                //Hitting the same tile that the last one
                if (_hittedTile == _rayHit.transform)
                    return;

                //Update tiling on new tile selectedq
                ResetTiling();
                _hittedTile = _rayHit.transform;
                UpdateTiling();
            }
        }
        #endregion

        #region Tiling Interactions
        /// <summary>
        /// Reset tiling color on tile changed
        /// </summary>
        private void ResetTiling()
        {
            if (!_hittedTile)
                return;

            _hittedTile.GetComponent<Tile>().NewTileState(Tile.TileState.Deselected);
        }

        /// <summary>
        /// Update the grid color based on the selected trap
        /// </summary>
        private void UpdateTiling()
        {
            _hittedTile.GetComponent<Tile>().NewTileState(Tile.TileState.Selected);

            //Get the grid bounds
            Vector3 gridX = orientation.right * ((X / 2f * tiling.lengthX) - (X % 2 * tiling.lengthX * correctiveOffset));
            Vector3 gridY = orientation.forward * ((Y / 2f * tiling.lengthY) - (Y % 2 * tiling.lengthY * correctiveOffset));
            Vector3 startPos = _hittedTile.position + (gridX + gridY);
            mark2.position = startPos;

            if (!Input.GetKey(KeyCode.Space))
                return;

            //Check the grid bounds
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    Vector3 origin = startPos + orientation.up * 0.25f;
                    Vector3 cast = orientation.right * (-i * tiling.lengthX + gridCorrection) + orientation.forward * (-j * tiling.lengthY + gridCorrection);
                    Vector3 fullCast = origin + cast;

                    //Instantiate(mark2, fullCast, Quaternion.identity);

                    if (Physics.Raycast(fullCast, -orientation.up, out RaycastHit hit, checkDistance, trapPositioningMask))
                    {
                        Debug.Log("Found");
                    }

                    /*if (Physics.Raycast(fullCast, -orientation.up, out RaycastHit hit, tilingLayer))
                    {
                        if (hit.collider.TryGetComponent(out Tile tile) && !tile.IsUsed)
                            _reachedTiles.Add(tile);
                        else
                            placeTrap = false;
                    }
                    else
                        placeTrap = false;*/
                }
            }

            SetTrapPosition();
        }
        #endregion

        #region Trap Positioning
        /// <summary>
        /// Set the trap position on the grid
        /// </summary>
        private void SetTrapPosition()
        {
            Vector3 Xpos = orientation.right * ((1 - X % 2) * tiling.lengthX * 0.5f);
            Vector3 Ypos = orientation.forward * ((1 - Y % 2) * tiling.lengthY * 0.5f);
            Vector3 trapPos = _hittedTile.position + (Xpos + Ypos);

            mark.position = trapPos;
        }
        #endregion

        #endregion
    }
}
