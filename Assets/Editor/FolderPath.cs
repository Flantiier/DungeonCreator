using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace _Scripts.Editor
{
	[CreateAssetMenu(fileName = "Folder Path", menuName = "Editor/Folder Path")]
	public class FolderPath : ScriptableObject
	{
		[HorizontalGroup("Horizontal"), VerticalGroup("Horizontal/G1"), BoxGroup("Horizontal/G1/Box", LabelText = "Gameplay Resources", CenterLabel = true)]
		[SerializeField] private FolderPathStruct resourcesFolder = new FolderPathStruct("Assets/Resources");
        [VerticalGroup("Horizontal/G1"), BoxGroup("Horizontal/G1/Box")]
        [SerializeField] private FolderPathStruct prefabFolder = new FolderPathStruct("Assets/_Prefabs");
        [VerticalGroup("Horizontal/G1"), BoxGroup("Horizontal/G1/Box")]
        [SerializeField] private FolderPathStruct scriptFolder = new FolderPathStruct("Assets/_Scripts");
        [VerticalGroup("Horizontal/G1"), BoxGroup("Horizontal/G1/Box")]
        [SerializeField] private FolderPathStruct scriptableObjects = new FolderPathStruct("Assets/_ScriptablesObjects");

        [VerticalGroup("Horizontal/G2"), BoxGroup("Horizontal/G2/Box", LabelText = "Graphics Resources", CenterLabel = true)]
        [SerializeField] private FolderPathStruct artworksFolder = new FolderPathStruct("Assets/_2DArts");
        [VerticalGroup("Horizontal/G2"), BoxGroup("Horizontal/G2/Box", LabelText = "Graphics Resources")]
        [SerializeField] private FolderPathStruct modelsFolder = new FolderPathStruct("Assets/_3DModels");
        [VerticalGroup("Horizontal/G2"), BoxGroup("Horizontal/G2/Box", LabelText = "Graphics Resources")]
        [SerializeField] private FolderPathStruct materialsFolder = new FolderPathStruct("Assets/_Materials");
        [VerticalGroup("Horizontal/G2"), BoxGroup("Horizontal/G2/Box", LabelText = "Graphics Resources")]
        [SerializeField] private FolderPathStruct texturesFolder = new FolderPathStruct("Assets/_Textures");

        [TitleGroup("Custom Paths")]
        [SerializeField] private FolderPathStruct[] paths;
	}

	[Serializable, HideLabel]
	public struct FolderPathStruct
	{
		[InlineButton("SelectFolder"), LabelWidth(100), LabelText("")]
        [SerializeField] private string folder;

		public FolderPathStruct(string value)
		{
            string path = value.Length <= 0 ? "Assets" : value;
			folder = path;
		}

        private void SelectFolder()
        {
            EditorUtility.FocusProjectWindow();
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);

            var pt = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
            var ins = pt.GetField("s_LastInteractedProjectBrowser", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
            var showDirMeth = pt.GetMethod("ShowFolderContents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            showDirMeth.Invoke(ins, new object[] { obj.GetInstanceID(), true });
        }
    }
}
