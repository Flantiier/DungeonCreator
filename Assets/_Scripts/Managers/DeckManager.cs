using System.Collections.Generic;
using UnityEngine;
using _ScriptablesObjects.Traps;
using _ScriptablesObjects._UserDatas;

namespace _Scripts.Managers
{
    public class DeckManager : MonoBehaviourSingleton<DeckManager>
    {
        #region Variables
        [Header("References")]
        [SerializeField] private UserDatas userDatas;

        private List<TrapSO> _cards = new List<TrapSO>();
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            //Create an shuffled instance of the current deck used
            _cards = NewDeckInstance<TrapSO>(userDatas.currentDeck.cards);
        }
        #endregion

        #region Methods
        [ContextMenu("Shuffle")]
        private void DebugList()
        {
            foreach (var item in userDatas.currentDeck.cards)
                Debug.Log(item.name + '\n');

            Debug.Log("---------------------------------");

            List<TrapSO> list = NewDeckInstance<TrapSO>(userDatas.currentDeck.cards);

            foreach (var item in list)
                Debug.Log(item.name + '\n');

            Debug.Log("---------------------------------");
        }

        /// <summary>
        /// Creates a new instance of the deck profile and shuffle it
        /// </summary>
        private List<T> NewDeckInstance<T>(List<T> baseList)
        {
            List<T> list = new List<T>(baseList);

            for (int i = 0; i < list.Count - 1; i++)
            {
                T temp = list[i];
                int random = Random.Range(1, list.Count);
                list[i] = list[random];
                list[random] = temp;
            }

            return list;
        }
        #endregion
    }
}