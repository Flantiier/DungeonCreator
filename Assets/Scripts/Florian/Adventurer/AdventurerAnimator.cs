using UnityEngine;

namespace Adventurer
{
    public class AdventurerAnimator : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Referencing the Animator Component")]
        [SerializeField] protected Animator animator;
        [Tooltip("Refrencing the AnimatorController")]
        [SerializeField] protected RuntimeAnimatorController animatorController;

        [Header("Animator Parameters")]
        [Tooltip("Speed Parameter Name in the Animator => Indicates the motion speed")]
        [SerializeField] protected string SPEED_PARAM = "MotionSpeed";
        [Tooltip("Input Parameter Name in the Animator => Indicates if the player is moving")]
        [SerializeField] protected string INPUTS_PARAM = "Inputs";
        [Tooltip("Running Parameter Name in the Animator => Indicates of the player is running")]
        [SerializeField] protected string RUN_PARAM = "Running";
        [Tooltip("Dodge Parameter Name in the Animator => Indicates when the player is dodging")]
        [SerializeField] protected string DODGE_PARAM = "Dodge";

        /// <summary>
        /// Reference to the adventurerController
        /// </summary>
        private AdventurerController _adventurer;

        /// <summary>
        /// Setting up the Animator
        /// </summary>
        protected virtual void SetUpAnimator()
        {
            //Get the controller
            _adventurer = GetComponent<AdventurerController>();

            if (animator == null)
                return;
            //Set the Animator Runtime Controller
            animator.runtimeAnimatorController = animatorController;
        }

        /// <summary>
        /// Updates Motion Animations
        /// </summary>
        protected virtual void MotionAnimations()
        {
            //No controller
            if (_adventurer == null)
                return;

            //Set Input Bool
            if (_adventurer.DirectionInputs == Vector2.zero)
                animator.SetBool(INPUTS_PARAM, false);
            else
                animator.SetBool(INPUTS_PARAM, true);

            //Set Run Bool
            if (_adventurer.State == AdventurerController.MotionStates.Running)
                animator.SetBool(RUN_PARAM, true);
            else
                animator.SetBool(RUN_PARAM, false);

            //Set Motion Speed
            animator.SetFloat(SPEED_PARAM, _adventurer.MoveSpeed);
        }

        /// <summary>
        /// Trigger the Dodge Animation
        /// </summary>
        public void SetDodgeAnimation()
        {
            animator.SetTrigger(DODGE_PARAM);
        }
    }
}
