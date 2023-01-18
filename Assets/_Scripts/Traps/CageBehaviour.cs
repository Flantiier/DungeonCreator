using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

namespace _Scripts.GameplayFeatures.Traps
{
    public class CageBehaviour : TriggeredTrap
    {
        #region Variables
        [TitleGroup("References")]
        [SerializeField] private GameObject cage;
        #endregion

        #region Builts_In
        private void Awake()
        {
            cage.SetActive(false);
        }

        public override void OnEnable()
        {
            if (!ViewIsMine())
                return;

            base.OnEnable();
            trigger.OnTriggered += HandleCageBehaviour;
        }

        public override void OnDisable()
        {
            if (!ViewIsMine())
                return;

            base.OnDisable();
            trigger.OnTriggered -= HandleCageBehaviour;
        }
        #endregion

        #region Methods
        private void HandleCageBehaviour(Characters.Character _)
        {
            RPCCall("CageBehaviourRPC", RpcTarget.AllViaServer, _);
        }

        [PunRPC]
        private void CageBehaviourRPC()
        {
            cage.SetActive(true);
        }
        #endregion
    }
}