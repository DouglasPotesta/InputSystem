using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Build;
using System;
using System.Linq;
using System.Reflection;
namespace Potesta
{
    public class BuildProcessing : IPreprocessBuild, IActiveBuildTargetChanged
    {
        public static MethodInfo[] GetAllMethodsWithAttributeType(Type type) { return GetAllMethodsWithAttributeType(type, true); }
        public static MethodInfo[] GetAllMethodsWithAttributeType(Type type, bool IncludeEditor)
        {

            List<MethodInfo> methodInfos = new List<MethodInfo>();
            methodInfos.AddRange(Assembly.GetAssembly(typeof(GameInitializer)).GetTypes().SelectMany(t => t.GetMethods()
              .Where(m => m.GetCustomAttributes(type, false).Length > 0)));
            if (IncludeEditor)
            {
                Type[] types = Assembly.GetCallingAssembly().GetTypes();
                methodInfos.AddRange(types.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                  .Where(m => m.GetCustomAttributes(type, false).Length > 0)));
            }
            return methodInfos.ToArray();
        }
        public static Type[] GetAllTypesWithAttributeType(Type type) { return GetAllTypesWithAttributeType(type, true); }
        public static Type[] GetAllTypesWithAttributeType(Type type, bool IncludeEditor)
        {

            List<Type> methodInfos = new List<Type>();
            methodInfos.AddRange(Assembly.GetAssembly(typeof(GameInitializer)).GetTypes()
              .Where(m => m.GetCustomAttributes(type, false).Length > 0));
            if (IncludeEditor)
            {
                methodInfos.AddRange(Assembly.GetCallingAssembly().GetTypes()
                  .Where(m => m.GetCustomAttributes(type, false).Length > 0));
            }
            return methodInfos.ToArray();
        }
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            PreprocessBuild(target, path);
        }
        public static bool ProcessAllScenes(Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessAllScenes("customProcess", false, customProcess);
        }
        public static bool ProcessAllScenes(string processName, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessAllScenes(processName, false, customProcess);
        }
        public static bool ProcessAllScenes(bool saveScenes, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessAllScenes("customProcess", saveScenes, customProcess);
        }
        public static bool ProcessAllScenes(string processName, bool saveScenes, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            bool Completed = true;
            SceneSetup[] sceneSetups = EditorSceneManager.GetSceneManagerSetup();
            if (saveScenes)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return false;
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            try
            {
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    if (EditorBuildSettings.scenes[i].enabled)
                    {
                        Scene scene = EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Single);
                        if (EditorUtility.DisplayCancelableProgressBar("Processing Scenes", "Processing \"" + scene.name + "\" with " + processName, (float)i / EditorBuildSettings.scenes.Length))
                        {
                            Debug.LogWarning("Scene Processing canceled by user.");
                            Completed = false;
                            break;
                        }
                        customProcess(scene);
                        if (saveScenes)
                        {
                            EditorSceneManager.MarkSceneDirty(scene);
                            EditorSceneManager.SaveOpenScenes();
                            EditorSceneManager.CloseScene(scene, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            EditorUtility.ClearProgressBar();
            EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);
            return Completed;
        }

        public static bool ProcessScene(string sceneName, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessScene(sceneName, "customProcess", false, customProcess);
        }

        public static bool ProcessScene(string sceneName, string processName, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessScene(sceneName, processName, false, customProcess);
        }

        public static bool ProcessScene(string sceneName, bool saveScenes, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            return ProcessScene(sceneName, "customProcess", saveScenes, customProcess);
        }

        public static bool ProcessScene(string sceneName, string processName, bool saveScenes, Action<UnityEngine.SceneManagement.Scene> customProcess)
        {
            bool Completed = true;
            SceneSetup[] sceneSetups = EditorSceneManager.GetSceneManagerSetup();
            if (saveScenes)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return false;
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            try
            {
                Scene scene = EditorSceneManager.OpenScene(EditorBuildSettings.scenes.First(x=>x.path.Contains(sceneName)).path, OpenSceneMode.Single);
                customProcess(scene);
                if (saveScenes)
                {
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveOpenScenes();
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            EditorUtility.ClearProgressBar();
            EditorSceneManager.RestoreSceneManagerSetup(sceneSetups);
            return Completed;
        }
        public static void PreprocessBuild(BuildTarget target, string path)
        {
            if (EditorUserBuildSettings.buildScriptsOnly) { return; }
            Type ROPPBAType = typeof(RunOnPreProcessBuildAttribute);
            MethodInfo[] bMethods = GetAllMethodsWithAttributeType(ROPPBAType, true).OrderBy(item => ((RunOnPreProcessBuildAttribute)item.GetCustomAttributes(ROPPBAType, true).First()).order).ToArray();
            for (int ii = 0; ii < bMethods.Length; ii++)
            {
                ParameterInfo[] paramaters = bMethods[ii].GetParameters();
                if (paramaters.Length == 0)
                {
                    bMethods[ii].Invoke(null, new object[] { });
                    Debug.Log("Succesfully Ran" + bMethods[ii].Name);
                }
                else
                {
                    Debug.LogError(bMethods[ii] + " does not follow the proper signature. Needs to be void method()");
                }
            }


            Type ROPPsBAType = typeof(RunOnPreProcessScenesBuildAttribute);
            MethodInfo[] sMethods = GetAllMethodsWithAttributeType(ROPPsBAType, true).OrderBy(item => ((RunOnPreProcessScenesBuildAttribute)item.GetCustomAttributes(ROPPsBAType, true).First()).order).ToArray();
            if (sMethods.Length == 0) { return; }
            ProcessAllScenes("PreProcesses", (Scene scene) =>
            {
                for (int ii = 0; ii < sMethods.Length; ii++)
                {
                    ParameterInfo[] paramaters = sMethods[ii].GetParameters();
                    if (paramaters.Length == 1 && paramaters[0].ParameterType == typeof(Scene))
                    {
                        sMethods[ii].Invoke(null, new object[] { scene });
                        Debug.Log("Succesfully Ran" + sMethods[ii].Name);
                    }
                    else
                    {
                        Debug.LogError(sMethods[ii] + " does not follow the proper signature. Needs to be void method(Scene _scene)");
                    }
                }
            });
        }

        private static bool hasStartedBuilding;
        public static Action BuildingStopped;
        [RunOnPreProcessBuild(-100)] // TODO Convert this to an attribute.
        private static void BuildCheckInit() {
            hasStartedBuilding = BuildPipeline.isBuildingPlayer;
            EditorApplication.update += BuildCheck;
        }
        private static void BuildCheck()
        {
            if (BuildPipeline.isBuildingPlayer)
            {
                if (!hasStartedBuilding)
                {
                    hasStartedBuilding = true;
                }
            }
            else if (hasStartedBuilding)
            {
                if (BuildingStopped != null)
                {
                    BuildingStopped();
                }
                Debug.Log("Buildng has ended thank you");
                EditorApplication.update -= BuildCheck;
                hasStartedBuilding = false;
            }
            
        }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            ActiveBuildTargetChanged(previousTarget, newTarget);
        }
        public static void ActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            MethodInfo[] types = GetAllMethodsWithAttributeType(typeof(RunOnTargetPlatformChangedAttribute));
            for (int ii = 0; ii < types.Length; ii++)
            {
                ParameterInfo[] paramaters = types[ii].GetParameters();
                if (paramaters.Length == 2 && paramaters[0].ParameterType == typeof(BuildTarget) && paramaters[1].ParameterType == typeof(BuildTarget) && types[ii].IsStatic)
                {
                    types[ii].Invoke(null, new object[] { previousTarget, newTarget });
                    Debug.Log("Succesfully Ran" + types[ii].Name);
                }
                else
                {
                    if (!types[ii].IsStatic)
                    {
                        Debug.LogError(types[ii] + " must be static.");
                    }
                    if (paramaters.Length == 2 && paramaters[0].ParameterType == typeof(BuildTarget) && paramaters[1].ParameterType == typeof(BuildTarget))
                    {
                        Debug.LogError(types[ii] + " does not follow the proper signature. Needs to be void method(Scene _scene)");
                    }
                }
            }
        }
    }
}