using _ScriptableObjects.Afflictions;
using UnityEngine;

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
}
