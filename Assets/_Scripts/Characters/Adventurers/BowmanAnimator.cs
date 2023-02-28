using UnityEngine;
using _Scripts.Characters.Adventurers;

namespace _Scripts.Characters.Animations
{
	public class BowmanAnimator : CharacterAnimator
	{
        #region Properties
        public Bowman MyBowman { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!ViewIsMine())
                return;

            base.Awake();
            MyBowman = GetComponentInParent<Bowman>();
        }
        #endregion
    }
}
