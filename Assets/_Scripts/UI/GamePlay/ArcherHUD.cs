using System.Collections;
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
        private void Awake()
        {
            defuseField.SetActive(false);
        }

        protected override IEnumerator Start()
        {
            _bowman = Character.GetComponent<Bowman>();
            return base.Start();
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