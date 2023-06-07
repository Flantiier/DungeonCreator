using TMPro;
using UnityEngine;
using _Scripts.UI.Interfaces;
using _Scripts.Characters.Adventurers;

namespace _Scripts.UI.Gameplay
{
	public class ArcherHUD : PlayerHUD
	{
        #region Variables
        [SerializeField] private GameObject defuseField;
        [SerializeField] private TextMeshProUGUI textField;

		private Bowman _bowman;
        #endregion

        #region Builts_In
        private void Start()
        {
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
