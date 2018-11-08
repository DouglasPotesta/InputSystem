using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace Potesta.FlexInput
{
    [CustomEditor(typeof(InputControllerDefault))]
    public class __DEFAULTSEDITOR__ : Editor
    {
        DefaultsEditor editor;
        public void OnEnable()
        {
            editor = new DefaultsEditor((InputControllerDefault)target);
            editor.OnEnable();
        }
        public override void OnInspectorGUI()
        {
            editor.OnInspectorGUI();
        }
    }
    public class DefaultsEditor
    {

        private List<Type> inputControllerClasses;
        public InputControllerDefault target;

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

        public DefaultsEditor(InputControllerDefault inputControllerDefault)
        {
            target = inputControllerDefault;
        }

        public void OnEnable()
        {
            InputControllerDefault inputControllerDefault = (InputControllerDefault)target;
            if (inputControllerDefault.InputControllerType == null)
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

        public void OnInspectorGUI()
        {
            InputControllerDefault targ = (InputControllerDefault)target;
            EditorGUI.BeginChangeCheck();
            Type controllerSelection = ICCS[EditorGUILayout.Popup("Controller Class", ICCS.IndexOf(targ.InputControllerType), ICCS.Select(x => x.FullName).ToArray())];
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(targ, "Changed Controller Selection");
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
            RuntimePlatform newRuntimePlatform = (RuntimePlatform)EditorGUILayout.EnumPopup("Platform", inputControllerDefault.runtimePlatform);
            if (EditorGUI.EndChangeCheck() && inputControllerDefault.runtimePlatform != newRuntimePlatform)
            {
                Undo.RegisterCompleteObjectUndo(target, "Changed Input Platform");
                inputControllerDefault.runtimePlatform = newRuntimePlatform;
                inputControllerDefault.inputController = (InputController)Activator.CreateInstance(inputControllerDefault.InputControllerType);
            }
            EditorGUILayout.LabelField("Buttons", EditorStyles.miniBoldLabel);
            for (int i = 0; i < inputControllerDefault.inputController.ButtonMaps.Length; i++)
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
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 3;
            bool changeToVirtual = EditorGUILayout.Popup("Axis Type", axisMap.IsVirtual ? 1 : 0, new string[] { "Direct", "Virtual" }) == 1;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, ((axisMap.IsVirtual ? "Changed to Real Axis for" : "Changed to Virtual Axis") + axisMap.Name));
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
                    Undo.RegisterCompleteObjectUndo(target, ("Changed Inversion of " + axisMap.Name));
                    AxisMapData axisMapData = axisMap.AxisMapData;
                    axisMapData.isInverted = isInverted;
                    typeof(AxisMap).GetField("axisMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(axisMap, axisMapData);
                    EditorUtility.SetDirty(target);
                }
            }

            Dictionary<string, string> joyAxes = axisMap.IsVirtual ? JoyPlatformMaps.GetButtonsForPlatform(newRuntimePlatform) : JoyPlatformMaps.GetAxisForPlatform(newRuntimePlatform);
            joyAxes.Add("", "Not Active");
            string[] joyAxisArrayNames = joyAxes.Select(x => x.Key + "  |  " + x.Value).OrderBy(x => x).ToArray();
            EditorGUI.indentLevel = baseIndentLevel + 1;
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

            EditorGUI.indentLevel = baseIndentLevel + 1;
            EditorGUI.BeginChangeCheck();
            newBut = EditorGUILayout.Popup("Positive Axis", joyAxes.Keys.OrderBy(x => x).ToList().IndexOf(axisMap.AxisMapData.positiveAxisName), joyAxisArrayNames);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel = baseIndentLevel + 2;
            GUI.enabled = !axisMap.AxisMapData.isVirtual;
            bool isPostiveAxisInverted = EditorGUILayout.Toggle("Is Inverted", (axisMap.AxisMapData.negativeAxisName == "" && !axisMap.AxisMapData.isVirtual ? axisMap.AxisMapData.isInverted : axisMap.AxisMapData.isPositiveAxisInverted), GUILayout.ExpandWidth(true));
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
            string[] joyButtonArrayNames = joyButtons.Select(x => x.Key + "  |  " + x.Value).OrderBy(x => x).ToArray();
            EditorGUI.BeginChangeCheck();
            int newBut = EditorGUILayout.Popup(joyButtons.Keys.OrderBy(x => x).ToList().IndexOf(buttonMap.ButtonMapData.buttonMapName), joyButtonArrayNames);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(target, ("Changed mapping of " + buttonMap.Name));
                string val = joyButtons.Keys.ToArray().OrderBy(x => x).ToArray()[newBut];
                ButtonMapData buttonMapData = buttonMap.ButtonMapData;
                buttonMapData.buttonMapName = val;
                typeof(ButtonMap).GetField("buttonMapData", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(buttonMap, buttonMapData);
                EditorUtility.SetDirty(target);
            }
        }
    }
}