using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Adventurer
{
    public class CharacterFighting : MonoBehaviour
    {
        #region Fighting Variables
        //References
        //Photon Comp
        protected PhotonView _view;
        //Player Inputs Comp
        protected PlayerInput _inputs;
        //Events Lusteners on the player
        protected EventsListener _events;

        [Header("Fighting Variables")]
        [SerializeField, Tooltip("Current player Abilities selected")]
        protected PlayerAbilities abilities;

        [SerializeField, Tooltip("Maximum holding attacks value")]
        protected float maxHoldValue = 3f;

        /// <summary>
        /// Indicates how long the playe is holding the attack
        /// </summary>
        protected float _holdValue;
        #endregion

        #region Animations References
        [Header("Animations Variables")]
        [SerializeField, Tooltip("Animator component")]
        protected bool overrideAnim;

        [SerializeField, Tooltip("Animator component")]
        protected Animator animator;

        [SerializeField, Tooltip("Animation overrider to change abilities animations")]
        protected AnimatorOverrider animatorOverrider;

        [SerializeField, Range(0f, 1f), Tooltip("Lerping value to the aiming layer weight")]
        protected float lerpLayerValue = 0.05f;

        /// <summary>
        /// Current aiming layerWeight
        /// </summary>
        protected float _layerWeight = 0f;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates if the player can attack
        /// </summary>
        public bool CanAttack { get; private set; }
        /// <summary>
        /// Indicates if the player is attacking (read inputs)
        /// </summary>
        public bool IsAttacking { get; private set; }
        /// <summary>
        /// Indicates if the player can aim
        /// </summary>
        public bool CanAim { get; private set; }
        /// <summary>
        /// Indicates if the player is aiming (read inputs)
        /// </summary>
        public bool IsAiming { get; private set; }
        /// <summary>
        /// Indiactes if the player can use his abilities
        /// </summary>
        public bool CanUseAbilities { get; private set; }
        #endregion

        #region Builts-In
        protected void Awake()
        {
            //Get pView
            _view = GetComponent<PhotonView>();
            if (!_view.IsMine)
                return;

            //Get PlayerInputs comp
            _inputs = GetComponent<PlayerInput>();
            //Get EventsListener comp
            _events = GetComponent<EventsListener>();

            //Override abilities animations 
            if (overrideAnim)
            {
                animatorOverrider.GetOverrideClips();
                animatorOverrider.OverrideAnimations(abilities.currentAbilities[0].clip, abilities.currentAbilities[1].clip);
            }

            //Player can attack
            EnableAttack();
            //Player can aim
            EnableAim();
            //Player can use abilities
            EnableAbilities();
        }

        protected void OnEnable()
        {
            //Not local
            if (!_view.IsMine)
                return;

            //Local
            //Subscribe to inputs events
            SubscribeToInputs();
        }

        protected void OnDisable()
        {
            //Not local
            if (!_view.IsMine)
                return;

            //Local
            //Unsubscribe to inputs events
            UnsubscribeToInputs();
        }

        protected void Update()
        {
            //Not local
            if (!_view.IsMine)
                return;

            //Local 
            //Setting attack animations
            SetAttackAnim();
            //Setting layerWeight
            SetLayerWeight();
        }
        #endregion

        #region Fighting Methods
        /// <summary>
        /// Aiming Method
        /// </summary>
        protected virtual void AimMehod() { }

        //Setting attack animations
        protected void SetAttackAnim()
        {
            //Holding attack timer
            if (CanAttack && IsAttacking && _holdValue < maxHoldValue)
                _holdValue += Time.deltaTime;
            else
                _holdValue = 0f;

            //Setting animators parameters
            animator.SetFloat("HoldingAttack", _holdValue);
            animator.SetBool("Aiming", CanAim && IsAiming);
            animator.SetFloat("Cooldown", Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1));
        }

        /// <summary>
        /// Setting Aiming layerWeight
        /// </summary>
        private void SetLayerWeight()
        {
            //Interpolates layerValue from 0 to 1 on aiming
            if (CanAim && IsAiming && _layerWeight != 1)
                _layerWeight = Mathf.Lerp(_layerWeight, 1f, lerpLayerValue);
            else if (!IsAiming && _layerWeight != 0f)
                _layerWeight = Mathf.Lerp(_layerWeight, 0f, lerpLayerValue);

            if (CanAim && IsAiming && _layerWeight >= 0.95f)
                _layerWeight = 1f;
            else if (!IsAiming && _layerWeight <= 0.05f)
                _layerWeight = 0f;

            animator.SetLayerWeight(1, _layerWeight);
        }

        /// <summary>
        /// Activating first ability
        /// </summary>
        private void TrigerFirstAbility(InputAction.CallbackContext ctx)
        {
            //Can't use Abilities
            if (!CanAttack && !CanUseAbilities && !abilities.currentAbilities[0].Used)
                return;

            //Using abilities
            abilities.currentAbilities[0].StartReloadTimer();
            FirstAbilityAnimationRPC();
        }

        [PunRPC]
        private void FirstAbilityAnimationRPC()
        {
            animator.SetTrigger("FirstAbility");
        }

        /// <summary>
        /// Activating second Ability
        /// </summary>
        private void TrigerSecondAbility(InputAction.CallbackContext ctx)
        {
            //Cant' use ability
            if (!CanAttack && !CanUseAbilities && !abilities.currentAbilities[0].Used)
                return;

            //Using second Ability
            abilities.currentAbilities[0].StartReloadTimer();
            SecondAbilityAnimationRPC();
        }

        [PunRPC]
        private void SecondAbilityAnimationRPC()
        {
            animator.SetTrigger("SecondAbility");
        }
        #endregion

        #region Events
        /// <summary>
        /// Subscribing to inputs events
        /// </summary>
        protected void SubscribeToInputs()
        {
            //Attack
            _inputs.actions["Attack"].started += ctx => IsAttacking = ctx.ReadValueAsButton();
            _inputs.actions["Attack"].canceled += ctx => IsAttacking = ctx.ReadValueAsButton();

            //Aim
            _inputs.actions["Aim"].started += ctx => IsAiming = ctx.ReadValueAsButton();
            _inputs.actions["Aim"].canceled += ctx => IsAiming = ctx.ReadValueAsButton();

            //Abilities
            _inputs.actions["FirstAbility"].started += TrigerFirstAbility;
            _inputs.actions["SecondAbility"].started += TrigerSecondAbility;
        }

        /// <summary>
        /// Unsubscribing to inputs events
        /// </summary>
        protected void UnsubscribeToInputs()
        {
            //Attack
            _inputs.actions["Attack"].started -= ctx => IsAttacking = ctx.ReadValueAsButton();
            _inputs.actions["Attack"].canceled -= ctx => IsAttacking = ctx.ReadValueAsButton();

            //Aim
            _inputs.actions["Aim"].started -= ctx => IsAiming = ctx.ReadValueAsButton();
            _inputs.actions["Aim"].canceled -= ctx => IsAiming = ctx.ReadValueAsButton();

            //Abilities
            _inputs.actions["FirstAbility"].started -= TrigerFirstAbility;
            _inputs.actions["SecondAbility"].started -= TrigerSecondAbility;
        }

        public void EnableAttack() { CanAttack = true; }
        public void DisableAttack() { CanAttack = false; }
        public void EnableAim() { CanAim = true; }
        public void DisableAim() { CanAim = false; }
        public void EnableAbilities() { CanUseAbilities = true; }
        public void DisableAbilites() { CanUseAbilities = false; }

        public void AnimDebug(string debug) { Debug.Log(debug); }
        #endregion
    }
}
