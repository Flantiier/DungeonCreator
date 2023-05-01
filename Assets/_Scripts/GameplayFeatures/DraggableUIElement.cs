using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayFeatures
{
    public class DraggableUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Variables/Properties
        protected RectTransform _rectTransform;
        protected Vector2 _anchoredPoint;
        #endregion

        #region Properties
        public bool IsDragged { get; protected set; }
        public bool CanBeDragged { get; protected set; } = true;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _rectTransform = transform as RectTransform;
        }
        #endregion

        #region Drag/Drop Interfaces
        //Begin drag
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanBeDragged)
                return;

            //Start Drag
            BeingDrag(true);
            _anchoredPoint = _rectTransform.anchoredPosition;
        }

        //Dragging
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;

            transform.position = eventData.position;
        }

        //End drag
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;

            _rectTransform.anchoredPosition = _anchoredPoint;
            BeingDrag(false);
        }
        #endregion

        #region Drag Methods
        /// <summary>
        /// Indicates if this UI element is dragged1
        /// </summary>
        protected void BeingDrag(bool dragged)
        {
            IsDragged = dragged;
        }
        #endregion
    }
}
