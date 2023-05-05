using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using Sirenix.OdinInspector;
//
using Utils;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.Traps;

namespace _Scripts.UI.Menus
{
    public class DeckMenuHandler : MonoBehaviour
    {
        #region Variables
        [Header("Datas")]
        [SerializeField] private CardsDatabase dataBase;
        [SerializeField] private DeckProflieSO deck;

        [FoldoutGroup("GUI")]
        [SerializeField] private DeckMenuGUI GUI;
        [FoldoutGroup("GUI")]
        [SerializeField] private GameObject slotBackground;
        [FoldoutGroup("GUI")]
        [SerializeField] private Transform gridGroup;
        [FoldoutGroup("GUI")]
        [SerializeField] private Transform deckGroup;
        [FoldoutGroup("GUI")]
        [SerializeField] private CardGUI dragCard;

        [FoldoutGroup("Slots")]
        [SerializeField] private CardSlot slotPrefab;
        [FoldoutGroup("Slots")]
        [SerializeField] private Transform slotsPool;
        [FoldoutGroup("Slots")]
        [SerializeField] private Transform deckPool;

        private readonly HashSet<CardSlot> _cardsPool = new HashSet<CardSlot>();
        private readonly HashSet<CardSlot> _deckSlots = new HashSet<CardSlot>();
        #endregion

        #region Events
        public static event Action<TrapSO> OnStartDrag;
        public static event Action OnEndDrag;
        public static event Action<CardSlot, CardSlot> OnSwapCards;
        public static event Action<TrapSO> OnUpdateGUI;
        #endregion

        #region Builts_In
        private void Awake()
        {
            dragCard.gameObject.SetActive(false);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(0.02f);

            InstantiateAllSlots();
            ArrangeAllSlots(deck);
        }

        private void Update()
        {
            if (!dragCard)
                return;

            if (!dragCard.gameObject.activeSelf)
                return;

            dragCard.transform.position = Input.mousePosition;
        }

        private void OnEnable()
        {
            OnStartDrag += StartDragging;
            OnEndDrag += EndDragging;
            OnSwapCards += SwappingCards;
            OnUpdateGUI += UpdateGUI;
        }

        private void OnDisable()
        {
            OnStartDrag -= StartDragging;
            OnEndDrag -= EndDragging;
            OnSwapCards -= SwappingCards;
            OnUpdateGUI -= UpdateGUI;
        }

        private void OnDestroy()
        {
            OverriteDeck();
        }
        #endregion

        #region Methods

        #region Deck Save
        public void NewDeckSelected(DeckProflieSO _deck)
        {
            if (_deck == deck)
                return;

            deck = _deck;
            ArrangeAllSlots(deck);
        }

        /// <summary>
        /// Override the deck with the cards in deck slots
        /// </summary>
        private void OverriteDeck()
        {
            for (int i = 0; i < _deckSlots.Count; i++)
                OverriteCard(i);
        }

        /// <summary>
        /// overrite a card in the deck
        /// </summary>
        private void OverriteCard(int i)
        {
            deck.cards[i] = _deckSlots.ElementAt(i).Trap;
        }
        #endregion

        #region Slots & Cards
        /// <summary>
        /// Instantiate all the references cards from the dataBase
        /// </summary>
        private void InstantiateAllSlots()
        {
            if (dataBase.elements.Length <= 0)
                return;

            //Loop through dataBase to instantiate each card
            for (int i = 0; i < dataBase.elements.Length; i++)
            {
                DataBaseElement element = dataBase.elements[i];

                //Missing trap error
                if (!element.card)
                {
                    Debug.LogError("Missing card");
                    continue;
                }

                CardSlot instance = CreateNewSlot(element.card, slotsPool);
                _cardsPool.Add(instance);
                instance.PoolSlot = true;
                instance.SetCardsAmount(element.maxAmount);
            }

            //Instantiate deck slots based on dataBase values
            for (int i = 0; i < dataBase.deckSize; i++)
            {
                CardSlot slot = CreateNewSlot(null, deckPool);
                _deckSlots.Add(slot);
                slot.DisableGUI();
            }
        }

        private CardSlot CreateNewSlot(TrapSO trap, Transform parent)
        {
            CardSlot instance = Instantiate(slotPrefab, parent);
            instance.UpdateInfos(trap);
            return instance;
        }

        /// <summary>
        /// Set correctly all the slots based on the grid elements
        /// </summary>
        /// <param name="deck"></param>
        private void ArrangeAllSlots(DeckProflieSO deck)
        {
            //Mising deck
            if (!deck)
            {
                Debug.LogWarning("Missing deck");
                return;
            }

            // 1 => Reset et Mettre toutes les cartes dans les slots
            for (int i = 0; i < _cardsPool.Count; i++)
            {
                CardSlot item = _cardsPool.ElementAt(i);
                item.transform.position = gridGroup.GetChild(i).position;
                item.ResetCardAmount();
            }

            // 2 => Mettre les cartes du deck dans les slots deck
            GetDeckCards(deck);
        }

