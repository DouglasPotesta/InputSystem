#define CONSOLE

using Potesta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public partial class Console {

    private static Dictionary<string, MethodInfo> methodDictionary;

    [RunOnGameInitialized]
    private static void Initialize()
    {
        if (singleton == null)
        {
            singleton = new Console();
        }
        GameInitializer.OnUpdate += singleton.Update;
        UpdateMethods();
        singleton.InitGUI();
    }
#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void UpdateScriptingSymbols()
    {
        UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
        string scriptSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
        if (!scriptSymbols.Contains("CONSOLE"))
        {
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup,
            scriptSymbols + ", CONSOLE");
        }
    }
#endif
#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
#endif
    private static void UpdateMethods()
    {
        MethodInfo[] methods = Assembly.GetAssembly(typeof(GameInitializer)).GetTypes().SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)).ToArray();
        methods = methods.Where(m => m.GetCustomAttributes(typeof(ConsoleComandAttribute), false).Length > 0).ToArray();
        List<MethodInfo> correctMethods = new List<MethodInfo>();
        for (int i = 0; i < methods.Length; i++)
        {
            if (!methods[i].IsStatic)
            {
                Debug.LogWarning(methods[i].Name + " is not static. Console command methods must be static.");
            }
            else if (!methods[i].GetParameters().All(x => TypeExtensions.IsCastableTo(typeof(string), x.ParameterType)||x.ParameterType.IsPrimitive))
            {
                Debug.LogWarning(methods[i].Name + " has paramaters that are not castable from string. Console Commands can only accept arguments castable from string.");
            }
            else
            {
                correctMethods.Add(methods[i]);
            }
        }
        methodDictionary = new Dictionary<string, MethodInfo>();
        for(int i = 0; i < correctMethods.Count; i++)
        {
            string key = correctMethods[i].Name.ToLower() + correctMethods[i].GetParameters().Length.ToString();
            if (methodDictionary.ContainsKey(key))
            {
                string method1Message = correctMethods[i].DeclaringType.Name + "." + correctMethods[i].Name;
                string method2Message = methodDictionary[key].DeclaringType.Name + "." + methodDictionary[key].Name;
                Debug.LogWarning("Console Command " + method1Message + " conflicts with " + method2Message + ". Console commands must have unique names or paramater counts.");
            }
            else
            {
                methodDictionary.Add(key, correctMethods[i]);
            }
        }
    }

    private string RunUserInputCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) { return null; }
        string trimedInput = input.Trim();
        string[] cleanedInputs = trimedInput.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        string function = cleanedInputs[0].ToLower()+(cleanedInputs.Length-1).ToString();
        string[] paramaters = cleanedInputs.ToList().GetRange(1, cleanedInputs.Length - 1).ToArray();
        if (!methodDictionary.ContainsKey(function))
        {
            return null;
        }
        MethodInfo method = methodDictionary[function];
        ParameterInfo[] parameterInfos = method.GetParameters();
        object[] objects = new object[parameterInfos.Length];
        for(int i= 0; i < objects.Length; i++)
        {
            objects[i] = Convert.ChangeType(paramaters[i], parameterInfos[i].ParameterType);
        }
        object value = method.Invoke(null, objects);
        return value.ToString();
    }
}
static class TypeExtensions
{
    public static bool IsCastableTo(this Type from, Type to)
    {
        if (to.IsAssignableFrom(from))
        {
            return true;
        }
        var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                          .Where(
                              m => m.ReturnType == to &&
                                   (m.Name == "op_Implicit" ||
                                    m.Name == "op_Explicit")
                          );
        return methods.Any();
    }
}