using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
    public class DraggableUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables/Properties
        [TitleGroup("UI Elements")]
        [SerializeField] protected float selectedOffset = 70f;

        protected RectTransform _rectTransform;
        protected Vector2 _previousPosition;
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

            //Reset Card selection
            SetPointerState(false);
            //Start Drag
            BeingDrag(true);
            SetAnchoredPosition(_rectTransform.anchoredPosition);
        }

        //Dragging
        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;

            FollowPointer(eventData.position);
        }

        //End drag
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;

            BeingDrag(false);
            ResetToPreviousPosition();
        }
        #endregion

        #region Pointer Interfaces
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            SetPointerState(true);
            Debug.LogWarning("enter");
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            SetPointerState(false);
            Debug.LogWarning("exit");
        }

        /// <summary>
        /// Indicates if the pointer is on/off the object
        /// </summary>
        /// <param name="state"></param>
        protected void SetPointerState(bool state)
        {

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
