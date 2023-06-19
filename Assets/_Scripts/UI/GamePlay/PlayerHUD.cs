using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using _Scripts.UI.Gameplay;
using _Scripts.Characters;

namespace _Scripts.UI.Interfaces
{
    public class PlayerHUD : MonoBehaviour
    {
        #region Variables/Properties
        [FoldoutGroup("Players panel")]
        [SerializeField] private GameObject mainPanel;
        [FoldoutGroup("Players panel")]
        [SerializeField] private Transform content;
        [FoldoutGroup("Players panel")]
        [SerializeField] private CharacterUI playerUI;

        [TitleGroup("Sliders")]
        [SerializeField] private CharacterSlider healthSlider;
        [TitleGroup("Sliders")]
        [SerializeField] private CharacterSlider staminaSlider;

        [FoldoutGroup("Skill GUI")]
        [SerializeField] private FloatVariable skillCooldown;
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private Image skillImage;
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private TextMeshProUGUI skillText;

        [FoldoutGroup("Feedback")]
        [SerializeField] private CanvasGroup hitFeedback;
        [FoldoutGroup("Feedback")]
        [SerializeField] private float hideSpeed = 2f;

        public Character Character { get; private set; }
        #endregion

        #region Builts-In
        protected virtual IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            InstantiatePlayersHUD();
        }

        private void OnEnable()
        {
            hitFeedback.alpha = 0f;
            skillImage.fillAmount = 0;
            skillText.gameObject.SetActive(skillCooldown.value > 0);

            if(Character)
                Character.OnCharacterDamaged += ShowHitFeedback;
        }

        private void OnDisable()
        {
            if(Character)
                Character.OnCharacterDamaged -= ShowHitFeedback;
        }

        private void LateUpdate()
        {
            SetHUD();
            HideHitFeedback();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the tracked character
        /// </summary>
        /// <param name="character"></param>
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
        /// Enable or disable the player HUD
        /// </summary>
        public virtual void EnableHUD(bool enabled)
        {
            mainPanel.SetActive(enabled);
            content.gameObject.SetActive(enabled);
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

                CharacterUI instance = Instantiate(playerUI, content);
                instance.SetCharacter(character);
            }
        }

        /// <summary>
        /// Enable hit feedback
        /// </summary>
        private void ShowHitFeedback()
        {
            Debug.Log("coucou");
            hitFeedback.alpha = 1f;
        }

        /// <summary>
        /// Slowly dissapear the hit feedback
        /// </summary>
        private void HideHitFeedback()
        {
            if (hitFeedback.alpha <= 0)
                return;

            hitFeedback.alpha -= Time.deltaTime * hideSpeed;
        }
        #endregion
    }
}
