using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.GameManagement
{
	[CreateAssetMenu(fileName = "New Deck Profile", menuName = "Game Management/DeckProfile"), InlineEditor]
	public class DeckProflieSO : ScriptableObject
	{
		[TitleGroup("Deck Profile")]
        public TrapSO[] cards = new TrapSO[8];

		public bool Contains(TrapSO reference)
		{
			return cards.Contains(reference);
		}
    }
}
