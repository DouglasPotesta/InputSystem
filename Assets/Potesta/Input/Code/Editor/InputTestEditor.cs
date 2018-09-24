using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace Potesta.FlexInput
{
    public class InputTestEditor
    {

        [MenuItem("Export Input Axes", menuItem = "Edit/Project Settings/Input/Export Axes")]
        public static void ExportAxes()
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
            string[] assets = AssetDatabase.FindAssets("Input");
            backUpPath = "";
            for (int i = 0; i < assets.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                if (assetPath.Contains("/Input.cs"))
                {
                    backUpPath = assetPath.Replace("/Input.cs", "/");
                }
            }
            backUpPath = backUpPath + "/BackupInputManager.FIFI";


            //backUpPath += ".asset";
            backUpPath = Application.dataPath + backUpPath.Substring(6);
            newInputsPath = backUpPath.Replace("/BackupInputManager.FIFI", "/InputManager.FIFI");
            backUpPath = backUpPath.Replace('/', Path.DirectorySeparatorChar);
            newInputsPath = newInputsPath.Replace('/', Path.DirectorySeparatorChar);
        }

        [MenuItem("Edit/Project Settings/Input/Upgrade")]
        public static void ReplaceInputs()
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
            //        System.IO.File.Open(Application.dataPath + EditorApplication.applicationContentsPath)
        }

        [MenuItem("Edit/Project Settings/Input/Revert")]
        public static void UseOldInputs()
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
        }
    }
}