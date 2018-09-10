﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(InputControllerDefault))]
public class DefaultsEditor : Editor {

    private List<Type> inputControllerClasses;

    private List<Type> ICCS
    {
        get
        {
            if (inputControllerClasses == null)
            {
                Type icType = typeof(InputController);
                inputControllerClasses = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(at => at.IsSubclassOf(icType))).ToList();
            }
            return inputControllerClasses;
        }
    }


    public void OnEnable()
    {
        InputControllerDefault inputControllerDefault = (InputControllerDefault)target;
        if(inputControllerDefault.InputControllerType == null)
        {
            inputControllerDefault.InputControllerType = ICCS[0];
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    inputControllerDefault.runtimePlatform = RuntimePlatform.WindowsPlayer;
                    inputControllerDefault.inputController = (InputController)Activator.CreateInstance(ICCS[0]);
                    break;
                case RuntimePlatform.LinuxEditor:
                    inputControllerDefault.runtimePlatform = RuntimePlatform.LinuxPlayer;
                    inputControllerDefault.inputController = (InputController)Activator.CreateInstance(ICCS[0]);
                    break;
                case RuntimePlatform.OSXEditor:
                    inputControllerDefault.runtimePlatform = RuntimePlatform.OSXPlayer;
                    inputControllerDefault.inputController = (InputController)Activator.CreateInstance(ICCS[0]);
                    break;
                default:
                    inputControllerDefault.runtimePlatform = RuntimePlatform.WindowsPlayer;
                    inputControllerDefault.inputController = (InputController)Activator.CreateInstance(ICCS[0]);
                    break;
            }
            inputControllerDefault.inputController.InitializeControls();
        }
    }

    public override void OnInspectorGUI()
    {
        InputControllerDefault targ = (InputControllerDefault)target;
        EditorGUI.BeginChangeCheck();
        Type controllerSelection = ICCS[EditorGUILayout.Popup("Controller Class",ICCS.IndexOf(targ.InputControllerType), ICCS.Select(x => x.Name).ToArray())];
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targ, "Changed Controller Selection");
            targ.InputControllerType = controllerSelection;
            EditorUtility.SetDirty(targ);
        }
            PlatformDefaultsGUI(targ);

    }
    public void ClassSelectionGUI()
    {

    }
    public void PlatformDefaultList()
    {

    }
    public void PlatformDefaultsGUI(InputControllerDefault inputControllerDefault)
    {

        EditorGUI.BeginChangeCheck();
        RuntimePlatform newRuntimePlatform = (RuntimePlatform) EditorGUILayout.EnumPopup("Platform", inputControllerDefault.runtimePlatform);
        if (EditorGUI.EndChangeCheck() && inputControllerDefault.runtimePlatform != newRuntimePlatform)
        {
            Undo.RecordObject(target, "Changed Input Platform");
            inputControllerDefault.runtimePlatform = newRuntimePlatform;
            inputControllerDefault.inputController = (InputController)Activator.CreateInstance(inputControllerDefault.InputControllerType);
        }
        EditorGUILayout.LabelField("Buttons", EditorStyles.miniBoldLabel);
        for(int i = 0; i < inputControllerDefault.inputController.ButtonMaps.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(inputControllerDefault.inputController.ButtonMaps[i].Name, GUILayout.MinWidth(144));
            ButtonMapsGUI(inputControllerDefault.inputController.ButtonMaps[i], newRuntimePlatform);
            EditorGUILayout.EndHorizontal();
        }
        for (int i = 0; i < inputControllerDefault.inputController.AxisMaps.Length; i++)
        {
            EditorGUILayout.LabelField(inputControllerDefault.inputController.AxisMaps[i].Name, GUILayout.MinWidth(144));
            AxisMapsGUI(inputControllerDefault.inputController.AxisMaps[i], newRuntimePlatform);
        }
        for (int i = 0; i < inputControllerDefault.inputController.DualAxisMaps.Length; i++)
        {
            EditorGUILayout.LabelField(inputControllerDefault.inputController.DualAxisMaps[i].Name, GUILayout.MinWidth(144));
            DualAxisMapsGUI(inputControllerDefault.inputController.DualAxisMaps[i], newRuntimePlatform);
        }
    }

    private void DualAxisMapsGUI(DualAxisMap dualAxisMap, RuntimePlatform newRuntimePlatform)
    {
        int baseIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = baseIndentLevel + 1;
        EditorGUILayout.LabelField("Horizontal", GUILayout.MinWidth(144));
        AxisMapsGUI(dualAxisMap.xAxisMap, newRuntimePlatform);
        EditorGUILayout.LabelField("Vertical", GUILayout.MinWidth(144));
        AxisMapsGUI(dualAxisMap.yAxisMap, newRuntimePlatform);
        EditorGUI.indentLevel = baseIndentLevel;
    }

    private void AxisMapsGUI(AxisMap axisMap, RuntimePlatform newRuntimePlatform)
    {
        int baseIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = baseIndentLevel + 1;
        EditorGUI.BeginChangeCheck();
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth/3;
        bool changeToVirtual = EditorGUILayout.Popup("Axis Type", axisMap.IsVirtual ? 1 : 0, new string[] { "Direct", "Virtual" })==1;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, ((axisMap.IsVirtual ? "Changed to Real Axis for" : "Changed to Virtual Axis") + axisMap.Name));
            AxisMapData axisMapData = axisMap.AxisMapData;
            axisMapData.isVirtual = changeToVirtual;
            axisMapData.negativeAxisName = axisMapData.isVirtual ? "0" : "";
            axisMapData.positiveAxisName = axisMapData.isVirtual ? "3" : "0";
            typeof(AxisMap).GetField("axisMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(axisMap, axisMapData);
            EditorUtility.SetDirty(target);
        }
        if (axisMap.IsVirtual)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel = baseIndentLevel + 1;
            bool isInverted = EditorGUILayout.Toggle("Invert Axis", axisMap.IsInverted);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, ("Changed Inversion of " + axisMap.Name));
                AxisMapData axisMapData = axisMap.AxisMapData;
                axisMapData.isInverted = isInverted;
                typeof(AxisMap).GetField("axisMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(axisMap, axisMapData);
                EditorUtility.SetDirty(target);
            }
        }
        
        Dictionary<string, string> joyAxes = axisMap.IsVirtual? JoyPlatformMaps.GetButtonsForPlatform(newRuntimePlatform): JoyPlatformMaps.GetAxisForPlatform(newRuntimePlatform);
        joyAxes.Add("", "Not Active");
        string[] joyAxisArrayNames = joyAxes.Select(x => x.Key + "  |  " + x.Value).OrderBy(x => x).ToArray();
        EditorGUI.indentLevel = baseIndentLevel+ 1;
        EditorGUI.BeginChangeCheck();
        int newBut = EditorGUILayout.Popup("Negative Axis", joyAxes.Keys.OrderBy(x => x).ToList().IndexOf(axisMap.AxisMapData.negativeAxisName), (joyAxisArrayNames).ToArray());
        EditorGUI.indentLevel = baseIndentLevel + 2;
        if (axisMap.NegativeAxisName == "" || axisMap.AxisMapData.isVirtual) { GUI.enabled = false; }
        bool isNegativeAxisInverted = EditorGUILayout.Toggle("Is Inverted", axisMap.AxisMapData.isNegativeAxisInverted);
        GUI.enabled = true; 
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(target, ("Changed mapping of " + axisMap.Name));
            string val = joyAxes.Keys.ToArray().OrderBy(x => x).ToArray()[newBut];
            AxisMapData axisMapData = axisMap.AxisMapData;
            axisMapData.negativeAxisName = val;
            axisMapData.isNegativeAxisInverted = isNegativeAxisInverted && !axisMapData.isVirtual;
            typeof(AxisMap).GetField("axisMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(axisMap, axisMapData);
            EditorUtility.SetDirty(target);
        }

        joyAxes.Remove("");
        joyAxisArrayNames = joyAxes.Select(x => x.Key + "  |  " + x.Value).OrderBy(x => x).ToArray();
        
        EditorGUI.indentLevel = baseIndentLevel+1;
        EditorGUI.BeginChangeCheck();
        newBut = EditorGUILayout.Popup("Positive Axis", joyAxes.Keys.OrderBy(x => x).ToList().IndexOf(axisMap.AxisMapData.positiveAxisName), joyAxisArrayNames);
        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel = baseIndentLevel + 2;
        GUI.enabled = !axisMap.AxisMapData.isVirtual;
        bool isPostiveAxisInverted = EditorGUILayout.Toggle("Is Inverted", (axisMap.AxisMapData.negativeAxisName == "" && !axisMap.AxisMapData.isVirtual ? axisMap.AxisMapData.isInverted:axisMap.AxisMapData.isPositiveAxisInverted), GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(target, ("Changed mapping of " + axisMap.Name));
            string val = joyAxes.Keys.ToArray().OrderBy(x => x).ToArray()[newBut];
            AxisMapData axisMapData = axisMap.AxisMapData;
            axisMapData.positiveAxisName = val;
            if (axisMapData.negativeAxisName == "") { axisMapData.isInverted = isPostiveAxisInverted; axisMapData.isPositiveAxisInverted = false; }
            else { axisMapData.isPositiveAxisInverted = isPostiveAxisInverted && !axisMapData.isVirtual; }
            typeof(AxisMap).GetField("axisMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(axisMap, axisMapData);
            EditorUtility.SetDirty(target);
        }
        EditorGUI.indentLevel = baseIndentLevel;
    }

    public void ButtonMapsGUI(ButtonMap buttonMap, RuntimePlatform runtimePlatform)
    {
        Dictionary<string, string> joyButtons = JoyPlatformMaps.GetButtonsForPlatform(runtimePlatform);
        string[] joyButtonArrayNames = joyButtons.Select(x => x.Key + "  |  " + x.Value).OrderBy(x=>x).ToArray();
        EditorGUI.BeginChangeCheck();
        int newBut = EditorGUILayout.Popup(joyButtons.Keys.OrderBy(x=>x).ToList().IndexOf(buttonMap.ButtonMapData.buttonMapName), joyButtonArrayNames);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, ("Changed mapping of " + buttonMap.Name));
            string val = joyButtons.Keys.ToArray().OrderBy(x => x).ToArray()[newBut];
            ButtonMapData buttonMapData = buttonMap.ButtonMapData;
            buttonMapData.buttonMapName = val;
            typeof(ButtonMap).GetField("buttonMapData", BindingFlags.NonPublic | BindingFlags.Instance|BindingFlags.Public).SetValue(buttonMap, buttonMapData);
            EditorUtility.SetDirty(target);
        }

    }


}

