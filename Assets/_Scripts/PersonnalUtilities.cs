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

    public abstract class TimeFunctions
    {
        public enum TimeUnit { Seconds, Minuts, Hours }

        /// <summary>
        /// Converting a duration in a selected time unit into an other
        /// </summary>
        /// <param name="time"></param>
        /// <param name="baseUnit"></param>
        /// <param name="targetUnit"></param>
        public static float ConvertTime(float time, TimeUnit baseUnit, TimeUnit targetUnit)
        {
            return GetConvertedTime(GetDurationInSeconds(time, baseUnit), targetUnit);
        }

        /// <summary>
        /// Converting a duration value into seconds
        /// </summary>
        /// <param name="time"> Duration value </param>
        /// <param name="unit"> Duration's time unit </param>
        public static float GetDurationInSeconds(float time, TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Minuts:
                    return time * 60f;

                case TimeUnit.Hours:
                    return time * 3600f;

                default:
                    return time;
            }
        }

        /// <summary>
        /// Converting a duration in seconds in a target time unit
        /// </summary>
        /// <param name="seconds"> Duration to convert (in seconds) </param>
        /// <param name="unit"> Unit to convert into </param>
        public static float GetConvertedTime(float seconds, TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Minuts:
                    return seconds / 60f;

                case TimeUnit.Hours:
                    return seconds / 3600f;

                default:
                    return seconds;
            }
        }
    }
}
