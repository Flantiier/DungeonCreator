using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Characters;

public class TeleportationTool : MonoBehaviour
{
    #region Variables
    [SerializeField] private InputAction input;
    [SerializeField] private GameObject panel;
    #endregion

    #region Builts_In
    private void OnEnable()
    {
        input.Enable();
        input.started += ctx => panel.SetActive(!panel.activeSelf);
    }

    private void OnDisable()
    {
        input.Disable();
        input.started -= ctx => panel.SetActive(!panel.activeSelf);
    }
    #endregion

    #region Methods
    public static void InvokeTeleport(Transform transform)
    {
        Character character = FindPlayer();

        if (!character)
            return;

        character.TeleportPlayer(transform);
    }

    /// <summary>
    /// Return the local character
    /// </summary>
    private static Character FindPlayer()
    {
        Character[] characters = FindObjectsOfType<Character>();

        foreach (Character character in characters)
        {
            if (!character || !character.ViewIsMine())
                continue;

            return character;
        }

        return null;
    }
    #endregion
}
