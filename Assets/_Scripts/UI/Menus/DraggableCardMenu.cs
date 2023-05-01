using UnityEngine;
using UnityEngine.EventSystems;
using _Scripts.GameplayFeatures;

namespace _Scripts.UI.Menus
{
	public class DraggableCardMenu : DraggableUIElement
	{
        #region Drag&Drop Methods
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            transform.SetParent(transform.root);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {

        }
        #endregion
    }
}
