using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Potesta {
    public class GameInitializerEditor : GameInitializer {

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void ConfigureEditorForInitialization()
        { 
            UnityEditor.EditorBuildSettingsScene[] sceneSetups = UnityEditor.EditorBuildSettings.scenes;
            if (!sceneSetups.Any(x => x.path.Contains(SCENE_NAME + ".unity")))
            {
                string path = AssetDatabase.FindAssets(SCENE_NAME).Where(x => AssetDatabase.GUIDToAssetPath(x).Contains("/" + SCENE_NAME + ".unity")).First();
                path = AssetDatabase.GUIDToAssetPath(path);
                EditorBuildSettings.scenes = EditorBuildSettings.scenes.Concat(new EditorBuildSettingsScene[] { new EditorBuildSettingsScene(path, true) }).ToArray();
                Debug.Log("Added" + SCENE_NAME + "scene to build settings.");
            }
        }
        private static void GrabInitializationComponents(AsyncOperation asyncOperation)
        {
            asyncOperation.completed -= GrabInitializationComponents;
        }
        [RunOnPreProcessBuild]
        private static void IncludeAssetsInBuild()
        {
            BuildProcessing.ProcessScene(GameInitializer.SCENE_NAME, true, (Scene scene) =>
            {
                GameInitializer gameInitializer = GameObject.FindObjectOfType<GameInitializer>();
                Type[] types = BuildProcessing.GetAllTypesWithAttributeType(typeof(IncludeInStartUpAttribute), false);
                for (int i = 0; i < types.Length; i++)
                {
                    string[] assetsToInclude = AssetDatabase.FindAssets("t:" + types[i].Name);
                    for (int ii = 0; ii < assetsToInclude.Length; ii++)
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(assetsToInclude[ii]);
                        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, types[i]);
                        gameInitializer.AddAsset(asset);
                        if (EditorUtility.DisplayCancelableProgressBar("Adding Assets", "Processing " + types[i].Name + "s: \"" + assetsToInclude + "\"", ((float)i / types.Length) + ((float)ii / assetsToInclude.Length) / types.Length))
                        {
                            Debug.LogWarning("Adding Assets To Build canceled by user.");
                            break;
                        }
                    }
                }
            });
        }
#endif
    }
}