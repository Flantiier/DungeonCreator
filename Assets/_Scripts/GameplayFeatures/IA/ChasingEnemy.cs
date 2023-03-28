using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using TMPro;
using System.Collections;
using UnityEngineInternal;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChasingEnemy : Enemy
    {
        #region Variables
        [TitleGroup("Chasing")]
        [SerializeField] private float smoothRotation = 0.1f;
        [TitleGroup("Chasing")]
        [SerializeField] protected float chaseSpeed = 3f;

        [TitleGroup("Patroling")]
        [SerializeField] private float patrolSpeed = 1f;
        [TitleGroup("Patroling")]
        public float patrolRadius = 3f;
        [TitleGroup("Patroling")]
        [SerializeField] private float waitTime = 2f;
        [TitleGroup("Patroling")]
        [SerializeField] private float wayPointDistance = 0.5f;
        private bool _patrolWait;
        private Vector3 _patrolPoint;

        [TitleGroup("FOV")]
        public float radius = 10f;
        [TitleGroup("FOV"), Range(0, 360)]
        public float angle = 160;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask targetMask;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask obstructionMask;

        protected NavMeshAgent _navMesh;
        #endregion

        #region Properties
        public EnemyStateMachine SM { get; private set; }
        public Transform CurrentTarget { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public bool TargetNear { get; private set; }
        public bool IsAttacking { get; set; }
        #endregion

        #region Buils_In
        protected virtual void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
            SM = new EnemyStateMachine();
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            ShowPlayerInFOV();
#endif
            HandleEnemyBehaviour();
            UpdateAnimations();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_patrolPoint, _patrolPoint + new Vector3(0f, 1f, 0f));
        }
        #endregion

        #region Methods
        protected override void InitializeEnemy()
        {
            //Init health
            base.InitializeEnemy();

            StartPosition = transform.position;
            SM.CurrentState = EnemyStateMachine.EnemyState.Patrol;
        }

        protected virtual void UpdateAnimations()
        {
            Animator.SetFloat("Velocity", _navMesh.velocity.normalized.magnitude);
            Animator.SetBool("Chasing", CurrentTarget);
            Animator.SetBool("TargetNear", TargetNear);
        }

        protected virtual void HandleEnemyBehaviour()
        {
            //Find Target & Get the distance with him
            LookForTarget();
            CalculateTargetDistance();

            //No target => Patrol
            if (!CurrentTarget)
                SM.CurrentState = EnemyStateMachine.EnemyState.Patrol;
            else
            {
                //Target and close => Combat
                if (TargetNear)
                    SM.CurrentState = EnemyStateMachine.EnemyState.Combat;
                else
                    SM.CurrentState = EnemyStateMachine.EnemyState.Chase;
            }

            switch (SM.CurrentState)
            {
                case EnemyStateMachine.EnemyState.Combat:
                    CombatState();
                    break;
                case EnemyStateMachine.EnemyState.Chase:
                    ChasingState();
                    break;
                default:
                    PatrolingState();
                    break;
            }
        }

        #region FieldOfView
        /// <summary>
        /// If there's no target yet, set the first visible by the enemy
        /// </summary>
        private void LookForTarget()
        {
            if (CurrentTarget)
                return;

            CurrentTarget = FieldOfViewCheck();
        }

        /// <summary>
        /// Return the closest target in the view
        /// </summary>
        protected Transform FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length <= 0)
                return null;

            Transform currentTarget = rangeChecks[0].transform;

            foreach (Collider item in rangeChecks)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    Ray ray = new Ray(transform.position, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    float distanceToCurrent = Vector3.Distance(transform.position, currentTarget.position);

                    if (Physics.Raycast(ray, distanceToTarget, obstructionMask))
                        continue;

                    if (distanceToTarget >= distanceToCurrent)
                        continue;
                    else
                        currentTarget = target;

                }
            }

            return currentTarget;
        }

        /// <summary>
        /// Shows all the targets currenlty in the fov of the enemy
        /// </summary>
        protected void ShowPlayerInFOV()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length <= 0)
                return;

            foreach (Collider item in rangeChecks)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    Ray ray = new Ray(transform.position, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics.Raycast(ray, distanceToTarget, obstructionMask))
                        Debug.DrawRay(ray.origin, ray.direction * distanceToTarget, Color.red);
                    else
                        Debug.DrawRay(ray.origin, ray.direction * distanceToTarget, Color.green);
                }
            }
        }
        #endregion

        #region Chasing
        /// <summary>
        /// Handle the Chasing behaviour
        /// </summary>
        protected void ChasingState()
        {
            //Position
            Move(chaseSpeed);
            SetDestination(CurrentTarget.position);

            //Rotation
            Vector3 direction = CurrentTarget.position - transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), smoothRotation);
            transform.rotation = targetRotation;
        }

        /// <summary>
        /// Set the navMesh agent destination
        /// </summary>
        /// <param name="position"> Target position </param>
        protected void SetDestination(Vector3 position)
        {
            _navMesh.SetDestination(position);
        }

        /// <summary>
        /// Get the distance between this AI and the current Target
        /// </summary>
        protected void CalculateTargetDistance()
        {
            if (!CurrentTarget)
            {
                TargetNear = false;
                return;
            }

            TargetNear = Vector3.Distance(transform.position, CurrentTarget.position) <= _navMesh.stoppingDistance;
        }

        /// <summary>
        /// Set the speed of the navMesh Agent
        /// </summary>
        /// <param name="speed"> Speed value </param>
        protected void Move(float speed)
        {
            _navMesh.isStopped = false;
            _navMesh.speed = speed;
        }

        /// <summary>
        /// Stop the enemy
        /// </summary>
        public void Stop()
        {
            _navMesh.isStopped = false;
            _navMesh.speed = 0f;
        }
        #endregion

        #region Combat
        /// <summary>
        /// Handle the Combat behaviour
        /// </summary>
        protected virtual void CombatState()
        {
            if (IsAttacking)
                return;

            Animator.SetTrigger("Attack");
        }
        #endregion

        #region Patroling
        /// <summary>
        /// Handle the Patrol Behaviour
        /// </summary>
        private void PatrolingState()
        {
            //Waiting
            if (_patrolWait)
                return;

            float distanceToWayPoint = Vector3.Distance(transform.position, _patrolPoint);
            Debug.Log(distanceToWayPoint);

            //Too far from the way point
            if (distanceToWayPoint > wayPointDistance)
            {
                Move(patrolSpeed);
                GetRandomPatrolPoint();
            }
            else
            {
                //Way point reached
                StartCoroutine(PatrolWait(Random.Range(0.5f, waitTime)));
            }
        }

        /// <summary>
        /// Get a random point around the zone thsi enemy spawned
        /// </summary>
        private void GetRandomPatrolPoint()
        {
            Vector3 randomPosition = StartPosition + Random.insideUnitSphere * patrolRadius;
            if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
                return;

            _patrolPoint = hit.position;
            SetDestination(_patrolPoint);
        }

        /// <summary>
        /// Wait before getting a new patrol point
        /// </summary>
        /// <param name="time"> Wait duration </param>
        private IEnumerator PatrolWait(float time)
        {
            _patrolWait = true;
            yield return new WaitForSecondsRealtime(time);
            _patrolWait = false;
        }
        #endregion

        #endregion
    }
}

#region EnemyStateMachine
public class EnemyStateMachine
{
    public enum EnemyState { Patrol, Chase, Combat }

    public EnemyState CurrentState { get; set; }

    public bool IsStateOf(EnemyState targetState)
    {
        return CurrentState == targetState;
    }
}
#endregion