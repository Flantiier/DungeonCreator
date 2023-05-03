using UnityEngine;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.GameManagement
{
	[CreateAssetMenu(fileName = "CardsDataBase", menuName = "Game Management/CardsDataBase")]
	public class CardsDataBase : ScriptableObject
	{
		public int deckLength = 8;
		public TrapSO[] cards;
	}
}
