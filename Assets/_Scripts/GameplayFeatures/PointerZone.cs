using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayFeatures
{
	public class PointerZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
	{
		public static Action OnEnterPointerZone;
		public static Action OnExitPointerZone;
        public Vector2 position;

        public void OnPointerEnter(PointerEventData eventData)
		{
            Debug.Log("Enter");
            OnEnterPointerZone?.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Exit");
            OnExitPointerZone?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            position = eventData.position;
        }
    }
}
