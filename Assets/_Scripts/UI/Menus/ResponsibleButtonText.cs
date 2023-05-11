using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI.Menus
{
	public class ResponsibleButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        [Header("Text properties")]
		[SerializeField] private TextMeshProUGUI textMeshPro;
		[SerializeField] private Color baseColor = Color.white, highlightColor = Color.white;

        private void Awake()
        {
            textMeshPro.color = baseColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            textMeshPro.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            textMeshPro.color = baseColor;
        }
    }
}
