using System;
using UnityEngine;
using Personnal.Florian;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "New GameSettings", menuName = "Scriptables/Game Management/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game info")]
        public TimeReference startTempo = new TimeReference(30f, PersonnalUtilities.Time.TimeUnit.Seconds);
        public TimeReference duration = new TimeReference(20f, PersonnalUtilities.Time.TimeUnit.Minuts);

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
    public PersonnalUtilities.Time.TimeUnit timeUnit = PersonnalUtilities.Time.TimeUnit.Seconds;

    public TimeReference(float _duration, PersonnalUtilities.Time.TimeUnit _unit)
    {
        duration = _duration;
        timeUnit = _unit;
    }

    public float GetTimeValue()
    {
        return PersonnalUtilities.Time.GetDurationInSeconds(duration, timeUnit);
    }
}
#endregion
