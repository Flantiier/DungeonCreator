using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.UserDatas;

namespace _ScriptableObjects._UserDatas
{
    [CreateAssetMenu(fileName = "New User Datas", menuName = "Scriptables/Game Management/User Datas")]
    public class UserDatas : ScriptableObject
    {
        #region Properties
        [TitleGroup("DM datas")]
        [ShowInInspector] public DeckProflieSO currentDeck { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Set the current deck used to a given one
        /// </summary>
        /// <param name="deck"> New deck to use </param>
        public void SetCurrentDeck(DeckProflieSO deck)
        {
            if (currentDeck == deck)
                return;

            currentDeck = deck;
        }
        #endregion
    }
}