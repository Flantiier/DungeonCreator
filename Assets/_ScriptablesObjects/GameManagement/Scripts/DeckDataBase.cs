using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.GameManagement;
using _ScriptableObjects.Traps;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(menuName = "Game Management/DeckDatabase")]
    public class DeckDatabase : ScriptableObject
    {
        public int deckUsed = 0;
        public DeckProflieSO[] decks;
        public CardsDatabase database;

        [Button("Save")]
        public void Save()
        {
            DecksDataStruct save = new DecksDataStruct(decks, database, deckUsed);
            SaveSystem.Save(save, "/_decks");
        }

        [Button("Load")]
        public void Load()
        {
            DecksDataStruct load = new DecksDataStruct();
            SaveSystem.Load(ref load, "_decks");

            deckUsed = load.currentDeck;
            TrapSO[][] array = load.GetDecksFromString(load.keys, database);

            for (int i = 0; i < array.Length; i++)
                decks[i].cards = array[i];
        }
    }
}

public class DecksDataStruct
{
    public int currentDeck;
    public string[] keys;

    public DecksDataStruct() {}

    public DecksDataStruct(DeckProflieSO[] decks, CardsDatabase database, int deckUsed)
    {        
        currentDeck = deckUsed;
        keys = new string[decks.Length * database.deckSize];
        for (int i = 0; i < keys.Length; i++)
        {
            int deckIndex = i / database.deckSize;
            int cardIndex = i - (deckIndex * database.deckSize);
            string value = database.GetKeyByCard(decks[deckIndex].cards[cardIndex]);
            keys[i] = value;
        }
    }

    public TrapSO[][] GetDecksFromString(string[] keys, CardsDatabase database)
    {
        //Create all arrays
        TrapSO[][] array = new TrapSO[keys.Length / database.deckSize][];
        for (int i = 0; i < array.Length; i++)
            array[i] = new TrapSO[database.deckSize];

        for(int i = 0;i < keys.Length; i++)
        {
            int deckIndex = i / database.deckSize;
            int cardIndex = i - (deckIndex * database.deckSize);
            array[deckIndex][cardIndex] = database.GetCardByKey(keys[i]);
        }

        return array;
    }
}
