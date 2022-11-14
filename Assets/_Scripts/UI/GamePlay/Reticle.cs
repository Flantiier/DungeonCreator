using UnityEngine;
using UnityEngine.UI;
using _ScriptablesObjects.Settings.UI;

namespace _Scripts.UI.Gameplay
{
    public class Reticle : MonoBehaviour
    {
        #region Variables
        [Header("Reticle infos")]
        [SerializeField] private ReticleDatas reticleParamaters;

        private RectTransform _rectTransform;
        private Image _image;
        #endregion

        #region Builts_In
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            SetReticleParameters();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Setting reticle's enable state, size and oppacity
        /// </summary>
        private void SetReticleParameters()
        {
            if(_image.enabled != reticleParamaters.enabled)
                _image.enabled = reticleParamaters.enabled;

            if (!reticleParamaters.enabled)
                return;

            SetReticleSize();
            SetReticleOppacity();
        }

        /// <summary>
        /// Return the size sets in the user settings
        /// </summary>
        private void SetReticleSize()
        {
            if (!_rectTransform)
                return;

            float size = reticleParamaters.size;
            _rectTransform.localScale = new Vector2(size, size);
        }

        /// <summary>
        /// Return the oppacity value sets in the user settings
        /// </summary>
        private void SetReticleOppacity()
        {
            if (!_image)
                return;

            Color newColor = Color.white;
            newColor.a = reticleParamaters.oppacity;
            _image.color = newColor;
        }
        #endregion
    }
}
