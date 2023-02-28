using UnityEngine;
using TMPro;

namespace _Scripts.UI
{
	public class GameTimerDisplay : MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private FloatVariable timer;

		private void Update()
		{
			UpdateText();
		}

        public void UpdateText()
        {
            if (!timer || !textMeshPro)
                return;

            textMeshPro.text = timer.value.ToString();
        }
    }
}
