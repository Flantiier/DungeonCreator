using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI.Menus
{
    public class CardSlot : CardGUI, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        #region Variables/Properties
        [SerializeField] private GameObject mask;
        [SerializeField] private TextMeshProUGUI amountField;
        private int _maxAmount;

        public int CurrentAmount { get; private set; }
        public bool PoolSlot { get; set; }
        #endregion

        #region Builts_In
        protected override void Awake()
        {
            base.Awake();
            mask.gameObject.SetActive(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the max amount of cards that can be dragged in deck
        /// </summary>
        /// <param name="amount"> Max amount </param>
        public void SetCardsAmount(int amount)
        {
            _maxAmount = amount;
            CurrentAmount = amount;
            UpdateAmountText();
        }

        /// <summary>
        /// Remove a card from the current amount
        /// </summary>
        public void DecreaseCardAmount()
        {
            if (!PoolSlot || CurrentAmount <= 0)
                return;

            CurrentAmount--;
            UpdateAmountText();

            if (CurrentAmount <= 0 && _group.blocksRaycasts)
                MaskCard(true);
        }

        /// <summary>
        /// Add a card to the current amount
        /// </summary>
        public void IncreaseCardAmount()
        {
            if (!PoolSlot || CurrentAmount >= _maxAmount)
                return;

            CurrentAmount++;
            UpdateAmountText();

            if (CurrentAmount >= 1 && !_group.blocksRaycasts)
                MaskCard(false);
        }

        /// <summary>
        /// Set the current amount to the max
        /// </summary>
        public void ResetCardAmount()
        {
            CurrentAmount = _maxAmount;
            MaskCard(false);
            UpdateAmountText();
        }
        
        /// <summary>
        /// Indicates if the card can be dragged and enable or disable the mask based on given value
        /// </summary>
        private void MaskCard(bool state)
        {
            _group.blocksRaycasts = !state;
            mask.gameObject.SetActive(state);
        }

        /// <summary>
        /// Update the text that shows the current amount of cards available
        /// </summary>
        private void UpdateAmountText()
        {
            amountField.SetText($"x{CurrentAmount}");
        }

        public void DisableGUI()
        {
            amountField.transform.parent.gameObject.SetActive(false);
        }
        #endregion

        #region Interfaces Methods
        //Update GUI infos
        public void OnPointerDown(PointerEventData eventData) { DeckMenuHandler.InvokeUpdateGUI(Trap); }

        //Start dragging a card
        public void OnBeginDrag(PointerEventData eventData) { DeckMenuHandler.InvokeStartDrag(Trap); }

        public void OnDrag(PointerEventData eventData) { }

        //Swap cards on drop if possible
        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag.TryGetComponent(out CardSlot slot) || (PoolSlot && slot.PoolSlot))
                return;

            DeckMenuHandler.InvokeSwappingEvent(slot, this);
        }

        //End drag
        public void OnEndDrag(PointerEventData eventData) { DeckMenuHandler.InvokeEndDrag(); }
        #endregion
    }
}
