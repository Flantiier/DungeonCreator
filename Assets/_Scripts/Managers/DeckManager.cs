using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;
using _ScriptableObjects.UserDatas;
using _Scripts.GameplayFeatures;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Photon.Chat.Demo;

namespace _Scripts.Managers
{
    public class DeckManager : MonoBehaviourSingleton<DeckManager>
    {
        #region Variables
        [TitleGroup("References")]
        [Required, SerializeField] private DeckProflieSO deck;
        [TitleGroup("References")]
        [Required, SerializeField] private DraggableCard cardPrefab;
        [TitleGroup("References")]
        [Required, SerializeField] private Transform cardZone;
        [TitleGroup("References")]
        [Required, SerializeField] private Transform storageZone;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            InstantiateDeck();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates every card in the deck
        /// </summary>
        private void InstantiateDeck()
        {
            if (!deck || deck.cards.Length <= 0)
                return;

            foreach (TrapSO trapDatas in deck.cards)
            {
                if (!trapDatas)
                    continue;

                //Create all the cards and put it in the storage zone
                DraggableCard instance = Instantiate(cardPrefab, storageZone);
                instance.name = $"Card : {trapDatas.trapName}";
                instance.UpdateCardInformations(trapDatas);
                instance.gameObject.SetActive(false);
            }

            ShuffleDeck(storageZone);
        }

        /// <summary>
        /// Shuffle the children of a given transform
        /// </summary>
        /// <param name="parent"> Transform to shuffle children from </param>
        private void ShuffleDeck(Transform parent)
        {
            int[] rnd = NewIntArray(parent.childCount);
            Shuffle(rnd);

            int index = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).SetSiblingIndex(rnd[index]);
                index++;
            }
        }

        /// <summary>
        /// Shuffle a array based on the Yates algorythm
        /// </summary>
        /// <typeparam name="T"> Array type </typeparam>
        /// <param name="array"> array to shuffle </param>
        private void Shuffle<T>(T[] array)
        {
            int range = array.Length;

            for (int i = range-1; i > 0; i--)
            {
                //Get the random index and the last item
                int rnd = Random.Range(0, i);
                T temp = array[i];
                //Exchange the random selected number and the last one of the array
                array[i] = array[rnd];
                array[rnd] = temp;
            }
        }

        [ContextMenu("Shuffle")]
        private void ShuffleTest()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            Debug.Log($"Before suffle : {array[0]} {array[1]} {array[2]} {array[3]} {array[4]}");
            Shuffle(array);
            Debug.Log($"After suffle : {array[0]} {array[1]} {array[2]} {array[3]} {array[4]}");
        }

        [ContextMenu("Shuffle stroage")]
        private void ShuffleStorage()
        {
            ShuffleDeck(storageZone);
        }

        /// <summary>
        /// Returns a random array from 0 to given length
        /// </summary>
        private int[] NewIntArray(int length)
        {
            int[] array = new int[length];

            for (int i = 0; i < array.Length; i++)
                array[i] = i;

            return array;
        }
        #endregion
    }
}