using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.GameplayFeatures.Projectiles;

namespace _ScriptableObjects.Traps
{
	[CreateAssetMenu(fileName = "New Baliste Datas", menuName = "Scriptables/Traps/Baliste Datas")]
	[InlineEditor]
	public class BallistaDatas : TrapSO
	{
        #region Variables
        [FoldoutGroup("Properties")]
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
		[Range(20, 150), GUIColor(1, 3, 1)]
        public int health = 50;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(10, 50), GUIColor(2, 0.5f, 0.3f)]
        public int damage = 25;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(0f, 0.2f), GUIColor(3, 2, 0.5f)] 
        public float smoothRotation = 0.125f;
        [BoxGroup("Properties/Stats"), LabelWidth(100)]
        [Range(0.5f, 3f), GUIColor(3, 2, 1)]
        public float fireRate = 2f;
        
        [TitleGroup("Properties/Projectile"), LabelWidth(100)]
        public Projectile projectilePrefab;
        #endregion
    }
}