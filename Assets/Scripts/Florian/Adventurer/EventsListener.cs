using UnityEngine;
using System;

namespace Adventurer
{
    public class EventsListener : MonoBehaviour
    {
        //Fall and Land
        public AnimationEvent onFall;
        public AnimationEvent onLand;

        //Dodge
        public AnimationEvent onStartDodge;
        public AnimationEvent onMiddleDodge;
        public AnimationEvent onEndDodge;

        //Attack
        public AnimationEvent onStartAttack;
        public AnimationEvent onMiddleAttack;
        public AnimationEvent onEndAttack;

        //Aim
        public AnimationEvent onStartAim;
        public AnimationEvent onMiddleAim;
        public AnimationEvent onEndAim;

        //Aim
        public AnimationEvent onStartAbility;
        public AnimationEvent onMiddleAbility;
        public AnimationEvent onEndAbility;
    }
}

[Serializable]
public class AnimationEvent
{
    public UnityEngine.Events.UnityEvent animEvent;

    public void RaiseEvent()
    {
        animEvent?.Invoke();
    }
}
