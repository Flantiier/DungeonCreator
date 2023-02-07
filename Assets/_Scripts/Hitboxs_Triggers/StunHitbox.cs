using UnityEngine;
using _Scripts.Interfaces;
using _Scripts.Hitboxs_Triggers.Hitboxs;

namespace _Scripts.Hitboxs_Triggers
{
	public class StunHitbox : Hitbox
	{
        [Header("Stun properties")]
        [SerializeField] private float stunDuration = 5f;

        private void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out IPlayerStunable player))
				return;

			player.Stunned(stunDuration);
		}
	}
}
