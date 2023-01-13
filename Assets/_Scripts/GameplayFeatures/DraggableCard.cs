using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using _Scripts.TrapSystem.Datas;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
	public class DraggableCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
        #region Variables
        [SerializeField] private TrapSO trapReference;

		private RectTransform _rectTransform;
		private Image _image;
		private Vector3 _anchoredPosition;
        private bool _isBeingDragged = false;
        #endregion

        #region Properties
        public TrapSO TrapReference => trapReference;
        #endregion

        #region Builts_In
        private void Awake()
		{
			_rectTransform = transform as RectTransform;
			_image = GetComponent<Image>();

            SetCardAnchor(_rectTransform.anchoredPosition);
		}
        #endregion

        #region DragAndDropInterfaces
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeingDragged(true);
            DMController_Test.Instance.StartDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Modify this card position during drag
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DMController_Test.Instance.EndDrag();

            BeingDragged(false);
            SetCardPosition();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable the image component on the card
        /// </summary>
        /// <param name="enabled"> Should be enable or not </param>
        public void EnableCard(bool enabled)
		{
			_image.enabled = enabled;
		}

        /// <summary>
        /// Set the anchored position variable
        /// </summary>
        public void SetCardAnchor(Vector3 anchored)
        {
            _anchoredPosition = anchored;
        }

        /// <summary>
        /// Set thsi obvject anchored position to  the last anchored position set
        /// </summary>
        public void SetCardPosition()
        {
            if (!_rectTransform)
                return;

            _rectTransform.anchoredPosition = _anchoredPosition;
        }

        /// <summary>
        /// Change drag state of this card
        /// </summary>
        private void BeingDragged(bool dragging)
        {
            _isBeingDragged = dragging;

            _image.raycastTarget = !dragging;
        }
        #endregion
    }
}
