#if FLEXINPUT
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UInput = UnityEngine.Input;
using Potesta;
using Potesta.FlexInput;

public partial class Input {

    private static string SavePath { get { return Application.persistentDataPath + "/InputConfig.dat"; } }

    public static void SaveSettings()
    {
        InputController[] data;
        if (num != null)
        {
            data = (InputController[])num.Clone();
        }
        else
        {
            data = new InputController[] {
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>()
            };
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(SavePath, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
    }
    [RunOnGameInitialized]
    public static void LoadSettings()
    {
        GameInitializer.OnUpdate += Update;
        if (File.Exists(SavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            file = File.Open(SavePath, FileMode.Open);
            try
            {
                num = (InputController[])bf.Deserialize(file);
                file.Close();
            }
            catch
            {
                Debug.LogError("Corrupted Save or Out of Date. It has been deleted sorry.");
                file.Close();
                ResetAllSettings();
            }
        }
        else
        {
            num = new InputController[] {
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>(),
                DefaultInputController.inputController.DeepClone<InputController>()
            };
        }
        SaveSettings();
        for (int i = 0; i < Num.Length; i++)
        {
            for (int ii = 0; ii < Num[i].InputMaps.Length; ii++) { Num[i].InitializeControls(); }
            Num[i].controllerStatus = CheckStatus(Num[i]);
        }
    }

#if UNITY_EDITOR
    //[UnityEditor.Callbacks.DidReloadScripts]
    //private static void UpdateScriptingSymbols()
    //{
    //    UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
    //    string scriptSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
    //    if (!scriptSymbols.Contains("FLEXINPUT"))
    //    {
    //        UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup,
    //        scriptSymbols + ", FLEXINPUT");
    //    }
    //}
#endif

    public static void ResetAllSettings()
    {
        File.Delete(SavePath);
        LoadSettings();
        UpdateJoyConfiguration(Input.GetJoystickNames());
    }
}
#endif