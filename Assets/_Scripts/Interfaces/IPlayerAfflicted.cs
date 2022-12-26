using _ScriptableObjects.Afflictions;

namespace _Scripts.Interfaces
{
	public interface IPlayerAfflicted
	{
		public void TouchedByAffliction(AfflictionStatus status);
	}
}
