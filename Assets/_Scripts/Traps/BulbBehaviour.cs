using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;
using Photon.Pun;
using _Scripts.Hitboxs_Triggers.Hitboxs;
using _ScriptableObjects.Afflictions;

namespace _Scripts.GameplayFeatures.Traps
{
    public class BulbBehaviour : DamagingTrap
    {
        #region Variables
        [TitleGroup("Bulb Properties")]
        [SerializeField] private VisualEffect fx;
        [SerializeField] private AfflictionPack afflictionPack;
        [SerializeField] private float waitTime = 2f;
        [SerializeField] private float sporesDuration = 5f;

        private bool _sporesOn;
        #endregion

        #region Builts_In
        public override void Awake()
        {
            base.Awake();
            afflictionPack.SetUpAfflictionPack();
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            base.OnDisable();
        }
        #endregion

        #region Methods
        private void HandleBulbBehaviour(Characters.Character _)
        {
            StartCoroutine("SporesRoutine");
        }

        private IEnumerator SporesRoutine()
        {
            RPCCall("BulbBehaviourRPC", RpcTarget.AllViaServer, true);
            yield return new WaitForSecondsRealtime(waitTime);
            RPCCall("BulbBehaviourRPC", RpcTarget.AllViaServer, false);
            yield return new WaitForSecondsRealtime(waitTime);
        }

        /// <summary>
        /// Bulb behaviour method through the network
        /// </summary>
        /// <param name="state"> Enable or disable </param>
        [PunRPC]
        private void BulbBehaviourRPC(bool state)
        {
            _sporesOn = state;

            //False
            if (!state)
            {
                fx.Stop();
                return;
            }

            //True
            fx.Play();
        }

        /// <summary>
        /// Enable or disable the spores hitbox
        /// </summary>
        /// <param name="enabled"> Enable or disable </param>
        public void EnableCollider(bool enabled)
        {
            afflictionPack.hitbox.EnableCollider(enabled);
        }
        #endregion
    }
}

#region AfflictionPack_Class
namespace _Scripts.GameplayFeatures
{
    public class AfflictionPack
    {
        public AfflictionHitbox hitbox;
        public AfflictionStatus affliction;

        public void SetUpAfflictionPack()
        {
            hitbox.affliction = affliction;
        }
    }
}
#endregion