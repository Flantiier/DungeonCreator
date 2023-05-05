using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;
using UnityEngine.InputSystem;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "CardsDatabase", menuName = "Game Management/CardsDatabase"), InlineEditor]
    public class CardsDatabase : ScriptableObject
    {
        public int deckSize = 8;
        public DataBaseElement[] elements = new DataBaseElement[8];

        public TrapSO GetCardByKey(string key)
        {
            foreach (DataBaseElement item in elements)
            {
                if (item.key != key)
                    continue;
                else
                    return item.card;
            }

            return null;
        }

        public string GetKeyByCard(TrapSO card)
        {
            foreach (DataBaseElement item in elements)
            {
                if (item.card != card)
                    continue;
                else
                    return item.key;
            }

            return null;
        }
    }

    [System.Serializable]
    public struct DataBaseElement
    {
        public TrapSO card;
        public int maxAmount;
        public string key;
    }
}