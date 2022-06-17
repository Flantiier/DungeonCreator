using UnityEngine;

namespace Adventurer
{
    public class AdventurerAnimator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("Referencing the Animator Component")]
        private Animator animator;

        [SerializeField, Tooltip("Refrencing the AnimatorController")]
        private RuntimeAnimatorController animatorController;

        [SerializeField, Tooltip("Index of the lower layer in the Animator")]
        private int lowerLayerIndex = 1;

        //

        [Header("Animator Parameters")]
        [SerializeField, Tooltip("Input Parameter Name in the Animator => Indicates if the player is moving")]
        private string INPUTS_PARAM = "Inputs";

        [SerializeField, Tooltip("Name of the DirX Parameter in the Animator")]
        private string DIR_X_PARAM = "DirX";

        [SerializeField, Tooltip("Name of the DirY Parameter in the Animator")]
        private string DIR_Y_PARAM = "DirY";

        [SerializeField, Tooltip("Grounded Parameter Name in the Animator => Indicates if the player is grounded")]
        private string GROUNDED_PARAM = "Grounded";

        [SerializeField, Tooltip("Speed Parameter Name in the Animator => Indicates the motion speed")]
        private string SPEED_PARAM = "MotionSpeed";

        [SerializeField, Tooltip("Dodge Parameter Name in the Animator => Indicates when the player is dodging")]
        private string DODGE_PARAM = "Dodge";

        [Space, SerializeField, Range(0f, 1f), Tooltip("Lerping value to the motionBlendTree")]
        private float lerpingMoveState = 0.1f;

        [SerializeField, Tooltip("Name of the Attack Parameter in the Animator")]
        private string ATTACK_PARAM = "Attack";

        [SerializeField, Tooltip("Name of the Holding Attack Parameter in the Animator")]
        private string HOLD_ATTACK_PARAM = "HoldAttack";

        [SerializeField, Tooltip("Nmae of the Holding Weapon Parameter in the Animator")]
        private string HOLD_WEAPON_PARAM = "HoldWeapon";

        private AdventurerController _adventurer;
        private AdventurerFighting _fighting;
        private float _motionSpeed;

        private void Awake()
        {
            SetUpAnimator();
        }

        private void Update()
        {
            UpdatesAnimations();
        }

        /// <summary>
        /// Setting up the Animator
        /// </summary>
        private void SetUpAnimator()
        {
            //Get scripts
            _adventurer = GetComponent<AdventurerController>();
            _fighting = GetComponent<AdventurerFighting>();

            if (animator == null)
                return;
            //Set the Animator Runtime Controller
            animator.runtimeAnimatorController = animatorController;
        }

        /// <summary>
        /// Updates Motion Animations
        /// </summary>
        private void UpdatesAnimations()
        {
            //No controller
            if (_adventurer == null)
                return;

            HandleIdleAnim();

            HandleMotionAnim();

            HandleDodgeAnim();

            HandleAttackAnim();

            HandleHoldWeapon();
        }

        private void HandleIdleAnim()
        {
            //Set Input Bool
            if (_adventurer.Input.DirectionInputs == Vector2.zero)
                animator.SetBool(INPUTS_PARAM, false);
            else
                animator.SetBool(INPUTS_PARAM, true);

            if (_adventurer.IsDodging)
                return;

            animator.SetBool(GROUNDED_PARAM, _adventurer.IsGrounded);

            animator.SetFloat(DIR_X_PARAM, _adventurer.Input.DirectionInputs.x);
            animator.SetFloat(DIR_Y_PARAM, _adventurer.Input.DirectionInputs.y);
        }

        private void HandleMotionAnim()
        {
            //Set Run Bool
            if (_adventurer.Input.isRunning)
                _motionSpeed = Mathf.Lerp(_motionSpeed, 2f, lerpingMoveState);

            else
                _motionSpeed = Mathf.Lerp(_motionSpeed, _adventurer.Input.DirectionInputs.magnitude, lerpingMoveState);

            if (_adventurer.Input.DirectionInputs == Vector2.zero)
                _motionSpeed = Mathf.Lerp(_motionSpeed, 0f, lerpingMoveState);

            if (_motionSpeed < 0.1f)
                _motionSpeed = 0f;

            animator.SetFloat(SPEED_PARAM, _motionSpeed);
        }

        private void HandleDodgeAnim()
        {
            if (_adventurer.IsDodging)
                animator.SetTrigger(DODGE_PARAM);
            else
                animator.ResetTrigger(DODGE_PARAM);

            animator.SetFloat("Cooldown", Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1));
        }

        private void HandleAttackAnim()
        {
            if (_fighting.HoldAttack > 0)
            {
                animator.SetTrigger(ATTACK_PARAM);
                animator.SetBool(HOLD_ATTACK_PARAM, true);
            }
            else
            {
                animator.ResetTrigger(ATTACK_PARAM);
                animator.SetBool(HOLD_ATTACK_PARAM, false);
            }
        }

        private void HandleHoldWeapon()
        {
            animator.SetBool(HOLD_WEAPON_PARAM, _adventurer.Input.IsHoldingWeapon);
        }
    }
}
