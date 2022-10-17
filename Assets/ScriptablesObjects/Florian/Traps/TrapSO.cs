using UnityEngine;

#region TrapSO
namespace _Scripts.TrapSystem.Datas
{
    [CreateAssetMenu(fileName = "New Trap", menuName = "Scriptables/Trap")]
    public class TrapSO : ScriptableObject
    {
        [Header("Trap infos")]
        public GameObject trapPrefab;
        public string trapName = "New trap";
        public float damage;
        public int health;
        [TextArea(10, 10)] public string description = "New description";
        [Range(1, 10)] public int xAmount = 5;
        [Range(1, 10)] public int yAmount = 5;
    }
}
#endregion
