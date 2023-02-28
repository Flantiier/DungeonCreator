using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Interfaces;

namespace _Scripts.Hitboxs_Triggers.Hitboxs
{
    public class EnemyHitbox : Hitbox
    {
        #region Variables
        [TitleGroup("Properties")]
        [SerializeField] private bool enabledOnStart = false;
        [TitleGroup("Properties")]
        [SerializeField] private bool initDamagesOnStart = true;
        [TitleGroup("Properties"), ShowIf("initDamagesOnStart")]
        [SerializeField] private float damages = 25f;
        public float Damages { get; set; }
        #endregion

        #region Build_In
        public override void Awake()
        {
            base.Awake();
            Collider.enabled = enabledOnStart;

            if (initDamagesOnStart)
                Damages = damages;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IPlayerDamageable player))
                return;

            Debug.LogWarning("Touched");
            player.DealDamage(Damages);
        }
        #endregion
    }
}