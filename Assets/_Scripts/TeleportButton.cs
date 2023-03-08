using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    [SerializeField] private Transform teleportTo;
    [SerializeField] private GameObject panel;

    public void Teleport()
    {
        TeleportationTool.InvokeTeleport(teleportTo);
        panel.SetActive(false);
    }
}
