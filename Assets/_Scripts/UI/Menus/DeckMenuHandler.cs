using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
//
using Utils;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.UserDatas;
using _ScriptableObjects.Traps;
using ExitGames.Client.Photon;

namespace _Scripts.UI.Menus
{
    public class DeckMenuHandler : MonoBehaviour
    {
        #region Variables
        [Header("Datas")]
        [SerializeField] private CardsDataBase dataBase;
        [SerializeField] private DeckProflieSO deck;

        [TitleGroup("Cards")]
        [SerializeField] private DraggableCardMenu cardPrefab;
        [SerializeField] private Transform cardsPool;

        [TitleGroup("Slots")]
        [SerializeField] private CardSlot slotPrefab;
        [SerializeField] private Transform deckSlots;
        [SerializeField] private Transform slots;
        #endregion

        #region Properties
        public HashSet<DraggableCardMenu> Pool { get; set; } = new HashSet<DraggableCardMenu>();
        #endregion

        #region Builts_In
        private void Start()
        {
            InstantiateCards();
            ArrangeDeck(deck);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Instantiate all the references cards from the dataBase
        /// </summary>
        private void InstantiateCards()
        {
            if (dataBase.cards.Length <= 0 || !cardPrefab)
                return;

            for (int i = 0; i < dataBase.cards.Length; i++)
            {
                TrapSO trap = dataBase.cards[i];

                //Missing trap error
                if (!trap)
                {
                    Debug.LogError("Missing card");
                    continue;
                }

                DraggableCardMenu instance = CreateCard(trap);
                Pool.Add(instance);
            }
        }

        /// <summary>
        /// Arrange cards in slot based on a given deck profile
        /// </summary>
        private void ArrangeDeck(DeckProflieSO deck)
        {
            if (!deck)
            {
                Debug.LogWarning("Missing deck reference");
                return;
            }

            HashSet<DraggableCardMenu> tempDeck = new HashSet<DraggableCardMenu>(dataBase.deckLength);
            HashSet<DraggableCardMenu> temp = new HashSet<DraggableCardMenu>(dataBase.cards.Length - deck.cards.Length);

            //Arrange in deck and pool
            foreach (DraggableCardMenu item in Pool)
            {
                Debug.Log(deck.ContainsCard(item.TrapReference));

                if (deck.ContainsCard(item.TrapReference))
                    tempDeck.Add(item);
                else
                    temp.Add(item);
            }

            ArrangeInSlots(tempDeck, deckSlots);
            ArrangeInSlots(temp, slots);
        }

        /// <summary>
        /// Arrange some cards in slots from a group
        /// </summary>
        private void ArrangeInSlots(HashSet<DraggableCardMenu> collection, Transform group)
        {
            for (int i = 0; i < group.childCount; i++)
            {
                Transform child = group.GetChild(i);
                DraggableCardMenu card = collection.ElementAt(i);

                card.transform.position = child.position;
                child.GetComponent<CardSlot>().SetCardInSlot(card);
            }
        }

        /// <summary>
        /// Instantiate a new Card
        /// </summary>
        private DraggableCardMenu CreateCard(TrapSO trap)
        {
            DraggableCardMenu card = Instantiate(cardPrefab, cardsPool);
            card.UpdateCardInfos(trap);
            return card;
        }
        #endregion

        #region Editor
#if UNITY_EDITOR
        [Button("Clean Slots")]
        private void ClearChilds()
        {
            Utilities.DestroyAllChildren(slots);
            Utilities.DestroyAllChildren(deckSlots);
        }

        [Button("Update Slots")]
        private void CreateSlots()
        {
            for (int i = 0; i < dataBase.cards.Length; i++)
            {
                Object instance = PrefabUtility.InstantiatePrefab(slotPrefab);

                if (i <= dataBase.deckLength - 1)
                {
                    instance.GetComponent<Transform>().SetParent(deckSlots);
                    instance.name += $"_{deckSlots.childCount}";
                }
                else
                {
                    instance.GetComponent<Transform>().SetParent(slots);
                    instance.name += $"_{slots.childCount}";
                }

                //Reset scale
                instance.GetComponent<Transform>().localScale = Vector3.one;
            }
        }
#endif
        #endregion
    }
}
