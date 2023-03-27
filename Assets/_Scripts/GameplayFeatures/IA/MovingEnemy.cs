using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using Sirenix.Utilities;
using System.Collections;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovingEnemy : Enemy
    {
        #region Variables
        [TitleGroup("Patroling")]
        [SerializeField] private float patrolSpeed = 1f;
        [TitleGroup("Patroling")]
        public float patrolRadius = 3f;
        private Vector3 _lastWayPoint;

        [TitleGroup("Chasing")]
        [SerializeField] private float smoothRotation = 0.1f;
        [TitleGroup("Chasing")]
        [SerializeField] private float chaseSpeed = 3f;
        [TitleGroup("Chasing")]
        [SerializeField] private float maxChaseDistance = 20f;

        [TitleGroup("FOV")]
        public float radius = 10f;
        [TitleGroup("FOV"), Range(0, 360)]
        public float angle = 160;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask targetMask;
        [TitleGroup("FOV")]
        [SerializeField] public LayerMask obstructionMask;

        private NavMeshAgent _navMesh;
        #endregion

        #region Properties
        public Transform CurrentTarget { get; protected set; }
        #endregion

        #region Buils_In
        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            ShowPlayerInRange();
#endif
            HandleEnemyBehaviour();
            UpdateAnimations();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_lastWayPoint, _lastWayPoint + new Vector3(0f, 1f, 0f));
        }
        #endregion

        #region Methods
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
        }

        protected virtual void UpdateAnimations()
        {
            Animator.SetFloat("Velocity", _navMesh.velocity.normalized.magnitude);
            Animator.SetBool("Chasing", CurrentTarget);
        }

        #region EnemyState Methods
        protected virtual void HandleEnemyBehaviour()
        {
            if (!CurrentTarget)
            {
                Debug.Log("Patrolling");

                Move(patrolSpeed);
                Patroling();
                CurrentTarget = FieldOfViewCheck();
                return;
            }

            if (TargetDistance() <= maxChaseDistance)
            {
                Debug.Log("Chasing");

                Move(chaseSpeed);
                ChasePlayer();
            }
            else
            {
                CurrentTarget = null;
                GetRandomPatrolPoint();
                Stop();
            }
        }

        /// <summary>
        /// Choose a random position around the enemy and move towards this destination
        /// </summary>
        private void Patroling()
        {
            if (Vector3.Distance(transform.position, _lastWayPoint) > _navMesh.stoppingDistance)
                return;

            GetRandomPatrolPoint();
        }

        private void GetRandomPatrolPoint()
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * patrolRadius;
            if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
                return;

            _lastWayPoint = hit.position;
            SetDestination(_lastWayPoint);
        }
        #endregion

        #region FOV Methods
        private void ShowPlayerInRange()
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

        /// <summary>
        /// Return the closest target in the view
        /// </summary>
        private Transform FieldOfViewCheck()
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
        #endregion

        #region Movements
        /// <summary>
        /// Set the navMesh agent speed
        /// </summary>
        private void Move(float speed)
        {
            _navMesh.isStopped = false;
            _navMesh.speed = speed;
        }

        /// <summary>
        /// Stop the agent and set its speed to 0
        /// </summary>
        private void Stop()
        {
            _navMesh.isStopped = true;
            _navMesh.speed = 0f;
        }

        /// <summary>
        /// Chase the current target
        /// </summary>
        protected void ChasePlayer()
        {
            //Position
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
        protected float TargetDistance()
        {
            return Vector3.Distance(transform.position, CurrentTarget.position);
        }
        #endregion

        #endregion
    }
}