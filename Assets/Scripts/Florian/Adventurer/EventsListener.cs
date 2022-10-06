using UnityEngine;
using System;

namespace Adventurer
{
    public class EventsListener : MonoBehaviour
    {
        #region Animations Events
        //Fall and Land
        [Header("Falling Events")]
        public AnimationEvent onFall;
        public AnimationEvent onLand;

        //Dodge
        [Header("Dodge Events")]
        public AnimationEvent onStartDodge;
        public AnimationEvent onMiddleDodge;
        public AnimationEvent onEndDodge;

        //Attack
        [Header("Attack Events")]
        public AnimationEvent onStartAttack;
        public AnimationEvent onMiddleAttack;
        public AnimationEvent onEndAttack;

        //Aim
        [Header("Aim Events")]
        public AnimationEvent onStartAim;
        public AnimationEvent onMiddleAim;
        public AnimationEvent onEndAim;

        //Aim
        [Header("Abilties Events")]
        public AnimationEvent onStartAbility;
        public AnimationEvent onMiddleAbility;
        public AnimationEvent onEndAbility;
        #endregion
    }
}

[Serializable]
public class AnimationEvent
{
    //List of methods reached by this events
    public UnityEngine.Events.UnityEvent animEvent;

    /// <summary>
    /// Raising the event
    /// </summary>
    public void RaiseEvent()
    {
        animEvent?.Invoke();
    }
}
