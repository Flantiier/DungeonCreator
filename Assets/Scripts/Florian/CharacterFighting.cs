using UnityEngine;
using UnityEngine.InputSystem;

namespace Adventurer
{
    public class CharacterFighting : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected AnimatorOverrider animatorOverrider;
        [SerializeField] protected PlayerAbilities abilities;
        protected PlayerInput _inputs;
        protected EventsListener _events;

        [SerializeField, Range(0f, 1f)] protected float lerpLayerValue = 0.05f;
        protected float _layerWeight = 0f;

        [SerializeField] protected float maxHoldValue = 3f;
        protected float _holdValue;

        public bool CanAttack { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool CanAim { get; private set; }
        public bool IsAiming { get; private set; }
        public bool CanUseAbilities { get; private set; }

        protected void Awake()
        {
            _inputs = GetComponent<PlayerInput>();
            _events = GetComponent<EventsListener>();

            animatorOverrider.GetOverrideClips();
            animatorOverrider.OverrideAnimations(abilities.currentAbilities[0].clip, abilities.currentAbilities[1].clip);

            EnableAttack();
            EnableAim();
            EnableAbilities();
        }

        protected void OnEnable()
        {
            SubscribeToInputs();
        }

        protected void OnDisable()
        {
            UnsubscribeToInputs();
        }

        protected void Update()
        {
            SetAttackAnim();
            SetLayerWeight();
        }

        protected virtual void AimMehod() { }

        protected void SetAttackAnim()
        {
            if (CanAttack && IsAttacking && _holdValue < maxHoldValue)
                _holdValue += Time.deltaTime;
            else
                _holdValue = 0f;

            animator.SetFloat("HoldingAttack", _holdValue);
            animator.SetBool("Aiming", CanAim && IsAiming);
            animator.SetFloat("Cooldown", Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1));
        }

        private void SetLayerWeight()
        {
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

        private void TrigerFirstAbility(InputAction.CallbackContext ctx)
        {
            if (!CanAttack && !CanUseAbilities && !abilities.currentAbilities[0].Used)
                return;

            abilities.currentAbilities[0].StartReloadTimer();
            animator.SetTrigger("FirstAbility");
        }

        private void TrigerSecondAbility(InputAction.CallbackContext ctx)
        {
            if (!CanAttack && !CanUseAbilities && !abilities.currentAbilities[0].Used)
                return;

            abilities.currentAbilities[0].StartReloadTimer();
            animator.SetTrigger("SecondAbility");
        }

        protected void SubscribeToInputs()
        {
            _inputs.actions["Attack"].started += ctx => IsAttacking = ctx.ReadValueAsButton();
            _inputs.actions["Attack"].canceled += ctx => IsAttacking = ctx.ReadValueAsButton();

            _inputs.actions["Aim"].started += ctx => IsAiming = ctx.ReadValueAsButton();
            _inputs.actions["Aim"].canceled += ctx => IsAiming = ctx.ReadValueAsButton();

            _inputs.actions["FirstAbility"].started += TrigerFirstAbility;
            _inputs.actions["SecondAbility"].started += TrigerSecondAbility;
        }

        protected void UnsubscribeToInputs()
        {
            _inputs.actions["Attack"].started -= ctx => IsAttacking = ctx.ReadValueAsButton();
            _inputs.actions["Attack"].canceled -= ctx => IsAttacking = ctx.ReadValueAsButton();

            _inputs.actions["Aim"].started -= ctx => IsAiming = ctx.ReadValueAsButton();
            _inputs.actions["Aim"].canceled -= ctx => IsAiming = ctx.ReadValueAsButton();

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
    }
}
