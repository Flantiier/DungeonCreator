using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters;

namespace _Scripts.UI.Gameplay
{
	public class CharacterUI : MonoBehaviour
	{
		#region Variables
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image image;
		[SerializeField] private Sprite[] icons;

		private Character _character;
        #endregion

        #region Builts_In
        private void LateUpdate()
        {
			if (!_character) 
			{
				gameObject.SetActive(false);
				return;
			}

			UpdateSlider();
        }
        #endregion

        #region Methods
        public void SetCharacter(Character character)
		{
			_character = character;
			nameField.text = character.View.Controller.NickName;
			healthSlider.maxValue = _character.CharacterDatas.health;

			switch (character.GetType().ToString())
			{
				case "_Scripts.Characters.Adventurers.Warrior":
					image.sprite = icons[0];
					break;
                case "_Scripts.Characters.Adventurers.Bowman":
					image.sprite = icons[1];
                    break;
                case "_Scripts.Characters.Adventurers.Wizard":
					image.sprite = icons[2];
                    break;
            }

			UpdateSlider();
		}

		private void UpdateSlider()
		{
			healthSlider.value = _character.CurrentHealth;
		}
		#endregion
	}
}
