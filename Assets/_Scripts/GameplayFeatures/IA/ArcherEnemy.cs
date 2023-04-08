using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.Projectiles;
using Photon.Pun;

namespace _Scripts.GameplayFeatures.IA
{
    public class ArcherEnemy : ChasingEnemy
    {
        #region Variables
        [TitleGroup("Archer properties")]
        [SerializeField] private float shootingDistance = 20f;
        [SerializeField] private LayerMask shootObstruction;

        [TitleGroup("Projectile")]
        [SerializeField] private EnemiesProjectile projectile;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private float throwForce = 10f;
        private bool _targetInRange;
        private bool _canReachTarget;
        #endregion

        #region Methods
        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
            //Class parameters
            bool shoot = CurrentTarget && _targetInRange && _canReachTarget;
            Animator.SetBool("CanShoot", shoot);
        }

        protected override void HandleEnemyBehaviour()
        {
            //Find Target & Get the distance with him
            LookForTarget();
            CalculateTargetDistance();
            HandleStoppingDistance();
            //Long range behaviour
            ArcherBehaviour();

            //Check player
            CheckTargetHealth();

            //No target => Patrol
            if (!CurrentTarget)
            {
                EnterPatrolState();
                CurrentState = EnemyState.Patrol;
            }
            else
            {
                //Target in shoot range and can be reached
                if (_targetInRange && _canReachTarget)
                    CurrentState = EnemyState.Combat;
                else
                    CurrentState = EnemyState.Chase;
            }

            switch (CurrentState)
            {
                case EnemyState.Combat:
                    CombatState();
                    break;
                case EnemyState.Chase:
                    ChasingState();
                    break;
                default:
                    PatrolingState();
                    break;
            }
        }

        #region StateMachine Methods
        private void ArcherBehaviour()
        {
            if (!CurrentTarget)
                return;

            //Can shoot on target
            float distance = GetDistanceWithTarget();
            _targetInRange = distance <= shootingDistance;

            if (!_targetInRange)
                return;

            //Look if there's some obstacles
            Ray ray = new Ray(transform.position, CurrentTarget.position - transform.position);
            //Hit an obstacle
            if (Physics.Raycast(ray, distance, shootObstruction))
                _canReachTarget = false;
            else
                _canReachTarget = true;
        }

        protected override void ChasingState()
        {
            Move(chaseSpeed);
            LookAtTarget();
            SetDestination(CurrentTarget.position);
        }

        protected override void CombatState()
        {
            LookAtTarget();
            Stop();
        }
        #endregion

        #region Projectile
        /// <summary>
        /// Laucnh a projectile into the target direction
        /// </summary>
        public void ShootProjectile()
        {
            if (!ViewIsMine() || !CurrentTarget)
                return;

            //Calculates the direction of the projectile
            Vector3 direction = CurrentTarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            //Launch
            EnemiesProjectile instance = PhotonNetwork.Instantiate(projectile.name, shootPosition.position, rotation).GetComponent<EnemiesProjectile>();
            instance.OverrideThrowForce(direction, throwForce);
        }
        #endregion

        #endregion
    }
}
