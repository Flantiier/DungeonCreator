using UnityEngine;

namespace _Scripts.GameplayFeatures.IA
{
    public class PooledEnemy : MonoBehaviour
    {
        [SerializeField] private GameObject AI;
        [SerializeField] private DestructibleRagdollHandler ragdoll;

        private void OnEnable()
        {
            AI.SetActive(true);
        }

        private void OnDisable()
        {            
            ragdoll.ResetRagdoll();
        }
    }
}
