using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Gameplay
{
	public class AbilityGUI : MonoBehaviour
	{
        #region Variables
        private Image _image;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        #endregion

        #region Methods
        #endregion
    }
}
