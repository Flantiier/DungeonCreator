using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class AnimatedTextField : MonoBehaviour
	{
		[SerializeField] private float animationRate = 1f;
		[SerializeField] private string[] texts;
		private int _index;
		private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
			if (texts.Length <= 0)
				return;

			StartCoroutine(AnimateText());
        }

        private IEnumerator AnimateText()
		{
			_text.text = texts[0];
			_index = _index >= texts.Length - 1 ? 0 : _index;

			yield return new WaitForSecondsRealtime(animationRate);
			StartCoroutine(AnimateText());
		}
	}
}
