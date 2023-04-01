﻿using UnityEngine;

namespace _Scripts.GameplayFeatures.IA.StateMachines
{
    public class EnemyAttackState : EnemyStateMachine
    {
        [SerializeField] private float attackDuration = 0.75f;
        [SerializeField] private float slerp = 0.01f;
        private bool _combo;
        private Quaternion _rotation;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _combo = false;
            _rotation = enemy.transform.rotation;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.IsInTransition(0))
            {
                enemy.Stop();
                enemy.transform.rotation = _rotation;
            }
            else
            {
                Vector3 direction = enemy.CurrentTarget.position - enemy.transform.position;
                _rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z)), slerp);
                enemy.transform.rotation = _rotation;
            }

            HandleComboTrigger(animator, stateInfo.normalizedTime);
        }

        private void HandleComboTrigger(Animator animator, float normalizedTime)
        {
            if (_combo || normalizedTime < attackDuration)
                return;

            if (!enemy.TargetNear)
                return;

            //Combo
            _combo = true;
            animator.SetTrigger("Combo");
        }
    }
}