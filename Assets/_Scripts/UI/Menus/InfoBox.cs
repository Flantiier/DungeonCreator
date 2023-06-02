using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI.Menus
{
	public class InfoBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        #region Variables
        [SerializeField] private PlayerList list;
        [SerializeField] private GameObject[] textPanels;

        private Image _image;
        private GameObject _currentPanel;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            SelectCharacterInfos();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Select the right panel to show
        /// </summary>
        private void SelectCharacterInfos()
        {
            _image.enabled = list.CurrentCharacter > 0;

            switch (list.CurrentCharacter)
            {
                //Warrior infos
                case 1:
                    _currentPanel = textPanels[0];
                    break;
                //Archer infos
                case 2:
                    _currentPanel = textPanels[1];
                    break;
                //Wizard Infos
                case 3:
                    _currentPanel = textPanels[2];
                    break;
                //DM infos
                case 4:
                    _currentPanel = textPanels[3];
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Enter");

            if (!_currentPanel)
                return;

            _currentPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Exit");

            if (!_currentPanel)
                return;

            _currentPanel.SetActive(false);
        }
        #endregion
    }
}
