using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _Scripts.Characters;

namespace _Scripts.UI.Interfaces
{
    public class PlayerHUD : MonoBehaviour
    {
        #region Variables/Properties
        [TitleGroup("Sliders properties")]
        [SerializeField] private CharacterSlider healthSlider;
        [TitleGroup("Sliders properties")]
        [SerializeField] private CharacterSlider staminaSlider;

        [FoldoutGroup("Players panel")]
        [SerializeField] private Transform content;
        [FoldoutGroup("Players panel")]
        [SerializeField] private GameObject playerUI;

        [FoldoutGroup("Skill GUI")]
        [SerializeField] private FloatVariable skillCooldown;
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private Image skillImage;
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private TextMeshProUGUI skillText;
        public Character Character { get; private set; }
        #endregion

        #region Builts-In
        protected virtual void Start()
        {
            InstantiatePlayersHUD();
        }

        private void OnEnable()
        {
            skillImage.fillAmount = 0;
            skillText.gameObject.SetActive(skillCooldown.value > 0);
        }

        private void LateUpdate()
        {
            SetHUD();
        }
        #endregion

        #region Methods
        public void SetTargetCharacter(Character character)
        {
            Character = character;
        }

        /// <summary>
        /// Set sliders character reference
        /// </summary>
        private void SetHUD()
        {
            if (!Character)
                return;

            //Sliders
            healthSlider.SetPlayer(Character);
            staminaSlider.SetPlayer(Character);
            skillText.gameObject.SetActive(skillCooldown.value > 0);

            //Skill hud
            if (!Character.PlayerSM.SkillUsed)
                return;

            float amount = skillCooldown.value / Character.CharacterDatas.skillCooldown;
            skillImage.fillAmount = amount;
            skillText.gameObject.SetActive(skillCooldown.value > 0);
            skillText.text = Mathf.Ceil(skillCooldown.value).ToString();
        }

        /// <summary>
        /// Instantiate other players UI
        /// </summary>
        private void InstantiatePlayersHUD()
        {
            Character[] characters = FindObjectsOfType<Character>();
            if (characters.Length <= 0)
                return;

            foreach (Character character in characters)
            {
                if (!character || character.ViewIsMine())
                    continue;

                GameObject instance = Instantiate(playerUI, content);
                //Set instance character
            }
        }
        #endregion
    }
}
