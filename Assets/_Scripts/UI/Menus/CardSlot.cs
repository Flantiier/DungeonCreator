using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.UI.Menus
{
    public class CardSlot : MonoBehaviour
    {
        [ShowInInspector]
        public DraggableCardMenu CardInSlot { get; set; }

        public void SetCardInSlot(DraggableCardMenu card)
        {
            CardInSlot = card;
            card.OccupedSlot = this;
        }
    }
}
