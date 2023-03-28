using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

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
        public Transform CurrentTarget { get; protected set; }
        public bool AttackTrigger { get; protected set; }
        public bool AttackWait { get; set; }
        #endregion

        #region Buils_In
        protected virtual void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            ShowPlayerInRange();
#endif
            HandleEnemyBehaviour();
            HandleCombatBehaviour();
            UpdateAnimations();
        }
        #endregion

        #region Methods
        protected virtual void UpdateAnimations()
        {
            Animator.SetFloat("Velocity", _navMesh.velocity.normalized.magnitude);
            Animator.SetBool("Chasing", CurrentTarget);
            Animator.SetBool("TargetNear", TargetDistance() <= _navMesh.stoppingDistance);
        }

        #region EnemyState Methods
        protected virtual void HandleEnemyBehaviour()
        {
            //No Target
            if (!CurrentTarget)
            {
                CurrentTarget = FieldOfViewCheck();
                return;
            }

            ChasePlayer();
        }
        #endregion

        #region FOV Methods
        protected void ShowPlayerInRange()
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
        #endregion

        #region Movements
        /// <summary>
        /// Set the navMesh agent speed
        /// </summary>
        protected void Move(float speed)
        {
            _navMesh.isStopped = false;
            _navMesh.speed = speed;
        }

        /// <summary>
        /// Stop the agent and set its speed to 0
        /// </summary>
        public void Stop()
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
        protected float TargetDistance()
        {
            if (!CurrentTarget)
                return 0f;

            return Vector3.Distance(transform.position, CurrentTarget.position);
        }
        #endregion

        #region Combat Methods
        protected virtual void HandleCombatBehaviour()
        {
            AttackTrigger = TargetDistance() <= _navMesh.stoppingDistance;

            if (!AttackTrigger || AttackWait)
                return;

            Attack();
        }

        protected void Attack()
        {
            Animator.SetTrigger("Attack");
        }
        #endregion

        #endregion
    }
}