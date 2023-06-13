using UnityEngine;

namespace _Scripts.UI.Menus
{
	public class CharacterSelection : MonoBehaviour
	{
        #region Variables
        [SerializeField] private GameObject[] panels;
        private GameObject _lastPanel;
        #endregion

        #region Builts_In
        private void OnEnable()
        {
            PlayerList.OnLocalRoleChanged += SetCharacterInfos;
        }

        private void OnDisable()
        {
            PlayerList.OnLocalRoleChanged -= SetCharacterInfos;
        }
        #endregion

        #region Methods
        private void SetCharacterInfos(Role role)
        {
            if (_lastPanel)
                _lastPanel.SetActive(false);

            int index = (int)role - 1;

            if (index < 0)
                return;

            _lastPanel = panels[index];
            _lastPanel.SetActive(true);
        }
		#endregion
	}
}
