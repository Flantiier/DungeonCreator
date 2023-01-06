using UnityEngine;

namespace _Scripts.Characters.Animations
{
	public class WizardAnimator : CharacterAnimator
	{
        #region Properties
        public Wizard Wizard { get; private set; }
        #endregion

        #region Builts_In
        public override void Awake()
        {
            if (!ViewIsMine())
                return;

            base.Awake();
            Wizard = GetComponentInParent<Wizard>();
        }
        #endregion

        #region Methods
        public void EnableScan()
        {
            Wizard.InstantiateScanArea();
        }
        #endregion
    }
}
