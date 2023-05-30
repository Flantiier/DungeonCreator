using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Cinemachine
{
    [CreateAssetMenu(menuName = "SO/Cinemachine/TopCamera Properties"), InlineEditor]
    public class TopCameraProperties : ScriptableObject
    {
        [FoldoutGroup("Body Properties"), HideLabel, GUIColor(1.5f, 1, 0.5f)]
        public TransposerProperties transposer = new TransposerProperties(0);
        [FoldoutGroup("Aim Properties"), HideLabel, GUIColor(0.5f, 1, 1)]
        public ComposerProperties composer = new ComposerProperties(0);
    }
}