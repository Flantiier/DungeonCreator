using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using _Scripts.TrapSystem;
using _Scripts.TrapSystem.Datas;

public class RayShooter : MonoBehaviour
{
    #region References Variables
    [Header("References")]
    [SerializeField, Tooltip("Referencing the playerCam")]
    private Camera mainCam;

    [SerializeField, Tooltip("Projected transform")]
    private Transform projectedTransform;

    //PView comp
    private PhotonView _view;
    //Player Inputs comp
    private PlayerInput _inputs;
    #endregion

    #region RayCasting Variables
    [Header("Tiles Ray Variables")]
    [SerializeField, Tooltip("Tile checking radius")]
    private float checkRadius = 0.25f;

    [SerializeField, Tooltip("Maximum checking distance")]
    private float maxCheckDistance = 2f;

    [SerializeField, Tooltip("Tiles LayerMask")]
    private LayerMask tileMask;

    //Last Tile hitted
    private Transform _lastTileHit;
    //Pivot to check tiles
    private Vector3 _pivot;
    //Trap position
    private Vector3 _trapPosition;
    //Offset Tiling Vector
    private Vector2 _offsetVector;
    //Indicates if the ray touches something
    private bool IsHitting;
    //Shooted ray on tiles
    private Ray _ray;
    private RaycastHit _rayHitting;
    private RaycastHit _hit;
    //List of tiles reached by the trap tiling
    private List<Tile> _reachedTiles = new List<Tile>();
    #endregion

    #region Tiling Infos
    [Header("Game Informations")]
    [SerializeField, Tooltip("Tiling Informations")]
    private TilingSO tilingInfos;

    //SelectedTrap
    public TrapSO trap { get; private set; }

    //Selected TrapTransform
    private Transform trapTransform;
    //Trap Angle
    private float _trapAngle;
    //Indicates if the player can place a trap
    private bool _canPlaceTrap;

    public void SelectingTrap(TrapSO selectedTrap)
    {
        trap = selectedTrap;

        if (trapTransform != null)
            Destroy(trapTransform.gameObject);

        trapTransform = Instantiate(selectedTrap.trapPrefab, Vector3.down * 20f, Quaternion.identity).transform;
    }
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
        //Get inputs
        _inputs = GetComponent<PlayerInput>();
        //Get the tiling infos (width, height, offsets)
        _offsetVector = new Vector3(tilingInfos.tilePrefab.transform.localScale.x, tilingInfos.tilePrefab.transform.localScale.y);
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
        //Not Local
        if (!_view.IsMine)
            return;

