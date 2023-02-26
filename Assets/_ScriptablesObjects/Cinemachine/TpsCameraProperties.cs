using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.Cinemachine
{
    [CreateAssetMenu(menuName = "Cinemachine/TpsCamera Properties"), InlineEditor]
    public class TpsCameraProperties : ScriptableObject
    {
        [FoldoutGroup("Body Properties"), HideLabel, GUIColor(1.5f, 1, 0.5f)]
        public FramingTransposerProperties framingTranposer = new FramingTransposerProperties(0);
        [FoldoutGroup("Aim Properties"), HideLabel]
        public POVProperties pov = new POVProperties(0);
    }
}