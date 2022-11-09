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
            if (!_image || !_rectTransform)
                return;

            _image.enabled = reticleParamaters.enabled;

            if (!_image.enabled)
                return;

            float size = reticleParamaters.size;
            _rectTransform.localScale = new Vector2(size, size);
            _image.color = reticleParamaters.color;
        }
        #endregion
    }
}
