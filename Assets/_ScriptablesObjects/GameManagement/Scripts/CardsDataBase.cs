using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "CardsDataBase", menuName = "Game Management/CardsDataBase"), InlineEditor]
    public class CardsDataBase : ScriptableObject
    {
        public int deckLength = 8;
        public DataBaseElement[] elements = new DataBaseElement[8];
    }

    [System.Serializable]
    public struct DataBaseElement
    {
        public TrapSO card;
        public int maxAmount;
    }
}