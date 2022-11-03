using UnityEngine;

[CreateAssetMenu(fileName = "New OverallDatas", menuName = "Scriptables/Adventurers/CharacterOverall")]
public class CharactersOverallDatas : ScriptableObject
{
    [Header("Movements properties")]
    [Range(0f, 0.2f)] public float inputSmoothing = 0.1f;
    [Range(0f, 0.2f)] public float speedSmoothing = 0.15f;
    [Range(0f, 0.2f)] public float rotationSmoothing = 0.1f;

    [Space] public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float aimSpeed = 1f;
    public float dodgeSpeed = 10f;

    [Header("Overall stats")]
    public float healthRecup = 7f;
    public float healthRecupTime = 3f;
    [Space] 
    public float staminaRecup = 5f;
    public float staminaToRun = 1f;
    public float staminaToDodge = 20f;
}

#region ExposedProperty_Class
[System.Serializable]
public class ExposedProperty<T>
{
    [SerializeField] private T value;
    public T Value => value;
}
#endregion
