using UnityEngine;
using System.Collections;
using _Scripts.Interfaces;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DefusableTrap : DamagingTrap, IDefusable
    {
        public float DefuseDuration { get; set; }
        public bool IsDisabled { get; set; }

        public void DefuseTrap()
        {
            StartCoroutine("DefusedTrapRoutine");
        }

        protected virtual IEnumerator DefusedTrapRoutine()
        {
            float baseSpeed = Animator.speed;

            IsDisabled = true;
            Animator.speed = 0f;
            hitbox.EnableCollider(false);
            yield return new WaitForSecondsRealtime(DefuseDuration);

            IsDisabled = false;
            Animator.speed = baseSpeed;
            hitbox.EnableCollider(true);
        }
    }
}