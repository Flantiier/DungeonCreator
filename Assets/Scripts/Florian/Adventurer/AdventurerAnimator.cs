using UnityEngine;

namespace Adventurer
{
    public class AdventurerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("Referencing the Animator Component")]
        protected Animator animator;

        [SerializeField, Tooltip("Refrencing the AnimatorController")]
        protected RuntimeAnimatorController animatorController;

        //

        [Header("Animator Parameters")]
        [SerializeField, Tooltip("Speed Parameter Name in the Animator => Indicates the motion speed")]
        protected string SPEED_PARAM = "MotionSpeed";

        [SerializeField, Tooltip("Input Parameter Name in the Animator => Indicates if the player is moving")]
        protected string INPUTS_PARAM = "Inputs";

        [SerializeField, Tooltip("Dodge Parameter Name in the Animator => Indicates when the player is dodging")]
        protected string DODGE_PARAM = "Dodge";

        [Space, SerializeField, Range(0f, 1f), Tooltip("Lerping value to the motionBlendTree")]
        protected float lerpingMoveState = 0.1f;

        private AdventurerController _adventurer;
        private float _motionSpeed;

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
                _motionSpeed = Mathf.Lerp(_motionSpeed, 2f, lerpingMoveState);
            else if (_adventurer.State == AdventurerController.MotionStates.Walking)
                _motionSpeed = Mathf.Lerp(_motionSpeed, _adventurer.DirectionInputs.magnitude, lerpingMoveState);
            if(_adventurer.State == AdventurerController.MotionStates.Standing)
                _motionSpeed = Mathf.Lerp(_motionSpeed, 0f, lerpingMoveState);

            if (_motionSpeed < 0.1f)
                _motionSpeed = 0f;

            animator.SetFloat(SPEED_PARAM, _motionSpeed);
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
