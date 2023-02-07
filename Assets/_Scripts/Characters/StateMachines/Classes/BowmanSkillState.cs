using UnityEngine;
using _Scripts.Characters.Adventurers;

namespace _Scripts.Characters.Animations.StateMachines
{
    public class BowmanSkillState : CharacterStateMachine
    {
        #region Variables
        private Bowman _bowman;
        private bool _defuseWait;
        #endregion

        #region Inherited Methods
        protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bowman = Character.GetComponent<Bowman>();
            _bowman.IsDefusing = true;

            Character.ResetCharacterVelocity();
            Character.LookTowardsOrientation();
            Character.PlayerSM.CanAttack = false;

            _defuseWait = false;
            _bowman.CurrentDefuseTime = 0f;
        }

        protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            HandleDefuse();
        }

        protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _bowman.IsDefusing = false;
            Character.PlayerSM.CanAttack = true;
        }
        #endregion

        #region Methods
        private void HandleDefuse()
        {
            if (!_bowman || _defuseWait)
                return;

            if (_bowman.CurrentDefuseTime >= _bowman.TargetDefuseTime)
            {
                _bowman.DefuseTrap();
                _bowman.IsDefusing = false;

                _defuseWait = true;
                return;
            }

            _bowman.CurrentDefuseTime += Time.deltaTime;
        }
        #endregion
    }
}
