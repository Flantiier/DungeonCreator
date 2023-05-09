using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.UI.Menus
{
	public class MenusManager : MonoBehaviour
	{
        #region Variables
        [SerializeField] private int currentIndex = 0;
		[SerializeField] private bool enableCurrentAtStart;
		[SerializeField] private GameObject[] menus;
        #endregion

        #region Builts_In
        private void Awake()
        {
			DisableAll();

			if (enableCurrentAtStart)
			{
				currentIndex = Mathf.Clamp(currentIndex, 0, menus.Length);
				menus[currentIndex].SetActive(true);
            }
        }
        #endregion

        #region Methods
        public void SetCurrentMenu(int menuIndex)
        {
            if (menus.Length <= 0 || menuIndex < 0 || menuIndex >= menus.Length)
                return;

            menus[currentIndex].SetActive(false);
			currentIndex = menuIndex;
            menus[currentIndex].SetActive(true);
        }

        [Button("Disable All")]
        private void DisableAll()
		{
			if (menus.Length <= 0)
				return;

			foreach (GameObject item in menus)
				item.SetActive(false);
		}
        #endregion
    }
}
