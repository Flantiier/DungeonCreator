using Sirenix.OdinInspector;
using UnityEngine;

namespace _ScriptableObjects.Settings.UI
{
    [CreateAssetMenu(fileName = "New Reticle Properties", menuName = "Temporary/Reticle")]
    public class ReticleDatas : ScriptableObject
    {
        [BoxGroup("Properties")]
        public bool enabled = true;
        [BoxGroup("Properties")]
        [Range(0f, 3f)] public float size = 1f;
        [BoxGroup("Properties")]
        [Range(0f, 1f)] public float oppacity = 1f;
    }
}
