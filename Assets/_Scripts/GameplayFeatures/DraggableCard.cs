using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
    public class DraggableCard : DraggableUIElement
    {
        #region Variables/Properties
        [TitleGroup("References")]
        [SerializeField] private GameObject card;
        [TitleGroup("References")]
        [SerializeField] private GameObject cardMask;
        [TitleGroup("References")]
        [SerializeField] private CardDesign design;
        [TitleGroup("References")]
        [SerializeField] private TrapSO trapReference;

        [TitleGroup("Variables_Events")]
        [SerializeField] private FloatVariable mana;
        [TitleGroup("Variables_Events")]
        [SerializeField] private GameEvent dragEvent, dropEvent;

        private Image _raycaster;
        public TrapSO TrapReference => trapReference;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            _raycaster = GetComponent<Image>();
        }

        public void Start()
        {
            if(trapReference)
                UpdateCardInformations(TrapReference);
        }

        private void LateUpdate()
        {
            UpdateDraggedState();
        }
        #endregion

        #region Interfaces Implementation
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanBeDragged)
                return;
            //Drag
            base.OnBeginDrag(eventData);
            _raycaster.raycastTarget = false;

            //DRAG EVENT CALL
            DMController.SelectedCard = this;
            dragEvent.Raise();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (!IsDragged)
                return;

            //DROP EVENT CALL
            dropEvent.Raise();

            //Drop
            base.OnEndDrag(eventData);
            card.SetActive(true);
            _raycaster.raycastTarget = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Update the different informations on the card
        /// </summary>
        /// <param name="reference"> Trap reference </param>
        public void UpdateCardInformations(TrapSO reference)
        {
            if (!reference)
            {
                Debug.LogWarning("Missing trap reference");
                return;
            }

            trapReference = reference;
            design.imageField.sprite = reference.image;
            design.nameField.SetText(reference.trapName + " :");
            design.descriptionField.SetText(reference.description + $" ({reference.xAmount}x{reference.yAmount})");
            //design.damageField.SetText(reference.damages.ToString());
            design.manaField.SetText(reference.manaCost.ToString());
        }

        /// <summary>
        /// Disable the drag and drop when the the mana is too low
        /// </summary>
        private void UpdateDraggedState()
        {
            if (!trapReference)
            {
                CanBeDragged = false;
                return;
            }
            else if (!mana)
            {
                CanBeDragged = true;
                return;
            }

            CanBeDragged = mana.value >= trapReference.manaCost;
            cardMask.SetActive(!CanBeDragged);
        }

        /// <summary>
        /// Enable card display
        /// </summary>
        public void EnableCard()
        {
            if (!IsDragged)
                return;

            card.SetActive(true);
        }

        /// <summary>
        /// Disable card display
        /// </summary>
        public void DisableCard()
        {
            if (!IsDragged)
                return;

            card.SetActive(false);
        }
        #endregion
    }
}

#region CardDesign_Class
[System.Serializable]
public struct CardDesign
{
    public Image imageField;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI damageField;
    public TextMeshProUGUI manaField;
}
#endregion
