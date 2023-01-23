using UnityEngine;

namespace Personnal.Florian
{
    public static class PersonnalUtilities
    {
        public abstract class Math
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

        public abstract class Layers
        {
            /// <summary>
            /// Indicates if the layer is in the the given layerMask
            /// </summary>
            /// <param name="mask"> LayerMask to check into </param>
            /// <param name="layer"> Layer to look for </param>
            public static bool LayerMaskContains(LayerMask mask, int layer)
            {
                return mask == (mask | (1 << layer));
            }

            /// <summary>
            /// Enable a layer mask in the culling mask of the main camera
            /// </summary>
            /// <param name="layer"> Layer name </param>
            public static int ShowLayer(string layer)
            {
                return Camera.main.cullingMask |= 1 << LayerMask.NameToLayer(layer);
            }

            /// <summary>
            /// Disable a layer mask in the culling mask of the main camera
            /// </summary>
            /// <param name="layer"> Layer name </param>
            public static int HideLayer(string layer)
            {
                return Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
            }

            /// <summary>
            /// Toggeling a layer into the culling mask of the main camera
            /// </summary>
            /// <param name="layer"> Layer name </param>
            public static int ToggleLayer(string layer)
            {
                return Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer(layer);
            }
        }

        public abstract class Time
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

}
