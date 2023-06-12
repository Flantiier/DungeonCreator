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

        public static bool CursorOnZone { get; set; }
        #endregion

        #region Interfaces Implementations
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            CursorOnZone = true;

            if (!DMController.IsDragging)
                return;

            pointerEnterEvent.Raise();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            CursorOnZone = false;

            if (!DMController.IsDragging)
                return;

            pointerExitEvent.Raise();
        }
        #endregion
    }
}
