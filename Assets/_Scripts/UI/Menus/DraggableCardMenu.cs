using UnityEngine;
using UnityEngine.EventSystems;
using _ScriptableObjects.Traps;

namespace _Scripts.UI.Menus
{
    public class DraggableCardMenu : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
    {
        #region Variables/Properties
        [SerializeField] private float dragAlpha = 0.75f;
        [SerializeField] private CardDesign design;
        private CanvasGroup _canvasGroup;

        public RectTransform RectTransform { get; private set; }
        [Sirenix.OdinInspector.ShowInInspector] public TrapSO TrapReference { get; set; }
        [Sirenix.OdinInspector.ShowInInspector] public Transform OccupedSlot { get; set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            RectTransform = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        #endregion

        #region Drag&Drop
        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
            _canvasGroup.alpha = dragAlpha;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out DraggableCardMenu card) || !OccupedSlot)
                return;

            SwapCards(card);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            RectTransform.position = OccupedSlot.transform.position;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update card design (Name, image, etc)
        /// </summary>
        public void UpdateCardInfos(TrapSO reference)
        {
            if (!reference)
                return;

            TrapReference = reference;
            design.imageField.sprite = reference.image;
            design.nameField.SetText(reference.trapName);
            design.damageField.SetText(reference.damages.ToString());
            design.manaField.SetText(reference.manaCost.ToString());
        }

        private void SwapCards(DraggableCardMenu card)
        {
            //1 => Store the two slots of these cards
            Transform targetSlot = OccupedSlot;
            Transform currentSlot = card.OccupedSlot;

            //2 => Swap positions
            card.RectTransform.position = targetSlot.transform.position;
            RectTransform.position = currentSlot.transform.position;
        }
        #endregion
    }
}