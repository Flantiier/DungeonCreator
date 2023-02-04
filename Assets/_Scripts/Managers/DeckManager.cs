using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Traps;
using _ScriptableObjects.UserDatas;
using _Scripts.GameplayFeatures;

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

        [TitleGroup("Gameplay properties")]
        [SerializeField] private int cardInHand = 3;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();

            InstantiateDeck();
            DrawMultiple(cardInHand);
        }
        #endregion

        #region Test Methods
        [ContextMenu("Shuffle stroage")]
        private void ShuffleStorage()
        {
            ShuffleDeck(storageZone);
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
        /// Draw the first card in the storage zone
        /// </summary>
        private void Draw()
        {
            if (!storageZone || !cardZone)
            {
                Debug.LogWarning("Missing reference in the deck manager");
                return;
            }

            if (storageZone.childCount <= 0)
                return;

            //Get the first child
            Transform card = storageZone.GetChild(0);
            //Set its parent and child index
            card.SetParent(cardZone);
            card.SetAsLastSibling();
            card.gameObject.SetActive(true);
        }

        /// <summary>
        /// Draw multiples card loop
        /// </summary>
        /// <param name="amount"> Draw amount </param>
        private void DrawMultiple(int amount)
        {
            for (int i = 0; i < amount; i++)
                Draw();
        }

        /// <summary>
        /// Disable the object and set its parent to the storage zone
        /// </summary>
        /// <param name="transform"> Transform to set parent </param>
        public void SendToStorageZone(Transform transform)
        {
            if (!transform)
                return;

            //Send the used card to the storage
            transform.gameObject.SetActive(false);
            transform.SetParent(storageZone);
            transform.SetAsLastSibling();

            //Draw a new card
            Draw();
        }

        /// <summary>
        /// Shuffle a array based on the Yates algorythm
        /// </summary>
        /// <typeparam name="T"> Array type </typeparam>
        /// <param name="array"> array to shuffle </param>
        private void Shuffle<T>(T[] array)
        {
            int range = array.Length;

            for (int i = range - 1; i > 0; i--)
            {
                //Get the random index and the last item
                int rnd = Random.Range(0, i);
                T temp = array[i];
                //Exchange the random selected number and the last one of the array
                array[i] = array[rnd];
                array[rnd] = temp;
            }
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