using UnityEngine;
using Sirenix.OdinInspector;
using _ScriptablesObjects.Traps;
using _Scripts.Hitboxs;

namespace _Scripts.GameplayFeatures.Traps
{
    public class DamagingTrap : TrapClass1
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] protected TrapSO trapData;
        [SerializeField] protected TrapHitbox hitbox;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            hitbox.Damages = trapData.damages;
        }
        #endregion
    }
}
