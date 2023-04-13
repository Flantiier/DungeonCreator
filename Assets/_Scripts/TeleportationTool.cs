using _Scripts.Characters;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportationTool : MonoBehaviour
{
    [SerializeField] private InputAction input;
    [SerializeField] private GameObject panel;

    public static event Action<Transform> OnTeleportSelected;

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
}
