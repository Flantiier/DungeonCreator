using UnityEngine;

namespace Adventurer 
{
	public class WarriorAnimator : AdventurerAnimator
	{
        private void Awake()
        {
            base.SetUpAnimator();
        }

        private void Update()
        {
            UpdateAnimations();
        }

        //Update other animations + Motion Animations(Inheritance)
        public void UpdateAnimations()
        {
            base.MotionAnimations();
        }
    }
}
