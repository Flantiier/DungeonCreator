using System.Collections;
using UnityEngine;

namespace Adventurer
{
    [System.Serializable]
    public class Dodge
    {
        public AnimationCurve dodgeCurve;

        public float _dodgeTimer { get; private set; }
        public bool _isDodging { get; private set; }

        public IEnumerator Dodging(CharacterController cc, Vector3 movement)
        {
            _isDodging = true;

            while (_dodgeTimer <= dodgeCurve.keys[dodgeCurve.length - 1].time)
            {
                _dodgeTimer += Time.deltaTime;
                cc.Move(movement * /*dodgeCurve.Evaluate(_dodgeTimer)*/ 1f * Time.deltaTime);
            }

            yield return new WaitForSecondsRealtime(0.5f);

            _dodgeTimer = 0;
            _isDodging = false;
        }
    }
}
