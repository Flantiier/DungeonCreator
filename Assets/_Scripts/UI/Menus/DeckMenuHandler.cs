using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.Traps;

namespace _Scripts.UI.Menus
{
	public class DeckMenuHandler : MonoBehaviour
	{
		[TitleGroup("References")]
		[SerializeField] private CardsDataBase dataBase;
		[SerializeField] private GameObject cardPrefab;

		[TitleGroup("UI")]
		[SerializeField] private Transform layoutButtonParent;
		[SerializeField] private Transform cardsPool;

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

			foreach (TrapSO trap in dataBase.cards)
			{
				if (!trap)
				{
					Debug.LogWarning("Msising card");
					continue;
				}

				InstantiateCardInPool(trap);
			}
		}

		/// <summary>
		/// Instantiate a new card prefab the pool
		/// </summary>
		private void InstantiateCardInPool(TrapSO trap)
		{
			GameObject instance = Instantiate(cardPrefab);

			//Set parent
			for (int i = cardsPool.childCount - 1; i >= 0; i--)
			{
				Transform child = cardsPool.GetChild(i);

				if (child.childCount >= 1)
					continue;

				instance.transform.SetParent(child);
				instance.transform.localPosition = Vector3.zero;
				instance.transform.localScale = Vector3.one;
			}
		}
        #endregion

        #region Editor
#if UNITY_EDITOR
        [Button("Init Buttons")]
		private void InitalizeButtons()
		{
			if (!layoutButtonParent)
				return;

			Button[] buttons = layoutButtonParent.GetComponentsInChildren<Button>();
			for (int i = 0; i < buttons.Length; i++)
				buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
		}


#endif
#endregion
	}
}
