using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts.Characters;
using _Scripts.Managers;

public class TeleportationTool : MonoBehaviour
{
    #region Variables
    [SerializeField] private InputAction input;
    [SerializeField] private GameObject panel;
    #endregion

    #region Builts_In
    private void OnEnable()
    {
        if (PlayersManager.Role == Role.Master)
            return;

        input.Enable();
        input.started += EnablePanel;
    }

    private void OnDisable()
    {
        if (PlayersManager.Role == Role.Master)
            return;

        input.Disable();
        input.started -= EnablePanel;
    }
    #endregion

    #region Methods
    public static void InvokeTeleport(Transform transform)
    {
        transform.GetComponent<TeleportPointDisplay>().LoadMap();
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

    private void EnablePanel(InputAction.CallbackContext ctx)
    {
        if (panel.activeSelf)
        {
            DisablePanel();
            return;
        }

        panel.SetActive(true);
        GameManager.Instance.EnableCursor(true);
    }

    public void DisablePanel()
    {
        panel.SetActive(false);
        GameManager.Instance.EnableCursor(false);
    }
    #endregion
}
