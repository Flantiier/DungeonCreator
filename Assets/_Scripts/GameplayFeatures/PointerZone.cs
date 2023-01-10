using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.GameplayFeatures
{
	public class PointerZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
	{
		public Action OnEnterPointerZone;
		public Action OnExitPointerZone;
        public Vector2 position;

        public void OnPointerEnter(PointerEventData eventData)
		{
            OnEnterPointerZone?.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData)
        {
            OnExitPointerZone?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            position = eventData.position;
        }
    }
}
