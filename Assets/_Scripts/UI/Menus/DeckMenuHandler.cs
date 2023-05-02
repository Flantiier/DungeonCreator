using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.Traps;
using UnityEngine.UI;
using UnityEditor;
using Unity.Transforms;

namespace _Scripts.UI.Menus
{
	public class DeckMenuHandler : MonoBehaviour
	{
		[TitleGroup("References")]
		[SerializeField] private CardsDataBase dataBase;
		[SerializeField] private DraggableCardMenu cardPrefab;
		[SerializeField] private Transform cards;

        [TitleGroup("Slots")]
		[SerializeField] private CardSlot slotPrefab;
		[SerializeField] private Transform slots;

        #region Builts_In
        private void Start()
        {
			InstantiateCards();
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

                CreateCard(i);
            }
		}

		/// <summary>
		/// Instantiate a new Card
		/// </summary>
		private void CreateCard(int i)
		{
			DraggableCardMenu card = Instantiate(cardPrefab, cards);
			CardSlot slot = slots.GetChild(i).transform.GetComponent<CardSlot>();
            card.transform.position = slot.transform.position;
			slot.SetCardInSlot(card);
		}

		#region Editor
#if UNITY_EDITOR
		[Button("Update Slots")]
		private void CreateSlots()
        {
			//Destroy all children
            var tempArray = new GameObject[slots.childCount];
            for (int i = 0; i < tempArray.Length; i++)
                tempArray[i] = slots.GetChild(i).gameObject;

            foreach (var child in tempArray)
                DestroyImmediate(child);

			//Create slots
			for (int i = 0; i < dataBase.cards.Length; i++)
				PrefabUtility.InstantiatePrefab(slotPrefab, slots);
		}
#endif
        #endregion

        #endregion
    }
}
