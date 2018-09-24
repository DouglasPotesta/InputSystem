using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
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
#endif
#if UNITY_EDITOR
        [RunOnPreProcessBuild]
        [RunOnGameInitialized]
        private static void GrabInitializationNecessaryComponents()
        {
            Type type = typeof(IncludeInStartUpAttribute);
            List<Type> types = new List<Type>();
            types.AddRange(Assembly.GetAssembly(typeof(GameInitializer)).GetTypes()
              .Where(m => m.GetCustomAttributes(type, false).Length > 0));
            for (int i = 0; i < types.Count; i++)
            {
                GameInitializer.assets().AddRange(AssetDatabase.FindAssets("t:" + types[i].Name).Select(x => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(x), types[i])).Where(x => x != null && !GameInitializer.assets().Contains(x)).ToArray());
            }
        }
        private static void GrabInitializationComponents(AsyncOperation asyncOperation)
        {

            asyncOperation.completed -= GrabInitializationComponents;
        }
#endif
    }
}