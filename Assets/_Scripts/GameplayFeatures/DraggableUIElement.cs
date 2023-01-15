using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayFeatures
{
    public class DraggableUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Variables/Properties
        protected RectTransform _rectTransform;
        protected Vector2 _previousPosition;
        public bool IsDragged { get; protected set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _rectTransform = transform as RectTransform;
        }
        #endregion

        #region Interfaces Implementation
        //Begin drag interface
        public virtual void OnBeginDrag(PointerEventData eventData)
        { 
            BeingDrag(true);
            SetAnchoredPosition(_rectTransform.anchoredPosition);
        }

        //Dragging interface
        public virtual void OnDrag(PointerEventData eventData)
        { 
            FollowPointer(eventData.position);
        }

        //End drag interface
        public virtual void OnEndDrag(PointerEventData eventData)
        { 
            BeingDrag(false);
            ResetToPreviousPosition();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if this UI element is dragged1
        /// </summary>
        protected void BeingDrag(bool dragged)
        {
            IsDragged = dragged;
        }

        /// <summary>
        /// SMoothly follow the mouse
        /// </summary>
        protected void FollowPointer(Vector2 position)
        {
            transform.position = position;
        }

        /// <summary>
        /// Set the parent of this element
        /// </summary>
        public void SetElementParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        /// <summary>
        /// Set the last position of this UI element
        /// </summary>
        public void SetAnchoredPosition(Vector2 position)
        {
            _previousPosition = position;
        }

        /// <summary>
        /// Reset the position of this UI element to the previous position sets
        /// </summary>
        public void ResetToPreviousPosition()
        {
            _rectTransform.anchoredPosition = _previousPosition;
        }
        #endregion
    }
}
