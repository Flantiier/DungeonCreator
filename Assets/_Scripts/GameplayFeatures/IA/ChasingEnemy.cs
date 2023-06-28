using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using _Scripts.Characters;
using _ScriptableObjects.Traps;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChasingEnemy : Enemy
    {
        #region Variables
        public enum EnemyState { Patrol, Chase, Combat }
        public enum PatrolState { BaseReturn, GetPoint, GoToPoint, Reached, Wait }

        [FoldoutGroup("References")]
        [SerializeField] private DestructibleRagdollHandler ragdoll;
        [FoldoutGroup("References")]
        [SerializeField] private CharacterAudio audioSource;

        [TitleGroup("Properties")]
        public EnemyProperties properties;

        protected NavMeshAgent _navMesh;
        private PatrolState _patrolState;
        private Vector3 _patrolPoint;
        private bool _patrolWait;
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

        public override void OnEnable()
        {
            base.OnEnable();
            InitializeEnemy();
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            ShowPlayerInFOV();
#endif

            LookForTarget();

            if (!ViewIsMine())
                return;

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
        protected override void HandleEntityHealth(float damages)
        {
            audioSource.PlayClip(0);
            base.HandleEntityHealth(damages);
        }

        #region Behaviours
        protected virtual void InitializeEnemy()
        {
            if (!ViewIsMine())
                return;

            InitializeHealth(properties.health);

            BasePosition = transform.position;
            CurrentState = EnemyState.Patrol;
            _patrolState = PatrolState.BaseReturn;
            _patrolWait = false;
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
            CalculateTargetDistance();
            HandleStoppingDistance();

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

        protected override void HandleEntityDeath()
        {
            RPCCall("EnableRagdollRPC", RpcTarget.All);
        }

        [PunRPC]
        public void EnableRagdollRPC()
        {
            ragdoll.EnableRagdoll();
            gameObject.SetActive(false);
        }
        #endregion

        #region AI Methods
        /// <summary>
        /// Check for the health points of the current target
        /// </summary>
        protected void CheckTargetHealth()
        {
            if (!CurrentTarget || !CurrentTarget.TryGetComponent(out Character character))
                return;

            if (character.CurrentHealth > 0)
                return;

            CurrentTarget = null;
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

            TargetNear = GetDistanceWithTarget() <= _navMesh.stoppingDistance;
        }

        /// <summary>
        /// Return the distance between the target and this object
        /// </summary>
        protected float GetDistanceWithTarget()
        {
            return Vector3.Distance(CurrentTarget.position, transform.position);
        }

        /// <summary>
        /// Modify the stopping distance of the agent based on the current state
        /// </summary>
        protected void HandleStoppingDistance()
        {
            _navMesh.stoppingDistance = IsStateOf(EnemyState.Patrol) ? 0 : properties.stoppingDistance;
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
        protected void LookForTarget()
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
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, properties.radius, properties.targetMask);

            if (rangeChecks.Length <= 0)
                return null;

            foreach (Collider item in rangeChecks)
            {
                if (!item.TryGetComponent(out Character character))
                    continue;

                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < properties.angle / 2)
                {
                    Ray ray = new Ray(transform.position, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics.Raycast(ray, distanceToTarget, properties.obstructionMask))
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
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, properties.radius, properties.targetMask);

            if (rangeChecks.Length <= 0)
                return;

            foreach (Collider item in rangeChecks)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < properties.angle / 2)
                {
                    Ray ray = new Ray(transform.position, directionToTarget);
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics.Raycast(ray, distanceToTarget, properties.obstructionMask))
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
        protected virtual void ChasingState()
        {
            //Position
            Move(properties.chaseSpeed);
            SetDestination(CurrentTarget.position);

            //Rotation
            LookAtTarget();
        }

        /// <summary>
        /// Look at the current target
        /// </summary>
        public void LookAtTarget()
        {
            Vector3 direction = CurrentTarget.position - transform.position;
            direction.y = 0f;
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), properties.smoothRotation);
            transform.rotation = targetRotation;
        }
        #endregion

        #region Combat
        /// <summary>
        /// Handle the Combat behaviour
        /// </summary>
        protected virtual void CombatState() { }
        #endregion

        #region Patroling
        /// <summary>
        /// Handle the Patrol Behaviour
        /// </summary>
        protected void PatrolingState()
        {
            switch (_patrolState)
            {
                case PatrolState.BaseReturn:
                    {
                        if (Vector3.Distance(transform.position, BasePosition) > 0.5f)
                        {
                            //Move to base
                            Move(properties.returnSpeed);
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
                        Move(properties.patrolSpeed);

                        if (Vector3.Distance(transform.position, _patrolPoint) > properties.pointDistance)
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
            Vector3 randomPosition = BasePosition + Random.insideUnitSphere * properties.patrolRadius;
            if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, properties.patrolRadius * 2, NavMesh.AllAreas))
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
            float randomTime = Random.Range(properties.minPatrolWait, properties.maxPatrolWait);
            yield return new WaitForSecondsRealtime(randomTime);

            _patrolWait = false;
        }

        /// <summary>
        /// Stops the patrol routine if it started
        /// </summary>
        protected void EnterPatrolState()
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