using UnityEngine;
using UnityEngine.AI;
using _Scripts.Characters;

namespace _Scripts.GameplayFeatures.IA
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovingEnemy : Entity
    {
        #region Variables
        private NavMeshAgent _navMesh;
        #endregion

        #region Buils_In
        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }
        #endregion

        #region Methods
        #endregion
    }
}