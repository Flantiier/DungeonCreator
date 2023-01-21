using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _ScriptablesObjects.Traps;

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

        #region Methods
        public override void EnableHitbox(int value)
        {
            if (!hitbox)
                return;

            bool state = value <= 0 ? false : true;
            hitbox.EnableCollider(state);
        }
        #endregion
    }
}