        //Local
        //Shoot a ray
        ShootRay();
    }
    #endregion

    #region RayShooting
    /// <summary>
    /// Shooting a ray from the center of the camera to the mouseScreen position
    /// </summary>
    private void ShootRay()
    {
        //Shoot a Ray from the camera
        _ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.cyan);

        //No traps selected
        if (trapTransform == null)
            //return
            return;

        //Trap selected
        //Raycasting the ray
        if (Physics.Raycast(_ray.origin, _ray.direction, out _rayHitting, Mathf.Infinity, tileMask))
        {
            //Hitting somethinf=g
            IsHitting = true;

            //Haven't touched a tile yet
            if (_lastTileHit == null)
                //Get firstTile Transform
                _lastTileHit = _rayHitting.transform;

            //Else If the player the same tile
            else if (_lastTileHit.gameObject == _rayHitting.collider.gameObject)
                //Then return
                return;

            //Touch a new Tile
            //Get tile transform
            _lastTileHit = _rayHitting.collider.transform;
            //Updating tiling informations
            UpdateTiling();
        }

        //Don't touch something
        IsHitting = false;
    }
    #endregion

    #region Tiling Logics Methods
    /// <summary>
    /// Differents steps to place a trap
    /// </summary>
    public void UpdateTiling()
    {
        //Step 1, Définir la position de début pour la vérification du piège");
        GetPivotPosition();

        //Step 2, Pré-posage du piège à sa position");
        SetTrapPosition();

        //Step 3, Mettre en couleur les tiles utilisées");
        ShowRangedTiles();
    }

    /// <summary>
    /// Calculating the pivotPosition
    /// </summary>
    public void GetPivotPosition()
    {
        //Get the bounds of the required tiling
        float pivotX = Mathf.FloorToInt(trap.xAmount / 2) * _offsetVector.x;
        float pivotZ = Mathf.FloorToInt(trap.yAmount / 2) * _offsetVector.y;

        //Projecting the vector on the hitted surface
        projectedTransform.position = _rayHitting.transform.position;

        //Comparing the normal vectors to get the right Quaternion
        if(_rayHitting.normal == Vector3.up)
            projectedTransform.rotation = Quaternion.Euler(0f, _trapAngle, 0f);
        else if(_rayHitting.normal == -Vector3.forward)
            projectedTransform.rotation = Quaternion.Euler(_trapAngle, -90f, 90f);
        else if (_rayHitting.normal == Vector3.forward)
            projectedTransform.rotation = Quaternion.Euler(_trapAngle, 90f, 90f);
        else if (_rayHitting.normal == Vector3.right)
            projectedTransform.rotation = Quaternion.Euler(_trapAngle, 0f, -90f);
        else
            projectedTransform.rotation = Quaternion.Euler(_trapAngle, 0f, 90f);

        //Setting the pivot based on the projected trasnform
        _pivot = projectedTransform.position + projectedTransform.right * pivotX + projectedTransform.forward * pivotZ;
        //Setting Trap rotation => same as the projected rotation
        trapTransform.rotation = projectedTransform.rotation;
    }

    /// <summary>
    /// Place the Trap position in the center of the tiling
    /// </summary>
    public void SetTrapPosition()
    {
        _trapPosition = _pivot - projectedTransform.right * ((trap.xAmount - 1f) * _offsetVector.x * 0.5f) - projectedTransform.forward * ((trap.yAmount - 1f) * _offsetVector.y * 0.5f);
        trapTransform.position = _trapPosition;
    }

    /// <summary>
    /// Showing Trap Tiling
    /// </summary>
    private void ShowRangedTiles()
    {
        //Clear Tiles List
        ResetTilesList();
        bool placeTrap = true;

        //For trap xAmount and yAmount of tiles, Displaying the tile like selected
        for (int i = 0; i < trap.xAmount; i++)
        {
            for (int j = 0; j < trap.yAmount; j++)
            {
                //Defining a Vector to check the tile position
                Vector3 castOrigin = _pivot - projectedTransform.right * _offsetVector.x * i - projectedTransform.forward * _offsetVector.y * j + trapTransform.up;

                //If you hit a tile
                if (Physics.SphereCast(castOrigin, checkRadius, -trapTransform.up , out _hit, maxCheckDistance, tileMask))
                {
                    //Its not currently used by a trap
                    if (_hit.collider.TryGetComponent(out Tile tile) && !tile.IsUsed)
                        //Add this tile in the reached Tiles List
                        _reachedTiles.Add(tile);
                    //Currently using a trap
                    else
                        placeTrap = false;
                }
                //No tiles detected
                else
                    placeTrap = false;
            }
        }

        //If you can't place a trap
        if (!placeTrap)
            //Waiting State
            ChangeTilesStates(Tile.TileState.Waiting);
        //If you can
        else
            //Selected State
            ChangeTilesStates(Tile.TileState.Selected);

        _canPlaceTrap = placeTrap;
    }
    #endregion

    #region Tiles Selection
    /// <summary>
    /// Changing the state of all the tiles in the reachedList
    /// </summary>
    /// <param name="newState">NewState of the tile</param>
    private void ChangeTilesStates(Tile.TileState newState)
    {
        foreach (Tile tile in _reachedTiles)
            tile.NewTileState(newState);
    }

    /// <summary>
    /// Clearing the List when another tile is touched
    /// </summary>
    private void ResetTilesList()
    {
        //Deselecting all the tiles
        foreach (Tile tile in _reachedTiles)
            tile.NewTileState(Tile.TileState.Deselected);

        //Clearing the list
        _reachedTiles.Clear();
    }
    #endregion

    #region Trap Methods
    //Instantiate a trap at the calculated trapPostion
    private void InstantiateTrap(InputAction.CallbackContext ctx)
    {
        if (!_canPlaceTrap || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        //Instantiate the trap
        Instantiate(trap.trapPrefab, _trapPosition, projectedTransform.rotation);
        //Changing the state of the used tiles for this trap
        ChangeTilesStates(Tile.TileState.Used);

        //Clearing the list
        _reachedTiles.Clear();
    }

    /// <summary>
    /// Rotating the trap clockwise (angle between 0 and 360)
    /// </summary>
    public void RotateClockwise(InputAction.CallbackContext ctx)
    {
        if (!IsHitting)
            return;

        //Getting the angle in radiant then convert in degrees (dividing by 2Pi to substract the useless turns)
        _trapAngle = Mathf.RoundToInt((((_trapAngle + 90) * Mathf.Deg2Rad) % (2 * Mathf.PI)) * Mathf.Rad2Deg);

        UpdateTiling();
    }

    /// <summary>
    /// Rotating the trap antiClockwise (angle between -360 and 0)
    /// </summary>
    public void RotateAntiClockwise(InputAction.CallbackContext ctx)
    {
        if (!IsHitting)
            return;

        //Getting the angle in radiant then convert in degrees (dividing by 2Pi to substract the useless turns)
        _trapAngle = Mathf.RoundToInt((((_trapAngle - 90) * Mathf.Deg2Rad) % (2 * Mathf.PI)) * Mathf.Rad2Deg);

        UpdateTiling();
    }
    #endregion

    #region Events Subscribing
    private void SubscribeToInputs()
    {
        //Activate inputs
        _inputs.ActivateInput();

        //Subscribe PlaceTrap method on interact button
        _inputs.actions["Interact"].performed += InstantiateTrap;

        //Subscribe RotateClockwise method on RotateCW button
        _inputs.actions["RotateCW"].performed += RotateClockwise;

        //Subscribe RotateAntiClockwise method on RotateACW button
        _inputs.actions["RotateACW"].performed += RotateAntiClockwise;
    }

    private void UnsubscribeToInputs()
    {
        //Deactivate inputs
        _inputs.DeactivateInput();

        //Unsubscribe PlaceTrap method on interact button
        _inputs.actions["Interact"].performed -= InstantiateTrap;

        //Unsubscribe RotateClockwise method on RotateCW button
        _inputs.actions["RotateCW"].performed -= RotateClockwise;

        //Unsubscribe RotateAntiClockwise method on RotateACW button
        _inputs.actions["RotateACW"].performed -= RotateAntiClockwise;
    }
    #endregion
}
