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
        private int _currentAmount;

        //Indicates if its a pool slot or not
        public bool IsPoolSlot { get; set; }
        public int CurrentAmount
        {
            get => _currentAmount;
            set
            {
                _currentAmount = value;
                //Update text
                UpdateText();
                //Update mask
                if (CurrentAmount <= 0 && canvasGroup.blocksRaycasts)
                    MaskCard(true);
                else if (CurrentAmount >= 1 && !canvasGroup.blocksRaycasts)
                    MaskCard(false);
            }
        }
        #endregion

        #region Builts_In
        protected void Awake()
        {
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
            _currentAmount = amount;
        }

        /// <summary>
        /// Remove a card from the current amount
        /// </summary>
        public void DecreaseCardAmount()
        {
            if (!IsPoolSlot || CurrentAmount <= 0)
                return;

            CurrentAmount--;
        }

        /// <summary>
        /// Add a card to the current amount
        /// </summary>
        public void IncreaseCardAmount()
        {
            if (!IsPoolSlot || CurrentAmount >= _maxAmount)
                return;

            CurrentAmount++;
        }

        /// <summary>
        /// Set the current amount to the max
        /// </summary>
        public void ResetCardAmount()
        {
            CurrentAmount = _maxAmount;
        }
        
        /// <summary>
        /// Indicates if the card can be dragged and enable or disable the mask based on given value
        /// </summary>
        private void MaskCard(bool state)
        {
            canvasGroup.blocksRaycasts = !state;
            mask.gameObject.SetActive(state);
        }

        /// <summary>
        /// Update the text that shows the current amount of cards available
        /// </summary>
        private void UpdateText()
        {
            amountField.SetText($"x{CurrentAmount}");
        }

        /// <summary>
        /// Disable amount field (same prefab used)
        /// </summary>
        public void DisableAmountField()
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
            if (!eventData.pointerDrag.TryGetComponent(out CardSlot slot) || (IsPoolSlot && slot.IsPoolSlot))
                return;

            DeckMenuHandler.InvokeSwappingEvent(slot, this);
        }

        //End drag
        public void OnEndDrag(PointerEventData eventData) { DeckMenuHandler.InvokeEndDrag(); }
        #endregion
    }
}
