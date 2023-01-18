using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class Hitbox : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField, Required, InfoBox("This has to be in trigger to run the script")] 
        private Collider hitboxCollider;

        public Collider Collider => hitboxCollider;
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            if (!Collider)
            {
                Debug.LogWarning($"Missing collider reference on {gameObject}!");
                return;
            }

            Collider.isTrigger = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable the collider
        /// </summary>
        public void EnableCollider(bool enabled)
        {
            Collider.enabled = enabled;
        }
        #endregion
    }
}