using UnityEngine;
using UnityEngine.UI;
using _Scripts.Characters;

namespace _Scripts.UI
{
    public class CharacterSlider : MonoBehaviour
    {
        #region Variables
        protected Slider _slider;

        public Characters.Character Character { get; protected set; }
        #endregion

        #region Builts_In
        public virtual void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public virtual void Update()
        {
            if (!Character)
                return;

            UpdateSlider();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the player to display
        /// </summary>
        public void SetPlayer(Characters.Character character)
        {
            Character = character;

            SetSliderBounds();
        }

        /// <summary>
        /// Set slider bounds
        /// </summary>
        protected virtual void SetSliderBounds() { }

        /// <summary>
        /// Update slider value
        /// </summary>
        protected virtual void UpdateSlider() { }
        #endregion
    }
}
