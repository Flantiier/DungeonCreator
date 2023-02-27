using UnityEngine;
using UnityEngine.EventSystems;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
	public class CardZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        #region Variables
        [SerializeField] private GameEvent pointerEnterEvent;
        [SerializeField] private GameEvent pointerExitEvent;
        #endregion

        #region Interfaces Implementations
        public virtual void OnPointerEnter(PointerEventData eventData)
		{
            if (!DMController.IsDragging)
                return;

            pointerEnterEvent.Raise();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!DMController.IsDragging)
                return;

            pointerExitEvent.Raise();
        }
        #endregion
    }
}
