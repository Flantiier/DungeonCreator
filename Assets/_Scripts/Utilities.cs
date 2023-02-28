using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class Utilities
    {
        #region Others
        /// <summary>
        /// Return a random value between a mininmum and a maximum
        /// </summary>
        public static float RandomValue(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Indicates if the given scene exists and is set up in the build settings
        /// </summary>
        /// <param name="name">Scene name to check</param>
        public static bool ExistingScene(string name)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (name != AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)).name)
                    continue;
                else
                    return true;
            }

            return false;
        }
        #endregion

        #region Math Class
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
        #endregion

        #region Layers Class
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
        #endregion

        #region Time Class
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
                return GetConvertedTime(GetTimeInSeconds(time, baseUnit), targetUnit);
            }

            /// <summary>
            /// Converting a duration value into seconds
            /// </summary>
            /// <param name="time"> Duration value </param>
            /// <param name="unit"> Duration's time unit </param>
            public static float GetTimeInSeconds(float time, TimeUnit unit)
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
        #endregion

        #region Colors Class
        public abstract class ColorProperties
        {
            public static Color RandomColor(float min, float max)
            {
                float r = RandomValue(min, max);
                float g = RandomValue(min, max);
                float b = RandomValue(min, max);
                return new Color(r, g, b, 1);
            }
        }
        #endregion
    }

}
