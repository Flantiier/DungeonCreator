using System;
using UnityEngine;
using _Scripts.Utilities.Florian;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "New GameSettings", menuName = "Scriptables/Game Management/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game info")]
        public TimeReference startTempo = new TimeReference(30f, TimeFunctions.TimeUnit.Seconds);
        public TimeReference duration = new TimeReference(20f, TimeFunctions.TimeUnit.Minuts);

        [Header("Respawns Table")]
        public RespawnUnit[] respawnUnits;
    }
}

#region RespawnInfo_Class
[Serializable]
public struct RespawnUnit
{
    public float minBound;
    public float maxBound;
    public float respawnDelay;
}
#endregion

#region TimeReference_Class
[Serializable]
public class TimeReference
{
    public float duration = 20f;
    public TimeFunctions.TimeUnit timeUnit = TimeFunctions.TimeUnit.Seconds;

    public TimeReference(float _duration, TimeFunctions.TimeUnit _unit)
    {
        duration = _duration;
        timeUnit = _unit;
    }

    public float GetTimeValue()
    {
        return TimeFunctions.GetDurationInSeconds(duration, timeUnit);
    }
}
#endregion
