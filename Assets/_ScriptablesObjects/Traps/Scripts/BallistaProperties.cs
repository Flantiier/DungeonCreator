using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.Projectiles;

namespace _ScriptableObjects.Traps
{
	[CreateAssetMenu(fileName = "New Baliste Properties", menuName = "Traps/Baliste Properties"), InlineEditor]
	public class BallistaProperties : TrapSO
	{
        #region Variables
        [FoldoutGroup("Properties"), LabelWidth(100), Range(20, 400), GUIColor(0, 2, 0.5f)]
        public int health = 50;
        [FoldoutGroup("Properties"), LabelWidth(100), Range(10, 150), GUIColor(0, 1.5f, 2)]
        public int damages = 25;
        [FoldoutGroup("Properties"), LabelWidth(100), Range(0, 0.5f), GUIColor(1, 2, 3)] 
        public float smoothRotation = 0.125f;
        [FoldoutGroup("Properties"), LabelWidth(100), Range(0.5f, 5), GUIColor(1, 2, 3)]
        public float fireRate = 2f;
        
        [TitleGroup("Properties/Projectile"), LabelWidth(100)]
        public Projectile projectilePrefab;
        [TitleGroup("Properties/Projectile"), LabelWidth(100), Range(5f, 30f), GUIColor(1, 2, 3)]
        public float throwForce = 20f;
        #endregion
    }
}