using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.UserDatas
{
	[CreateAssetMenu(fileName = "New Deck Profile", menuName = "Gameplay/DeckProfile"), InlineEditor]
	public class DeckProflieSO : ScriptableObject
	{
		[TitleGroup("Deck Profile")]
		public string deckName = "New Deck";
		[TitleGroup("Deck Profile")]
        public TrapSO[] cards = new TrapSO[6];

		public bool Contains(TrapSO reference)
		{
			return cards.Contains(reference);
		}
    }
}
