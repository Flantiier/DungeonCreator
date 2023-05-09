using UnityEngine;
using Utils;
using Sirenix.OdinInspector;
using static Utils.Utilities.Time;

namespace _ScriptableObjects.GameManagement
{
    [CreateAssetMenu(fileName = "New Game Properties", menuName = "SO/Game Management/Game Properties"), InlineEditor]
    public class GameProperties : ScriptableObject
    {
        [BoxGroup("Game Steps", CenterLabel = true)]
        [Tooltip("Le temps de la phase de preparation au début de la partie"), LabelText("Start Phase")]
        public GameStep startPhase = new GameStep(20f, TimeUnit.Seconds);
        [BoxGroup("Game Steps", CenterLabel = true)]
        [Tooltip("Le temps total de la game"), LabelText("Game")]
        public GameStep game = new GameStep(20f, TimeUnit.Minuts);
    }

    #region TimeVariable
    [System.Serializable, HideLabel]
    public class GameStep
    {
        [HorizontalGroup("Group")]
        public float duration;
        [HorizontalGroup("Group", 75), ShowInInspector, HideLabel]
        public TimeUnit TimeUnit
        {
            get => _unit;
            set { ConvertTime(value); }
        }
        [LabelText("TimeOut Event"), Tooltip("Event to raise after timer")]
        public GameEvent gameEvent;

        private TimeUnit _unit;

        public GameStep(float _duration, TimeUnit _unit)
        {
            duration = _duration;
            TimeUnit = _unit;
        }

        private void ConvertTime(TimeUnit unit)
        {
            duration = Utilities.Time.ConvertTime(duration, TimeUnit, unit);
            _unit = unit;
        }
        #endregion

    }
}
