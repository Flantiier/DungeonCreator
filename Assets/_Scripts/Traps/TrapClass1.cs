using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.TrapSystem;

namespace _Scripts.GameplayFeatures.Traps
{
	public class TrapClass1 : NetworkMonoBehaviour
	{
        #region Variables/Properties
        [TitleGroup("References")]
        [SerializeField] protected Animator animator;

        [TitleGroup("Default properties")]
		[SerializeField] protected Tile.TilingType type;
        #endregion

        #region Properties
        public Tile.TilingType Type => type;
        #endregion
    }
}
