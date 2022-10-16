using System;
using UnityEngine;

namespace _Scripts.TrapSystem
{
    [CreateAssetMenu(fileName = "New Trap", menuName = "Scriptables/Trap")]
    public class TrapSO : ScriptableObject
    {
        public Trap trapInstance;
    }
}

namespace _Scripts.TrapSystem
{
    [Serializable]
    public struct Trap
    {
        [Tooltip("Trap prefab to instantiate")]
        public GameObject trapPrefab;

        [Range(1, 10), Tooltip("XAmount of required tiles")]
        public int xAmount;

        [Range(1, 10), Tooltip("YAmount of required tiles")]
        public int yAmount;
    }
}
