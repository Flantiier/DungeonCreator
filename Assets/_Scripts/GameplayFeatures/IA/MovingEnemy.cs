using UnityEngine;
using UnityEngine.AI;
using _Scripts.IA;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovingEnemy : Enemy
    {
        #region Variables
        [SerializeField] private FieldOfView fov;
        private NavMeshAgent _navMesh;

        [Header("Variables")]
        [SerializeField] private float smoothRotation = 0.1f;
        #endregion

        #region Properties
        public Transform Target { get; protected set; }
        #endregion

        #region Buils_In
        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            ChaseTarget();
            FaceTarget();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Chase the current target
        /// </summary>
        protected void ChaseTarget()
        {
            Debug.Log(Target);

            if (!Target)
            {
                Target = fov.GetTarget();
                return;
            }

            _navMesh.SetDestination(Target.position);
        }

        /// <summary>
        /// When the player is close, the enemy faces him
        /// </summary>
        protected void FaceTarget()
        {
            if (!_navMesh || !Target)
                return;

            if (TargetDistance() > _navMesh.stoppingDistance)
                return;

            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.position - transform.position), smoothRotation);
            transform.rotation = targetRotation;
        }
        
        /// <summary>
        /// Get the distance between this AI and the current Target
        /// </summary>
        protected float TargetDistance()
        {
            return Vector3.Distance(transform.position, Target.position);
        }
        #endregion
    }
}