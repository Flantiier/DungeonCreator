using UnityEngine;
using _Scripts.Interfaces;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.Hitboxs_Triggers
{
	public class StunHitbox : Hitbox
	{
        #region Variables
        [Header("Stun properties")]
        [SerializeField] private float stunDuration = 5f;
		[SerializeField] private float timeBeforeDestroy = 0.25f;
        #endregion

        #region Builts_In
        private void Start()
		{
			Destroy(gameObject, timeBeforeDestroy);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out IPlayerStunable player))
				return;

			player.Stunned(stunDuration);
		}
        #endregion
    }
}