public class JoyPlatformMaps
{

    public static readonly Dictionary<string, string> GenericButtons = new Dictionary<string, string>
    {
            {"0","0" },
            {"1","1"},
            {"2","2" },
            {"3","3"},
            {"4","4" },
            {"5","5" },
            {"6","6" },
            {"7","7"},
            {"8","8" },
            {"9","9"},
            {"10","10" },
            {"11","11"},
            {"12","12" },
            {"13","13" },
            {"14","14" },
            {"15","15"},
            {"16","16" },
            {"17","17"},
            {"18","18" },
            {"19","19"},
    };

    public static readonly Dictionary<string, string> XboxWindowsAxes = new Dictionary<string, string>
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
            {"6","D-Pad ↓↑)" },
            {"5","D-Pad ←→" },
            {"9","Right Trigger →" },
            {"8","Left Trigger →" },
            {"10","Right Trigger →" },
    };

    public static readonly Dictionary<string, string> XboxMacAxes = new Dictionary<string, string>
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ↓↑" },
            {"4","Right Stick ←→" },
            {"5","Left Trigger →" },
            {"6","Right Trigger →" },
    };

    public static readonly Dictionary<string, string> XboxLinuxAxes = new Dictionary<string, string>()
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"4","Right Stick ↓↑" },
            {"5","Right Stick ←→" },
            {"7","D-Pad ↓↑" },
            {"8","D-Pad ←→" },
            {"3","Left Trigger →" },
            {"6","Right Trigger →" },
    };

    public static readonly Dictionary<string, string> GenericAxes = new Dictionary<string, string>()
    {
            {"0","0" },
            {"1","1"},
            {"2","2" },
            {"3","3"},
            {"4","4" },
            {"5","5" },
            {"6","6" },
            {"7","7"},
            {"8","8" },
            {"9","9"},
            {"10","10" },
            {"11","11"},
            {"12","12" },
            {"13","13" },
            {"14","14" },
            {"15","15"},
            {"16","16" },
            {"17","17"},
            {"18","18" },
            {"19","19"},
            {"20","20" },
            {"21","21" },
            {"22","22" },
            {"23","23"},
            {"24","24" },
            {"25","25"},
            {"26","26" },
            {"27","27"},
    };

    public static readonly Dictionary<string, string> WebGLAxes = new Dictionary<string, string>()
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
    };

    public static readonly Dictionary<string, string> XboxWindowsButtons =
        new Dictionary<string, string>
        {
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"8","Left Stick Click" },
            {"9","Right Stick Click" }
        };
    public static readonly Dictionary<string, string> XboxMacButtons =
    new Dictionary<string, string>
    {
            {"16","A" },
            {"17","B" },
            {"18","X" },
            {"19","Y" },
            {"13","Left Bumper" },
            {"14","Right Bumper" },
            {"10","Back Button" },
            {"9","Start Button" },
            {"11","Left Stick Click" },
            {"12","Right Stick Click" },
            {"5","D-Pad Up" },
            {"6","D-Pad Down" },
            {"7","D-Pad Left" },
            {"8","D-Pad Right" },
            {"15","Xbox Button" }
    };
    public static readonly Dictionary<string, string> XboxLinuxButtons =
    new Dictionary<string, string>
    {
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"9","Left Stick Click" },
            {"10","Right Stick Click" },
            {"13","D-Pad Up" },
            {"14","D-Pad Down" },
            {"11","D-Pad Left" },
            {"12","D-Pad Right" }
        };

    public static readonly Dictionary<string, string> XboxWebGLButtons =
