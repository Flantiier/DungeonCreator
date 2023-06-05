using UnityEngine;
using _ScriptableObjects.Afflictions;

namespace _Scripts.Interfaces
{
	public interface IPlayerDamageable
	{
		public void DealDamage(float damages);
		public void KnockbackDamages(float damages, Vector3 hitPoint);
    }
    
    public interface ITrapDamageable
    {
        public void TrapDamages(float damages);
    }

    public interface IPlayerAfflicted
    {
        public void TouchedByAffliction(AfflictionStatus status);
    }

    public interface IPlayerStunable
    {
        public void StunPlayer(float duration);
    }
}
