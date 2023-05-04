using UnityEngine;
using _ScriptableObjects.Traps;

namespace _Scripts.UI.Menus
{
	public class CardGUI : MonoBehaviour
	{
        #region Variables/Properties
        [SerializeField] private CardDesign design;
        protected CanvasGroup _group;
        public TrapSO Trap { get; private set; }
        #endregion

        #region Builts_In
        protected virtual void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }
        #endregion

        #region Methods
        public void UpdateInfos(TrapSO reference)
        {
            if (!reference)
                return;

            Trap = reference;
            design.imageField.sprite = reference.image;
            design.nameField.SetText(reference.trapName);
            design.damageField.SetText(reference.damages.ToString());
            design.manaField.SetText(reference.manaCost.ToString());
        }
        #endregion
    }
}
