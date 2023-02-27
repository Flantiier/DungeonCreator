using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptableObjects.Cinemachine;

namespace _ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "New DM Properties", menuName = "Scriptables/Characters/DM"), InlineEditor]
	public class DMProperties : ScriptableObject
	{
		[BoxGroup("Movements", CenterLabel = true)]
		[Range(10f, 35f), GUIColor(1, 2, 1)]
		public float motionSpeed = 25;
        [BoxGroup("Movements")]
        [Range(80f, 150f), GUIColor(3, 2, 0)]
        public float rotationSpeed = 100;
        [BoxGroup("Movements")]
        [Range(0f, 0.2f), GUIColor(1, 3, 2)]
        public float smoothingMotion = 0.1f;

        [BoxGroup("Mana properties", CenterLabel = true)]
        [Range(50f, 200f), GUIColor(3, 0.5f, 3)]
        public int manaAmount = 100;
        [BoxGroup("Mana properties")]
        [Range(1f, 20f), GUIColor(0.5f, 1, 2)]
        public float manaRecovery = 10f;

        [BoxGroup("Camera Properties", CenterLabel = true), HideLabel]
        public TopCameraProperties cameraProperties;
    }
}
