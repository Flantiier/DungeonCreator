using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class AnimatedTextField : MonoBehaviour
	{
        #region Variables
        [SerializeField] private float animationRate = 1f;
		[SerializeField] private bool useBaseString;
		[SerializeField] private string[] texts;
		private int _index;
		private string _baseString;
		private TextMeshProUGUI _text;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
			_baseString = _text.text;
        }

        private void Start()
        {
			if (texts.Length <= 0)
				return;

			StartCoroutine(AnimateText());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Animating the text at run time
        /// </summary>
        private IEnumerator AnimateText()
		{
			_text.text = useBaseString ? _baseString + texts[_index] : texts[_index];
			_index++;

			if (_index >= texts.Length)
				_index = 0;

			yield return new WaitForSecondsRealtime(animationRate);
			StartCoroutine(AnimateText());
		}

		/// <summary>
		/// Set the base text of the field
		/// </summary>
		public void SetBaseText(string text)
		{
			_baseString = text;

			//Reset coroutine
			StopAllCoroutines();
			StartCoroutine(AnimateText());
		}
        #endregion
    }
}
