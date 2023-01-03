using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using _Scripts.NetworkScript;
using _Scripts.Characters;

namespace _Scripts.GameplayFeatures
{
	[RequireComponent(typeof(Rigidbody))]
	public class Trigger : MonoBehaviour
	{
		#region Variables
		protected List<Character> _charactersInTrigger;
		#endregion

		#region Builts_In
		private void Awake()
		{
			_charactersInTrigger = new List<Character>();
		}

		public virtual void OnTriggerEnter(Collider other)
		{
			if (!other.TryGetComponent(out Character character))
				return;

			CharacterEnterTrigger(character);
		}

        public virtual void OnTriggerExit(Collider other)
		{
            if (!other.TryGetComponent(out Character character))
                return;

			CharacterLeftTrigger(character);
        }
		#endregion

		#region Methods
		private void CharacterEnterTrigger(Character character)
		{
			if (_charactersInTrigger.Contains(character))
				return;

			_charactersInTrigger.Add(character);
		}

		private void CharacterLeftTrigger(Character character)
		{
            if (!_charactersInTrigger.Contains(character))
                return;

            _charactersInTrigger.Remove(character);
        }
		#endregion
	}
}
