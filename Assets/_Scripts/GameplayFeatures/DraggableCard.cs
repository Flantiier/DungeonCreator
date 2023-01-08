using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.GameplayFeatures
{
	public class DraggableCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] private GameObject obj;

		private RectTransform _rectTransform;
		private Image _image;
		private Vector3 _previousPosition;

		private void Awake()
		{
			_rectTransform = transform as RectTransform;
			_image = GetComponent<Image>();
		}

		private void OnEnable()
		{
			_previousPosition = _rectTransform.anchoredPosition;
        }

		public void EnableCard(bool enabled)
		{
			_image.enabled = enabled;
		}

        public void OnBeginDrag(PointerEventData eventData)
		{
			DragAndDropTest.Instance.IsDragging = true;
            DragAndDropTest.Instance.NewObjectSelected(this, obj);

			_image.raycastTarget = false;
		}

		public void OnDrag(PointerEventData eventData)
		{
			transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
			_image.raycastTarget = true;

            DragAndDropTest.Instance.IsDragging = false;
			_rectTransform.anchoredPosition = _previousPosition;
        }
    }
}