new Dictionary<string, string>
{
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Left Trigger" },
            {"7","Right Trigger" },
            {"8","Back" },
            {"9","Start" },
            {"10","Left Stick Click" },
            {"11","Right Stick Click" },
            {"12","D-Pad Up" },
            {"13","D-Pad Down" },
            {"14","D-Pad Left" },
            {"15","D-Pad Right" }
    };

    public static Dictionary<string, string> GetButtonsForPlatform(RuntimePlatform runtimePlatform)
    {
        Dictionary<string, string> buttonMapStrings;
        switch (runtimePlatform)
        {
            case RuntimePlatform.OSXPlayer:
                buttonMapStrings = JoyPlatformMaps.XboxMacButtons;
                break;
            case RuntimePlatform.WindowsPlayer:
                buttonMapStrings = JoyPlatformMaps.XboxWindowsButtons;
                break;
            case RuntimePlatform.LinuxPlayer:
                buttonMapStrings = JoyPlatformMaps.XboxLinuxButtons;
                break;
            case RuntimePlatform.WebGLPlayer:
                buttonMapStrings = JoyPlatformMaps.XboxWebGLButtons;
                break;
            case RuntimePlatform.PS4:
                // TODO add custom PS4 Support
                buttonMapStrings = JoyPlatformMaps.GenericButtons;
                break;
            case RuntimePlatform.XboxOne:
                // TODO add custom Xbox Support
                buttonMapStrings = JoyPlatformMaps.GenericButtons;
                break;
            case RuntimePlatform.Switch:
                // TODO add custom Switch Support
                buttonMapStrings = JoyPlatformMaps.GenericButtons;
                break;
            default:
                buttonMapStrings = JoyPlatformMaps.GenericButtons;
                break;
        }
        return buttonMapStrings;
    }
    public static Dictionary<string, string> GetAxisForPlatform(RuntimePlatform runtimePlatform)
    {
        Dictionary<string, string> axisMapStrings;
        switch (runtimePlatform)
        {
            case RuntimePlatform.OSXPlayer:
                axisMapStrings = JoyPlatformMaps.XboxMacAxes;
                break;
            case RuntimePlatform.WindowsPlayer:
                axisMapStrings = JoyPlatformMaps.XboxWindowsAxes;
                break;
            case RuntimePlatform.LinuxPlayer:
                axisMapStrings = JoyPlatformMaps.XboxLinuxAxes;
                break;
            case RuntimePlatform.WebGLPlayer:
                axisMapStrings = JoyPlatformMaps.GenericAxes;
                break;
            case RuntimePlatform.PS4:
                // TODO add custom PS4 Support
                axisMapStrings = JoyPlatformMaps.GenericAxes;
                break;
            case RuntimePlatform.XboxOne:
                // TODO add custom Xbox Support
                axisMapStrings = JoyPlatformMaps.GenericAxes;
                break;
            case RuntimePlatform.Switch:
                // TODO add custom Switch Support
                axisMapStrings = JoyPlatformMaps.GenericAxes;
                break;
            default:
                axisMapStrings = JoyPlatformMaps.GenericAxes;
                break;
        }
        return axisMapStrings;
    }
}