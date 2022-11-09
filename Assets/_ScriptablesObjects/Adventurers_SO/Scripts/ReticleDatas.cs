using UnityEngine;

namespace _ScriptablesObjects.Settings.UI
{
    [CreateAssetMenu(fileName = "New ReticleDatas", menuName = "Scriptables/UI/Reticle")]
    public class ReticleDatas : ScriptableObject
    {
        [Header("Reticle properties")]
        public bool enabled = true;
        [Range(0f, 3f)] public float size = 1f;
        public Color color = Color.white;
    }
}
