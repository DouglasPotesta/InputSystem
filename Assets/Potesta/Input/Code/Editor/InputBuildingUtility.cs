using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Potesta.FlexInput
{
    public class InputBuildingUtility
    {
#if FLEXINPUT

        private static string assetSavePath;

        [RunOnPreProcessBuild(0)]
        private static void AddInputDefaultsToBuild()
        {
            InputControllerDefault[] defaults = InputBuildingUtility.ImportDefaultInputs(false);
            BuildTarget targetPlatform = EditorUserBuildSettings.activeBuildTarget;
            RuntimePlatform runtimePlatform = buildTargetToRuntimePlatform[targetPlatform];
            InputControllerDefault platformDefault = defaults.FirstOrDefault(x => x.runtimePlatform == runtimePlatform);
            if (platformDefault == null)
            {
                platformDefault = defaults.FirstOrDefault(x => x.runtimePlatform == Application.platform);
                if (platformDefault != null)
                {
                    Debug.LogWarning("No input default config found for " + System.Enum.GetName(typeof(RuntimePlatform), runtimePlatform) + ". It is highly sugggested you make a default for this platform otherwise it will default to the editors defaults.");
                }
                else
                {
                    Debug.LogError("No input defaults found for target platform nor editor, " +
                        "no default inputs will be included and the flexinput will fail to work." +
                        "\nAdd a default input for the target platform and the editor.");
                    return;
                }
            }
            assetSavePath = AssetDatabase.GenerateUniqueAssetPath("Assets/platformDefault.asset");
            AssetDatabase.CreateAsset(platformDefault, assetSavePath);
            BuildProcessing.BuildingStopped += CleanUpAssetCreated;
        }

        private static void CleanUpAssetCreated()
        {
            AssetDatabase.DeleteAsset(assetSavePath);
            BuildProcessing.BuildingStopped -= CleanUpAssetCreated;
        }

        private static string GetPathToDefaultInputs(bool temp)
        {
            string path = Application.dataPath.Remove(Application.dataPath.LastIndexOf("Assets"), 6);
            if (temp)
            {
                path = path + "Temp/DefaultInputs.asset";
            }
            else
            {
                path = path + "ProjectSettings/DefaultInputs.asset";
            }
            path = path.Replace('/', Path.DirectorySeparatorChar);
            return path;
        }

        public static void ExportDefaultInputs(InputControllerDefault[] inputControllerDefaults, bool temp)
        {
            string path = Application.dataPath.Remove(Application.dataPath.LastIndexOf("Assets"), 6);
            if (temp)
            {
                path = path + "Temp/DefaultInputs.asset";
            }
            else
            {
                path = path + "ProjectSettings/DefaultInputs.asset";
            }
            path = path.Replace('/', Path.DirectorySeparatorChar);
            string data = new System.String(inputControllerDefaults.SelectMany(x => JsonUtility.ToJson(x) + "|||||").ToArray());
            System.IO.File.WriteAllText(path, data);
        }


        public static bool IsTempUpToDate()
        {
            string tempsPath = GetPathToDefaultInputs(true);
            string ogPath = GetPathToDefaultInputs(false);
            System.DateTime dateTimeTemp = File.GetLastWriteTime(tempsPath);
            System.DateTime dateTimeOG = File.GetLastWriteTime(ogPath);
            return dateTimeTemp >= dateTimeOG;
        }

        public static InputControllerDefault[] ImportDefaultInputs(bool temp)
        {
            string path = Application.dataPath.Remove(Application.dataPath.LastIndexOf("Assets"), 6);
            if (temp)
            {
                path = path + "Temp/DefaultInputs.asset";
            }
            else
            {
                path = path + "ProjectSettings/DefaultInputs.asset";
            }
            path = path.Replace('/', Path.DirectorySeparatorChar);
            string data = System.IO.File.ReadAllText(path);
            string[] datas = data.Split(new string[] { "|||||" }, System.StringSplitOptions.RemoveEmptyEntries);
            InputControllerDefault[] defaults = datas.Select(x => ScriptableObject.CreateInstance<InputControllerDefault>()).ToArray();
            for (int i = 0; i < datas.Length; i++)
            {
                JsonUtility.FromJsonOverwrite(datas[i], defaults[i]);
            }
            return defaults;
        }
#endif

        [MenuItem("Edit/Project Settings/Input/Export Axes")]
        private static void ExportAxes()
        {
            var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedObject obj = new SerializedObject(inputManager);

            SerializedProperty axisArray = obj.FindProperty("m_Axes");

            if (axisArray.arraySize == 0)
                Debug.Log("No Axes");
            string names = "";
            for (int i = 0; i < axisArray.arraySize; ++i)
            {
                var axis = axisArray.GetArrayElementAtIndex(i);

                names += "\"" + axis.FindPropertyRelative("m_Name").stringValue + "\"" + ", ";
            }

            Debug.Log("string[] axesNames = new string[]{" + names.Trim(' ', ',') + "};");
        }


        private static void GetPaths(out string path, out string backUpPath, out string newInputsPath)
        {
            AssetDatabase.Refresh();
            path = Application.dataPath.Remove(Application.dataPath.LastIndexOf("Assets"), 6);
            path = path + "ProjectSettings/InputManager.asset";
            path = path.Replace('/', Path.DirectorySeparatorChar);
            backUpPath = GetFullFilePath("Input", "cs").Replace("Input.cs", "") + "BackupInputManager.FIFI";
            newInputsPath = GetFullFilePath("InputManager", "FIFI");
            backUpPath = backUpPath.Replace('/', Path.DirectorySeparatorChar);
            newInputsPath = newInputsPath.Replace('/', Path.DirectorySeparatorChar);
        }

        private static string GetFullFilePath(string fileName, string extension)
        {
            string fileEndPath = "/" + fileName + "." + extension;
            string[] assets = AssetDatabase.FindAssets(fileName);
            for (int i = 0; i < assets.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                if (assetPath.Contains(fileEndPath))
                {
                    return (Application.dataPath + assetPath.Substring(6));
                }
            }
            return null;
        }

        [MenuItem("Edit/Project Settings/Input/Upgrade (Enable)")]
        private static void ReplaceInputs()
        {
            string path, backUpPath, upgradedInputPath;
            GetPaths(out path, out backUpPath, out upgradedInputPath);
            if (File.Exists(backUpPath))
            {
                EditorApplication.Beep();
                if (!EditorUtility.DisplayDialog("Overwrite BackUp", "Warning you are about to overwrite your back up Input Manger. Is this okay?", "Yes", "Cancel"))
                {
                    return;
                }
            }

            System.IO.File.Copy(path, backUpPath, true);
            AssetDatabase.Refresh();

            EditorApplication.Beep();
            if (!EditorUtility.DisplayDialog("Overwrite Inputs", "Warning you are about to overwrite your Input Manger. Is this okay?", "Yes", "Cancel"))
            {
                return;
            }

            System.IO.File.Copy(upgradedInputPath, path, true);
            AssetDatabase.Refresh();
            EditorWindow.GetWindow<EditorWindow>("Inspector").Repaint();
        UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
        string scriptDefineSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
        string defineSymbolsToSet = scriptDefineSymbols;
        defineSymbolsToSet = defineSymbolsToSet.Replace("FLEXINPUT", "");
        defineSymbolsToSet = defineSymbolsToSet + " FLEXINPUT";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup, defineSymbolsToSet);
        //        System.IO.File.Open(Application.dataPath + EditorApplication.applicationContentsPath)
    }

        [MenuItem("Edit/Project Settings/Input/Revert (Disable)")]
        private static void UseOldInputs()
        {
            string path, backUpPath, empty;
            GetPaths(out path, out backUpPath, out empty);
            if (!File.Exists(backUpPath))
            {
                EditorApplication.Beep();
                EditorUtility.DisplayDialog("No Backup Found", "Sorry no backup of your input manager exists. Aborting.", "Okay");
                return;
            }
            System.IO.File.Copy(backUpPath, path, true);
            AssetDatabase.Refresh();
            EditorWindow.GetWindow<EditorWindow>("Inspector").Repaint();
            UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string scriptDefineSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
            string defineSymbolsToSet = scriptDefineSymbols;
            defineSymbolsToSet = defineSymbolsToSet.Replace("FLEXINPUT", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup, defineSymbolsToSet);
        }

#pragma warning disable 414
        private readonly static Dictionary<BuildTarget, RuntimePlatform> buildTargetToRuntimePlatform = new Dictionary<BuildTarget, RuntimePlatform>()
        {
            {BuildTarget.Android, RuntimePlatform.Android },
            {BuildTarget.iOS, RuntimePlatform.IPhonePlayer },
            {BuildTarget.WSAPlayer, RuntimePlatform.WSAPlayerX86 },
            {BuildTarget.NoTarget, RuntimePlatform.WindowsPlayer },
            {BuildTarget.PS4, RuntimePlatform.PS4 },
            {BuildTarget.PSM, RuntimePlatform.PSM },
            {BuildTarget.StandaloneLinux, RuntimePlatform.LinuxPlayer },
            {BuildTarget.StandaloneLinux64, RuntimePlatform.LinuxPlayer },
            {BuildTarget.StandaloneLinuxUniversal, RuntimePlatform.LinuxPlayer },
            {BuildTarget.StandaloneOSX, RuntimePlatform.OSXPlayer},
            {BuildTarget.StandaloneWindows, RuntimePlatform.WindowsPlayer },
            {BuildTarget.StandaloneWindows64, RuntimePlatform.WindowsPlayer },
            {BuildTarget.Switch, RuntimePlatform.Switch },
            {BuildTarget.Tizen, RuntimePlatform.TizenPlayer },
            {BuildTarget.tvOS, RuntimePlatform.tvOS },
            {BuildTarget.WebGL, RuntimePlatform.WebGLPlayer },
            {BuildTarget.XboxOne, RuntimePlatform.XboxOne }
        };
#pragma warning restore 414
    }
}
