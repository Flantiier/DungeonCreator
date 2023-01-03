using System;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace _Editor.Scripts
{
    public class SceneNavigator : OdinMenuEditorWindow
    {
        [MenuItem("Tools/DC/SceneNavigator")]
        private static void OpenWindow()
        {
            GetWindow<SceneNavigator>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            tree.Add("Scene Loader", new SceneLoader());

            return tree;
        }

        public class SceneLoader
        {
            [InfoBox("Ce tool permet de charger des scenes rapidements, vous rentrez le nom de la scene que vous voulez. Ensuite pour la charger, il faut appuyer sur le petit bouton")]

            [LabelText("Scene Folder path")] public string path = "Assets/_Scenes/";

            [ShowInInspector]
            public List<ArrayElement> loadouts = new List<ArrayElement>();

            [Serializable]
            public class ArrayElement
            {
                [ShowInInspector]
                [HorizontalGroup("Loading"), LabelWidth(60)]
                public string scene = "";

                [HorizontalGroup("Loading")]
                [Button("Load")]
                public void LoadSceneFromName()
                {
                    string _path = /*path + */scene + ".unity";

                    if (scene.Length <= 0)
                        return;

                    if (SceneManager.GetActiveScene().name == scene || SceneManager.GetSceneByName(scene).isLoaded ||
                            SceneManager.GetSceneByName(scene).IsValid())
                        return;

                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(_path);
                }
            }
        }
    }
}
