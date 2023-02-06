using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.GameplayFeatures
{
	public class CardZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
        #region Variables
        public static Action OnEnterPointerZone;
		public static Action OnExitPointerZone;
        #endregion

        #region Interfaces Implementations
        public virtual void OnPointerEnter(PointerEventData eventData)
		{
            if (!DMController.Instance.IsDragging)
                return;

            OnEnterPointerZone?.Invoke();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!DMController.Instance.IsDragging)
                return;

            OnExitPointerZone?.Invoke();
        }
        #endregion
    }
}
