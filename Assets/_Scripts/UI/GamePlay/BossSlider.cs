using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters.DungeonMaster;

namespace _Scripts.UI.Gameplay
{
	public class BossSlider : MonoBehaviour
	{
        private Slider _slider;
		private BossController _boss;

        private void Awake()
        {
            _boss = FindObjectOfType<BossController>();
            _slider = GetComponent<Slider>();
            _slider.maxValue = _boss.Datas.health;
        }

        private void LateUpdate()
        {
            _slider.value = _boss.CurrentHealth; 
        }
    }
}
