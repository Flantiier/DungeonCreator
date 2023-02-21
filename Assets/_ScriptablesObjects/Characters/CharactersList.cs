using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects
{
    [CreateAssetMenu(menuName = "Characters List"), InlineEditor]
    public class CharactersList : ScriptableObject
    {
        public CharacterDatas[] characters = new CharacterDatas[1];
        public int Index { get; set; }

        /// <summary>
        /// Get the character at the given index
        /// </summary>
        public CharacterDatas GetCharacterAtIndex()
        {
            if (characters.Length <= 0)
                return null;

            return Index < 0 || Index >= characters.Length ? characters[0] : characters[Index];
        }

        /// <summary>
        /// Decrease the value of the index
        /// </summary>
        public void DecreaseIndex()
        {
            Index = Index - 1 < 0 ? characters.Length - 1 : Index - 1;
        }

        /// <summary>
        /// Increase the value of the index
        /// </summary>
        public void IncreaseIndex()
        {
            Index = Index + 1 >= characters.Length ? 0 : Index + 1;
        }
    }

    #region Character Class
    [System.Serializable]
    public class CharacterDatas
    {
        [HorizontalGroup("Horizontal", 75), PreviewField(75), HideLabel]
        public Sprite icon;
        [VerticalGroup("Horizontal/Vert"), LabelText("Name"), LabelWidth(65)]
        public string characterName;
        [VerticalGroup("Horizontal/Vert"), TextArea(3, 3)]
        public string description;
        [LabelText("Gameplay prefab"), LabelWidth(140)]
        public GameObject prefab;
    }
    #endregion
}
