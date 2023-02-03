using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using _Scripts.Characters.DungeonMaster;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures
{
    public class DraggableCard : DraggableUIElement
    {
        #region Variables/Properties
        [FoldoutGroup("References")]
        [SerializeField] private GameObject card;
        [FoldoutGroup("References")]
        [SerializeField] private CardDesign design;
        [FoldoutGroup("References")]
        [SerializeField] private TrapSO trapReference;

        [TitleGroup("Helpers")]
        [SerializeField] private bool updateOnStart = false;

        private Image _raycaster;
        public TrapSO TrapReference => trapReference;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            _raycaster = GetComponent<Image>();

            if (!updateOnStart)
                return;

            //Update card's informations
            UpdateCardInformations(TrapReference);
        }

        private void OnEnable()
        {
            PointerZone.OnEnterPointerZone += EnableCard;
            PointerZone.OnExitPointerZone += DisableCard;
        }

        private void OnDisable()
        {
            PointerZone.OnEnterPointerZone -= EnableCard;
            PointerZone.OnExitPointerZone -= DisableCard;
        }
        #endregion

        #region DragAndDropInterfaces
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            _raycaster.raycastTarget = false;

            DMController_Test.Instance.StartDrag(this);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            DMController_Test.Instance.EndDrag();

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
        /// Enable card display
        /// </summary>
        private void EnableCard()
        {
            if (!IsDragged)
                return;

            card.SetActive(true);
        }

        /// <summary>
        /// Disable card display
        /// </summary>
        private void DisableCard()
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
