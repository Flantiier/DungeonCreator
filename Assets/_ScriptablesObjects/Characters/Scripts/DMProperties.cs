using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Cinemachine;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New DM Properties", menuName = "SO/Characters/DM¨Properties"), InlineEditor]
	public class DMProperties : ScriptableObject
	{
		[BoxGroup("Movements", CenterLabel = true), LabelWidth(120), Range(10, 40), GUIColor(2, 1, 0)]
		public float motionSpeed = 25;
        [BoxGroup("Movements"), LabelWidth(120), Range(50, 200), GUIColor(2, 1, 0)]
        public float rotationSpeed = 100;
        [BoxGroup("Movements"), LabelWidth(120) , Range(0, 0.2f), GUIColor(2, 1, 0)]
        public float smoothingMotion = 0.1f;

        [BoxGroup("Mana properties", CenterLabel = true), LabelWidth(120), Range(0, 100), GUIColor(2, 0.5f, 2)]
        public int manaAmount = 10;
        [BoxGroup("Mana properties"), LabelWidth(120), Range(0f, 20f), GUIColor(2, 0.5f, 2)]
        public float manaRecovery = 10f;

        [BoxGroup("Camera Properties", CenterLabel = true), HideLabel]
        public TopCameraProperties cameraProperties;
    }
}
