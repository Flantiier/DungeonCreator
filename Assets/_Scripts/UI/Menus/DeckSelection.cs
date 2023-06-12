using UnityEngine;
using UnityEngine.UI;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.Traps;
using Unity.VisualScripting;

namespace _Scripts.UI.Menus
{
	public class DeckSelection : MonoBehaviour
	{
		#region Variables
		[SerializeField] private GameObject panelButton;
        [SerializeField] private DeckDatabase deckDatabase;
        [SerializeField] private CardGUI slotPrefab;
        [SerializeField] private Transform slotParent;
        [SerializeField] private GameObject selectButton;
        [SerializeField] private DeckPresetButton[] deckButtons;

        private CardGUI[] _slots;
		private int _deckIndex;
		private DeckPresetButton _lastButton;
        #endregion

        #region Builts_In
        private void Awake()
        {
			deckDatabase.Load();
			InitializeSlots();
        }

        private void OnEnable()
        {
			PlayerList.OnRoleChanged += EnableDeckButton;
        }

        private void OnDisable()
        {
			PlayerList.OnRoleChanged -= EnableDeckButton;
        }

        private void OnDestroy()
        {
			deckDatabase.Save();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable the deck button based on the current role
        /// </summary>
        private void EnableDeckButton(Role role)
		{
			panelButton.SetActive(role == Role.Master);
		}

        /// <summary>
		/// Create all slots
		/// </summary>
        private void InitializeSlots()
		{
			//Init array
			_slots = new CardGUI[deckDatabase.GetDeck().cards.Length];
			for (int i = 0; i < _slots.Length; i++)
			{
				CardGUI instance = Instantiate(slotPrefab, slotParent);
				_slots[i] = instance;
			}

			//Display current deck
			DisplayDeck(deckDatabase.DeckIndex);
			_lastButton = deckButtons[deckDatabase.DeckIndex];
            _lastButton.IsSelected(true);
        }

		/// <summary>
		/// Update card slots tp display a selected deck
		/// </summary>
		public void DisplayDeck(int index)
		{
			if (!deckDatabase)
				return;

			//Set index
			index = Mathf.Clamp(index, 0, deckDatabase.decks.Length);
			DeckProflieSO deck = deckDatabase.decks[index];
			for (int i = 0; i < deck.cards.Length; i++)
			{
				TrapSO card = deck.cards[i];
				_slots[i].UpdateInfos(card);
			}

			EnableDeckButton(index);
		}

		/// <summary>
		/// Select a new deck in the database
		/// </summary>
		public void SelectDeck()
		{
			if (_lastButton)
				_lastButton.IsSelected(false);

			deckDatabase.DeckIndex = _deckIndex;
			_lastButton = deckButtons[_deckIndex];
			_lastButton.IsSelected(true);
			selectButton.SetActive(false);
		}

		/// <summary>
		/// Enable or disable a deck button
		/// </summary>
        public void EnableDeckButton(int index)
        {
            deckButtons[_deckIndex].Button.interactable = true;
            deckButtons[index].Button.interactable = false;

            _deckIndex = index;
			selectButton.SetActive(index != deckDatabase.DeckIndex);
        }
        #endregion
    }
}
