using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Menus
{
	public class DeckPresetButton : MonoBehaviour
	{
        #region Variables
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private Sprite baseBordure;
		[SerializeField] private Sprite selectBordure;
        #endregion

        #region Properties
        public Button Button => button;
        #endregion

        #region Methods
        public void IsSelected(bool selected)
        {
            if (!image)
                return;

            Sprite sprite = selected ? selectBordure : baseBordure;
            image.sprite = sprite;
        }
        #endregion
    }
}
