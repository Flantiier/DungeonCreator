using UnityEngine;
using TMPro;

namespace _Scripts.UI.Gameplay
{
	public class DisplayText : MonoBehaviour
	{
        #region Variables
        [Header("Display properties")]
        [SerializeField] protected TextMeshProUGUI textMesh;
        #endregion

        #region Methods
        public virtual void UpdateText()
        {
            if (!textMesh)
                return;

            textMesh.SetText("");
        }
        #endregion
    }
}
