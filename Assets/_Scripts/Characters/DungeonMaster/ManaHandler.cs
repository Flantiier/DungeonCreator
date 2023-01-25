using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.Characters.DungeonMaster
{
	public class ManaHandler : MonoBehaviour
	{
		#region Variables
		[TitleGroup("Properties")]
		[SerializeField] private float maxMana = 100f;
        [SerializeField] private float recovery = 5f;

        private Coroutine _recoveryRoutine;
        #endregion

        #region Properties
        public float CurrentMana { get; private set; }
        #endregion

        #region Builts_In
        private void Awake()
        {
            CurrentMana = maxMana;
        }

        private void LateUpdate()
        {
            if (CurrentMana >= maxMana || _recoveryRoutine != null)
                return;

            _recoveryRoutine = StartCoroutine("RecoveryRoutine");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Decrease the amount from the current mana
        /// </summary>
        /// <param name="amount"> Used mana amount </param>
        public void UseMana(float amount)
        {
            if (!HasMuchMana(amount))
                return;

            CurrentMana -= amount;
        }

        /// <summary>
        /// Returns if the amount can be decreased from current mana
        /// </summary>
        /// <param name="amount"> Used mana </param>
        public bool HasMuchMana(float amount)
        {
            return CurrentMana - amount >= 0;
        }

        /// <summary>
        /// Increase the current mana coroutine
        /// </summary>
        private IEnumerator RecoveryRoutine()
        {
            while (CurrentMana < maxMana)
            {
                CurrentMana += recovery * Time.deltaTime;
                yield return null;
            }

            CurrentMana = maxMana;
            _recoveryRoutine = null;
        }
        #endregion
    }
}
