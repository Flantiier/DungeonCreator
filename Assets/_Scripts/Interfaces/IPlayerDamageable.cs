using UnityEngine;

namespace _Scripts.Interfaces
{
	public interface IPlayerDamageable
	{
		public void SoftDamages(float damages);
		public void HardDamages(float damages, Vector3 hitPoint);
	}
}
