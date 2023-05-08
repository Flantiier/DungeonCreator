using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Menus
{
    public class PlayerInfos : MonoBehaviour
    {
        #region Variables/Properties
        [SerializeField] private PlayerRoleGUI[] roles;

        [Header("GUI")]
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI roleField;
        [SerializeField] private Image image;
        [SerializeField] private GameObject[] localGUI;

        public PlayerProperties MyPlayer { get; set; }
        #endregion

        #region Builts_In
        private void OnEnable()
        {
            PlayerList.OnPlayerReady += EnableLocalGUI;
        }

        private void OnDisable()
        {
            PlayerList.OnPlayerReady -= EnableLocalGUI;   
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set name, role and roleSprite on this GUI element
        /// </summary>
        public void SetPlayerGUI(PlayerProperties playerProperties)
        {
            MyPlayer = playerProperties;

            string name = MyPlayer.player.NickName == "*Player" ? MyPlayer.player.NickName : $"Player{Random.Range(0, 1000)}";
            name = MyPlayer.player.IsLocal ? name + " (Me)" : name;
            nameField.text = name;
            SetRoleInfos(MyPlayer.role);
            EnableLocalGUI(PhotonNetwork.LocalPlayer, MyPlayer.player.IsLocal);
        }

        /// <summary>
        /// Set role name and sprite based on the player role
        /// </summary>
        public void SetRoleInfos(Role role)
        {
            foreach (PlayerRoleGUI item in roles)
            {
                if (item.role != role)
                    continue;

                roleField.text = item.name;
                image.sprite = item.sprite;
            }
        }

        /// <summary>
        /// Enable buttons if player is local
        /// </summary>
        private void EnableLocalGUI(Player player, bool enabled)
        {
            if (!player.IsLocal)
                return;

            foreach (GameObject item in localGUI)
                item.SetActive(enabled);
        }

        /// <summary>
        /// Change the curretn character selected
        /// </summary>
        public void ChangeCharacter(int value)
        {
            int newValue = (int)MyPlayer.role;
            int size = sizeof(Role) - 1;
            newValue += value;
            newValue = newValue > size ? 0 : newValue < 0 ? size : newValue;

            //Update value
            MyPlayer.role = (Role)System.Enum.ToObject(typeof(Role), newValue);
            SetRoleInfos(MyPlayer.role);

            //Rpc
            PlayerList.OnRoleUpdated?.Invoke(MyPlayer.player, MyPlayer.role);
        }
        #endregion
    }
}

[System.Serializable]
public struct PlayerRoleGUI
{
    public Role role;
    public string name;
    public Sprite sprite;
}