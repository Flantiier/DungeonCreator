using UnityEngine;
using Sirenix.OdinInspector;

namespace _ScriptableObjects.DM
{
    [CreateAssetMenu(fileName = "New DM Datas", menuName = "Scriptables/DM")]
    [InlineEditor]
	public class DMDatas : ScriptableObject
	{
		#region Variables
		[BoxGroup("Movements")]
		[Range(10f, 35f), GUIColor(1, 2, 1)]
		public float motionSpeed = 25;
        [BoxGroup("Movements")]
        [Range(80f, 150f), GUIColor(3, 2, 0)]
        public float rotationSpeed = 100;
        [BoxGroup("Movements")]
        [Range(0f, 0.2f), GUIColor(1, 3, 2)]
        public float smoothingMotion = 0.1f;

        [BoxGroup("Mana properties")]
        [Range(50f, 200f), GUIColor(3, 0.5f, 3)]
        public int manaAmount = 100;
        [BoxGroup("Mana properties")]
        [Range(1f, 20f), GUIColor(0.5f, 1, 2)]
        public float manaRecovery = 10f;

        [BoxGroup("Camera properties")]
        public Vector3 followOffset = new Vector3(0f, 20f, -10f);
        [BoxGroup("Camera properties")]
        public Vector3 XYZDamping = Vector3.zero;
        [BoxGroup("Camera properties")]
        [Range(0f, 2f), GUIColor(0.5f, 2f, 2f)]
        public float YawDamping = 0.75f;
        #endregion
    }
}
