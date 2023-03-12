using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace _Scripts.Editor
{
    public class RenameObjects : OdinEditorWindow
    {
        [SerializeField] private string prefix;
        [SerializeField] private string suffix;
        [SerializeField, Min(0)] private int removeFromStart = 0;
        [SerializeField, Min(0)] private int removeFromEnd = 0;

        [Button("Rename select(ion", ButtonSizes.Medium)]
        private void Rename()
        {
            if (Selection.objects.Length <= 0)
                return;

            for (int i = 0; i < Selection.objects.Length; i++)
            {
                //Get the string
                Object obj = Selection.objects[i];
                string name = obj.name;
                name = removeFromStart <= 0 ? name : name.Remove(0, removeFromStart);
                name = removeFromEnd <= 0 ? name : name.Remove(name.Length - removeFromEnd);
                obj.name = prefix + name + suffix;
            }

            ResetValue();
        }

        private void ResetValue()
        {
            prefix = string.Empty;
            suffix = string.Empty;
            removeFromStart = 0;
            removeFromEnd = 0;
        }

        [MenuItem("Tools/Rename Objects")]
        private static void OpenWindow()
        {
            GetWindow<RenameObjects>().Show();
        }
    }
}
