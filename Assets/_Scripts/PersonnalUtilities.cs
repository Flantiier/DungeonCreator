using UnityEngine;

namespace _Scripts.Utilities.Florian
{
    public static class PersonnalUtilities
    {
        public abstract class MathFunctions
        {
            /// <summary>
            /// Compares two floating point values and returns true if they are approximately similar
            /// </summary>
            /// <param name="a"> Variable to compare </param>
            /// <param name="b"> Compared number </param>
            /// <param name="tolerance"> Tolerance between them </param>
            public static bool ApproximationRange(float a, float b, float tolerance)
            {
                return (Mathf.Abs(a - b) < tolerance);
            }
        }
    }
}
