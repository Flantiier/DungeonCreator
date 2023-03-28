using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.GameplayFeatures.IA
{
    public class PatrolingEnemy : ChasingEnemy
    {
        #region Variables
        [TitleGroup("Patroling")]
        [SerializeField] private float patrolSpeed = 1f;
        [TitleGroup("Patroling")]
        public float patrolRadius = 3f;
        private Vector3 _lastWayPoint;
        #endregion

        #region Builts_In
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_lastWayPoint, _lastWayPoint + new Vector3(0f, 1f, 0f));
        }
        #endregion

        #region Patrol Methods
        protected override void HandleEnemyBehaviour()
        {
            if (!CurrentTarget)
            {
                Debug.Log("Patrolling");

                Move(patrolSpeed);
                Patroling();
                CurrentTarget = FieldOfViewCheck();
                return;
            }

            if (CurrentTarget)
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
    }
}
