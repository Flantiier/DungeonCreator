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
        [SerializeField] private GameObject readyImage;

        public PlayerProperties Infos { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Set name, role and roleSprite on this GUI element
        /// </summary>
        public void SetPlayerGUI(PlayerProperties playerProperties)
        {
            Infos = playerProperties;

            //Name
            string name = Infos.player.NickName != "*Player" ? Infos.player.NickName : $"Player{Random.Range(0, 1000)}";
            name = Infos.player.IsLocal ? name + " (Me)" : name;
            nameField.text = name;
            //Role
            SetRoleInfos(Infos.role);
            //Ready
            readyImage.SetActive(Infos.isReady);
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

        public void SetReady(bool value)
        {
            Infos.isReady = value;
            readyImage.SetActive(Infos.isReady);
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