        /// <summary>
        /// Get deck cards in the pool
        /// </summary>
        private void GetDeckCards(DeckProflieSO deck)
        {
            int x = 0;
            foreach (TrapSO SO in deck.cards)
            {
                for (int i = 0; i < _cardsPool.Count; i++)
                {
                    CardSlot slot = _cardsPool.ElementAt(i);
                    //Not the right trap
                    if (slot.Trap != SO)
                        continue;

                    if (slot.CurrentAmount <= 0)
                    {
                        //Looking fro an available trap
                        Debug.LogWarning("A card was took randomly because of multiple cards error");

                        for (int j = _cardsPool.Count - 1; j >= 0; j--)
                        {
                            CardSlot temp = _cardsPool.ElementAt(j);
                            if (temp.CurrentAmount <= 0)
                                continue;

                            slot = temp;
                        }
                    }

                    //Place a deck slot
                    CardSlot deckSlot = _deckSlots.ElementAt(x);
                    deckSlot.transform.position = deckGroup.GetChild(x).position;
                    deckSlot.UpdateInfos(slot.Trap);
                    x++;

                    //Remove a card from the pool
                    slot.DecreaseCardAmount();
                }
            }
        }

        /// <summary>
        /// Swap between two cards (one from the pool, one from the deck)
        /// </summary>
        private void SwappingCards(CardSlot from, CardSlot to)
        {
            // 1 => Trouver qui est le slot deck et qui est le slot pool
            CardSlot tempDeck;
            CardSlot tempPool;

            if (from.PoolSlot) { tempPool = from; tempDeck = to; }
            else { tempPool = to; tempDeck = from; }

            // 2 => Trouve l'element dans la liste pool en reference a la carte pool
            _cardsPool.TryGetValue(tempPool, out CardSlot poolSlot);
            // 3 => Trouve l'element dans la liste deck en reference a la carte deck
            _deckSlots.TryGetValue(tempDeck, out CardSlot deckSlot);
            // 4 => Trouve l'element dans la liste pool en reference a la carte deck
            TrapSO temp = tempDeck.Trap;

            // 5 => Met la carte pool dans deck
            deckSlot.UpdateInfos(tempPool.Trap);
            // 6 => Enleve 1 carte pool dans celles de la pool
            poolSlot.DecreaseCardAmount();
            // 7 => Ajoute une carte deck dans celles de la pool
            foreach (CardSlot item in _cardsPool)
            {
                if (item.Trap != temp)
                    continue;

                item.IncreaseCardAmount();
            }
            // 8 => Change the card in the deck
            for (int k = 0; k < _deckSlots.Count; k++)
            {
                if (_deckSlots.ElementAt(k) != deckSlot)
                    continue;

                OverriteCard(k);
            }
        }
        #endregion

        #region GUI Feeedbacks
        private void StartDragging(TrapSO SO)
        {
            if (!dragCard)
                return;

            dragCard.gameObject.SetActive(true);
            dragCard.transform.position = Input.mousePosition;
            dragCard.UpdateInfos(SO);
        }

        private void EndDragging()
        {
            dragCard.gameObject.SetActive(false);
        }

        private void UpdateGUI(TrapSO reference)
        {
            GUI.design.imageField.sprite = reference.image;
            GUI.design.nameField.SetText(reference.trapName);
            GUI.design.damageField.SetText(reference.damages.ToString());
            GUI.design.manaField.SetText(reference.manaCost.ToString());
            GUI.description.SetText(reference.description);
            GUI.type.SetText(reference.type.ToString());
            GUI.tiling.SetText($"{reference.xAmount} x {reference.yAmount}");
        }
        #endregion

        #region Static Methods
        public static void InvokeStartDrag(TrapSO SO)
        {
            OnStartDrag?.Invoke(SO);
        }

        public static void InvokeEndDrag()
        {
            OnEndDrag?.Invoke();
        }

        public static void InvokeUpdateGUI(TrapSO SO)
        {
            OnUpdateGUI?.Invoke(SO);
        }

        public static void InvokeSwappingEvent(CardSlot from, CardSlot to)
        {
            OnSwapCards?.Invoke(from, to);
        }
        #endregion

        #endregion

        #region Editor
#if UNITY_EDITOR
        [Button("Clean Slots")]
        private void ClearChilds()
        {
            Utilities.DestroyAllChildren(gridGroup);
            Utilities.DestroyAllChildren(deckGroup);
        }

        [Button("Update Slots")]
        private void CreateSlots()
        {
            ClearChilds();

            //Deck slots
            for (int i = 0; i < dataBase.deckSize; i++)
            {
                UnityEngine.Object instance = PrefabUtility.InstantiatePrefab(slotBackground);
                instance.GetComponent<Transform>().SetParent(deckGroup);
                instance.name += $"_{deckGroup.childCount}";

                //Reset scale
                instance.GetComponent<Transform>().localScale = Vector3.one;
            }

            //All slots
            int slotsAmount = (Mathf.CeilToInt(dataBase.elements.Length / 4f)) * 4;
            for (int i = 0; i < slotsAmount; i++)
            {
                UnityEngine.Object instance = PrefabUtility.InstantiatePrefab(slotBackground);
                instance.GetComponent<Transform>().SetParent(gridGroup);
                instance.name += $"_{gridGroup.childCount}";

                //Reset scale
                instance.GetComponent<Transform>().localScale = Vector3.one;
            }
        }
#endif
        #endregion
    }

    [System.Serializable]
    public struct DeckMenuGUI
    {
        public CardDesign design;
        public TextMeshProUGUI description;
        public TextMeshProUGUI type;
        public TextMeshProUGUI tiling;
    }
}
