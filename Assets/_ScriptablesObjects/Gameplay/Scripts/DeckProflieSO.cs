using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.UserDatas
{
	[CreateAssetMenu(fileName = "New Deck Profile", menuName = "Gameplay/DeckProfile")]
	public class DeckProflieSO : ScriptableObject
	{
		#region Variables/Protected
		[TitleGroup("Deck Profile")]
		public string deckName = "New Deck";
		[TitleGroup("Deck Profile")]
        public TrapSO[] cards = new TrapSO[6];
        #endregion
    }
}
