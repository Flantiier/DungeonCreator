using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.GameplayFeatures.PhysicsAdds
{
    public class PhysicsDetection : MonoBehaviour
    {
        #region Variables
        [TitleGroup("Helpers")]
        [SerializeField] protected bool enableHelpers = true;
        [ShowIf("enableHelpers"), SerializeField] protected Color helpersColor = new Color(0f, 200f, 0f, 255f);

        public bool Enabled { get; set; } = true;
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if something is detected
        /// </summary>
        public virtual bool IsDetecting() { return Enabled; }
        #endregion
    }
}