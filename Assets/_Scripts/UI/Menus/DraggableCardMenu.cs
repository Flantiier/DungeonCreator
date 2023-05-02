using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI.Menus
{
    public class DraggableCardMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        #region Variables
        private CanvasGroup _canvasGroup;
        private bool _swaped;
        public RectTransform RectTransform { get; private set; }
        [Sirenix.OdinInspector.ShowInInspector] public CardSlot OccupedSlot { get; set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            RectTransform = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        #endregion

        #region Drag&Drop
        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out DraggableCardMenu card))
                return;

            _swaped = true;
            SwapCards(card);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (_swaped)
                _swaped = false;
            else
                RectTransform.position = OccupedSlot.transform.position;
        }
        #endregion

        #region Methods
        private void SwapCards(DraggableCardMenu card)
        {
            //1 => Store the two slots of these cards
            CardSlot targetSlot = OccupedSlot;
            CardSlot currentSlot = card.OccupedSlot;

            //2 => Move this one to the other one
            targetSlot.SetCardInSlot(card);
            card.RectTransform.position = targetSlot.transform.position;

            //3 => Move the other one with the stored values
            RectTransform.position = currentSlot.transform.position;
            currentSlot.SetCardInSlot(this);
        }
        #endregion
    }
}