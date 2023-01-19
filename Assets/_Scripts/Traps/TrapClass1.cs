using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.TrapSystem;

namespace _Scripts.GameplayFeatures.Traps
{
	public class TrapClass1 : NetworkAnimatedObject
	{
        #region Variables/Properties
        [TitleGroup("Default properties")]
		[SerializeField] protected Tile.TilingType type;
        #endregion

        #region Properties
        public Tile.TilingType Type => type;
        #endregion
    }
}
