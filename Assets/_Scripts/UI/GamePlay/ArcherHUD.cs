using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.UI.Interfaces;
using _Scripts.Characters.Adventurers;

namespace _Scripts.UI.Gameplay
{
	public class ArcherHUD : PlayerHUD
	{
        #region Variables
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private GameObject defuseField;
        [FoldoutGroup("Skill GUI")]
        [SerializeField] private TextMeshProUGUI textField;

        private Bowman _bowman;
        #endregion

        #region Builts_In
        protected override void Start()
        {
            base.Start();

            _bowman = Character.GetComponent<Bowman>();
            defuseField.SetActive(false);
        }

        private void Update()
        {
            ShowUI();
        }
        #endregion

        #region Methods
        private void ShowUI()
        {
            if (!_bowman)
                return;

            defuseField.SetActive((_bowman.TrapDetected && !_bowman.PlayerSM.SkillUsed) || _bowman.IsDefusing);

            string text;
            if (_bowman.IsDefusing)
                text = $"Desamorcage du piege {Mathf.Ceil(_bowman.TargetDefuseTime - _bowman.CurrentDefuseTime)}";
            else
                text = $"F : Desamorcer le piege";

            textField.text = text;
        }
        #endregion
    }
}