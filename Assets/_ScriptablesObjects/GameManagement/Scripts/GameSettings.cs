using System;
using UnityEngine;
using Utils;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "New GameSettings", menuName = "Scriptables/Game Management/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game info")]
        public TimeReference startTempo = new TimeReference(30f, Utils.Utilities.Time.TimeUnit.Seconds);
        public TimeReference duration = new TimeReference(20f, Utils.Utilities.Time.TimeUnit.Minuts);

        [Header("Respawns Table")]
        public RespawnUnit[] respawnUnits;
    }
}

#region RespawnInfo_Class
[Serializable]
public struct RespawnUnit
{
    [HorizontalGroup("Time Bounds"), LabelText("Time Bounds")]
    public float minBound;
    [HorizontalGroup("Time Bounds"), HideLabel, LabelWidth(40)]
    public float maxBound;
    public float respawnDelay;
}
#endregion

#region TimeReference_Class
[Serializable]
public class TimeReference
{
    public float duration = 20f;
    public Utils.Utilities.Time.TimeUnit timeUnit = Utils.Utilities.Time.TimeUnit.Seconds;

    public TimeReference(float _duration, Utils.Utilities.Time.TimeUnit _unit)
    {
        duration = _duration;
        timeUnit = _unit;
    }

    public float GetTimeValue()
    {
        return Utils.Utilities.Time.GetDurationInSeconds(duration, timeUnit);
    }
}
#endregion
