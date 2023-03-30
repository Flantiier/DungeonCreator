using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using TMPro;
using System.Collections;
using UnityEngineInternal;
using static Utils.Utilities;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChasingEnemy : Enemy
    {
        #region Variables
        public enum EnemyState { Patrol, Chase, Combat }
        public enum PatrolState { BaseReturn, GetPoint, GoToPoint, Reached, Wait }

        #region Chasing variables
        [TitleGroup("Chasing")]
        [SerializeField] private float smoothRotation = 0.1f;
        [TitleGroup("Chasing")]
        [SerializeField] protected float chaseSpeed = 3f;
        [TitleGroup("Chasing")]
        [SerializeField] protected float stoppingDistance = 1.5f;
        #endregion

        #region Patrol variables
        [TitleGroup("Patroling")]
        [SerializeField] private float patrolSpeed = 1f;
        [TitleGroup("Patroling")]
        [SerializeField] private float returnSpeed = 2f;
        [TitleGroup("Patroling")]
        public float patrolRadius = 3f;
        [TitleGroup("Patroling")]
        [SerializeField] private float patrolWait = 2f;
        [TitleGroup("Patroling")]
        [SerializeField] private float pointDistance = 0.5f;
        private PatrolState _patrolState;
        private Vector3 _patrolPoint;
        private bool _patrolWait;
        #endregion

        #region FOV varibales
        [TitleGroup("FOV")]
        public float radius = 10f;
        [TitleGroup("FOV"), Range(0, 360)]
        public float angle = 160;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask targetMask;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask obstructionMask;
        #endregion

        protected NavMeshAgent _navMesh;
        #endregion

        #region Properties
        public EnemyState CurrentState { get; set; }
        public Transform CurrentTarget { get; private set; }
        public Vector3 BasePosition { get; private set; }
        public bool TargetNear { get; private set; }
        #endregion

        #region Buils_In
        protected virtual void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
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

            BasePosition = transform.position;
            CurrentState = EnemyState.Patrol;

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
            HandleStoppingDistance();

            //No target => Patrol
            if (!CurrentTarget)
            {
                EnterPatrolState();
                CurrentState = EnemyState.Patrol;
            }
            else
            {
                //Target and close => Combat
                if (TargetNear)
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

        #region AI Methods
        /// <summary>
        /// Set the navMesh agent destination
        /// </summary>
        /// <param name="position"> Target position </param>
        protected void SetDestination(Vector3 position)
        {
            _navMesh.SetDestination(position);
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
        /// Modify the stopping distance of the agent based on the current state
        /// </summary>
        private void HandleStoppingDistance()
        {
            _navMesh.stoppingDistance = IsStateOf(EnemyState.Patrol) ? 0 : stoppingDistance;
        }

        /// <summary>
        /// Indicates if the current state is the same as the given parameter
        /// </summary>
        /// <param name="targetState"> state to check </param>
        public bool IsStateOf(EnemyState targetState)
        {
            return CurrentState == targetState;
        }
        #endregion

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

            foreach (Collider item in rangeChecks)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    Ray ray = new Ray(transform.position, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics.Raycast(ray, distanceToTarget, obstructionMask))
                        continue;

                    return target;
                }
                continue;
            }

            return null;
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
        #endregion

        #region Combat
        /// <summary>
        /// Handle the Combat behaviour
        /// </summary>
        protected virtual void CombatState()
        {
        }
        #endregion

        #region Patroling
        /// <summary>
        /// Handle the Patrol Behaviour
        /// </summary>
        private void PatrolingState()
        {
            switch (_patrolState)
            {
                case PatrolState.BaseReturn:
                    {
                        if (Vector3.Distance(transform.position, BasePosition) > 0.5f)
                        {
                            //Move to base
                            Move(returnSpeed);
                            SetDestination(BasePosition);
                            Animator.SetBool("BaseReturn", true);
                            return;
                        }

                        Animator.SetBool("BaseReturn", false);
                        StartCoroutine(PatrolWait());
                        break;
                    }
                case PatrolState.GetPoint:
                    {
                        GetRandomPatrolPoint();
                        _patrolState = PatrolState.GoToPoint;
                        break;
                    }
                case PatrolState.GoToPoint:
                    {
                        Move(patrolSpeed);

                        if (Vector3.Distance(transform.position, _patrolPoint) > pointDistance)
                            return;

                        _patrolState = PatrolState.Reached;
                        break;
                    }
                case PatrolState.Reached:
                    {
                        StartCoroutine(PatrolWait());
                        break;
                    }
                case PatrolState.Wait:
                    {
                        if (_patrolWait)
                            return;

                        _patrolState = PatrolState.GetPoint;
                        break;
                    }
            }
        }

        /// <summary>
        /// Get a random point around the zone thsi enemy spawned
        /// </summary>
        private void GetRandomPatrolPoint()
        {
            Vector3 randomPosition = BasePosition + Random.insideUnitSphere * patrolRadius;
            if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, patrolRadius * 2, NavMesh.AllAreas))
                return;

            _patrolPoint = hit.position;
            SetDestination(_patrolPoint);
        }

        /// <summary>
        /// Patrol Routine
        /// </summary>
        private IEnumerator PatrolWait()
        {
            _patrolState = PatrolState.Wait;

            _patrolWait = true;
            float randomTime = Random.Range(1f, patrolWait);
            yield return new WaitForSecondsRealtime(randomTime);

            _patrolWait = false;
        }

        /// <summary>
        /// Stops the patrol routine if it started
        /// </summary>
        private void EnterPatrolState()
        {
            if (CurrentState == EnemyState.Patrol)
                return;

            _patrolState = PatrolState.BaseReturn;
            _patrolWait = false;
        }
        #endregion

        #endregion
    }
}