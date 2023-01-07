using UnityEngine;
using InputsMaps;
using _Scripts.NetworkScript;
using _Scripts.Characters.Cameras;

namespace _Scripts.Characters.DungeonMaster
{
	public class DMController_Test : MonoBehaviour
	{
        #region Variables

        #region References
        [Header("References")]
        [SerializeField] private SkyCameraSetup cameraPrefab;

        private InputsDM _inputs;
		private SkyCameraSetup _camSetup;
		#endregion

		#region Motion
		[Header("Motion variables")]
        [SerializeField] private float moveSpeed = 25f;
        [SerializeField] private float rotationSpeed = 100f;
		[Space, SerializeField, Range(0f, 0.2f)] private float smoothingMovements = 0.1f;

        private Vector2 _inputsVector;
		private Vector3 _currentMovement;
		private Vector2 _rotationInputs;
        #endregion

		#endregion

		#region Builts_In
		private void Awake()
		{
			_inputs = new InputsDM();
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

        #endregion
    }
}
