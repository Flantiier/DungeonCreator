using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Multi.Connexion;

[CreateAssetMenu(menuName = "Characters List"), InlineEditor]
public class CharactersList : ScriptableObject
{
    #region Variables
    [TabGroup("Adventurers")]
    public CharacterDatas warrior;
    [TabGroup("Adventurers")]
    public CharacterDatas archer;
    [TabGroup("Adventurers")]
    public CharacterDatas wizard;
    [TabGroup("DM")]
    public CharacterDatas dungeonMaster;
    [TabGroup("DM")]
    public CharacterDatas boss;
    #endregion

    #region Methods
    /// <summary>
    /// Get the character at the given index
    /// </summary>
    public CharacterDatas GetCharacterFromRole(ListPlayersRoom.Roles role)
    {
        switch (role)
        {
            case ListPlayersRoom.Roles.Warrior:
                return warrior;

            case ListPlayersRoom.Roles.Archer:
                return archer;

            case ListPlayersRoom.Roles.Wizard:
                return wizard;

            case ListPlayersRoom.Roles.DM:
                return dungeonMaster;

            default:
                return warrior;
        }
    }
    #endregion
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
