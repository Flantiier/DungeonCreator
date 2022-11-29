using UnityEngine;

namespace _ScriptablesObjects.GameManagement
{
    [CreateAssetMenu(fileName = "New GameSettings", menuName = "Scriptables/Game Management/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game info")]
        public float startTempo = 10f;
        public float duration = 20f;

        [Header("Respawns Table")]
        public RespawnUnit[] respawnUnits;
    }
}

#region RespawnInfo_Class
[System.Serializable]
public struct RespawnUnit
{
    public float minBound;
    public float maxBound;
    public float respawnDelay;
}
#endregion
