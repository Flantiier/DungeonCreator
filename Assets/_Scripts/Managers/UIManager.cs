using UnityEngine;

namespace _Scripts.Managers
{
	public class UIManager : MonoBehaviour
	{
		#region Variables
		[Header("UI references")]
		[SerializeField] private GameObject gameHUD;
		#endregion

		#region Builts_In
		private void Awake()
		{
			InstantiateHUDs();
		}
		#endregion

		#region Methods
		private void InstantiateHUDs()
		{
			Instantiate(gameHUD);
		}
		#endregion
	}
}
