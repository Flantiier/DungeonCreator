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
        [SerializeField] private int removeFromStart = 0;
        [SerializeField] private int removeFromEnd = 0;

        private void Rename()
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                //Get the string
                string name = Selection.objects[i].name;
                name = prefix + name + suffix;
                
            }
        }

        [MenuItem("Tools/Rename Objects")]
        private static void OpenWindow()
        {
            GetWindow<RenameObjects>().Show();
        }
    }
}
