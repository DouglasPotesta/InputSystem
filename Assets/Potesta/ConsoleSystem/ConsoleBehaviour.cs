#if UNITY_EDITOR

using UnityEditor;
#endif
#if CONSOLE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace Potesta.Console
{
    public partial class Console : MonoBehaviour
    {
        /// <summary>
        /// Contains the map for the console commands. This gets populated in the updatemethods method.
        /// </summary>
        private static Dictionary<string, MethodInfo> methodDictionary;

        private void OnEnable()
        {
            if (singleton == null || singleton == this)
            {
                singleton = this;
                DontDestroyOnLoad(singleton.gameObject);
                UpdateMethods();
                singleton.InitGUI();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// A method that will run when the game loads to allow for the console to spawn and initialize.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (singleton == null)
            {
                singleton = new GameObject("Console").AddComponent<Console>();
                singleton.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
        }
        /// <summary>
        /// Updates the dictionary of console commands when scripts are recompiled. This will add any viable function with a consolecommand attribute.
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
#endif
        private static void UpdateMethods()
        {
            MethodInfo[] methods = Assembly.GetAssembly(typeof(Console)).GetTypes().SelectMany(t => t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)).ToArray();
            methods = methods.Where(m => m.GetCustomAttributes(typeof(ConsoleComandAttribute), false).Length > 0).ToArray();
            List<MethodInfo> correctMethods = new List<MethodInfo>();
            for (int i = 0; i < methods.Length; i++)
            {
                if (!methods[i].IsStatic)
                {
                    Debug.LogWarning(methods[i].Name + " is not static. Console command methods must be static.");
                }
                else if (!methods[i].GetParameters().All(x => TypeExtensions.IsCastableTo(typeof(string), x.ParameterType) || x.ParameterType.IsPrimitive))
                {
                    Debug.LogWarning(methods[i].Name + " has paramaters that are not castable from string. Console Commands can only accept arguments castable from string.");
                }
                else
                {
                    correctMethods.Add(methods[i]);
                }
            }
            methodDictionary = new Dictionary<string, MethodInfo>();
            for (int i = 0; i < correctMethods.Count; i++)
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
        /// <summary>
        /// Method for running user input in the console. This can be helpful in blackboxing large sections of the project.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string RunCommand(string input)
        {
            if (string.IsNullOrEmpty(input) || input == "\n") { return null; }
            string trimedInput = input.Trim();
            string[] cleanedInputs = trimedInput.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            string function = cleanedInputs[0].ToLower() + (cleanedInputs.Length - 1).ToString();
            string[] paramaters = cleanedInputs.ToList().GetRange(1, cleanedInputs.Length - 1).ToArray();
            if (!methodDictionary.ContainsKey(function))
            {
                return null;
            }
            MethodInfo method = methodDictionary[function];
            ParameterInfo[] parameterInfos = method.GetParameters();
            object[] objects = new object[parameterInfos.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i] = Convert.ChangeType(paramaters[i], parameterInfos[i].ParameterType);
            }
            object value = method.Invoke(null, objects);
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }
    }
    /// <summary>
    /// A little extension class for checking custom implicit and explicit extensions.
    /// </summary>
    internal static class TypeExtensions
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
}
#endif
#if UNITY_EDITOR
namespace Potesta.Console
{
    /// <summary>
    /// A class for enabling and disabling the console system across the project.
    /// </summary>
    public class ConsoleEnabler
    {
        /// <summary>
        /// Adds the scripting define symbol "CONSOLE" to the script define symbols for the project.
        /// </summary>
        [MenuItem("Edit/Project Settings/Console/Enable")]
        public static void EnableConsole()
        {
            UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string scriptDefineSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
            string defineSymbolsToSet = scriptDefineSymbols;
            defineSymbolsToSet = defineSymbolsToSet.Replace("CONSOLE", "");
            defineSymbolsToSet = defineSymbolsToSet + " CONSOLE";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup, defineSymbolsToSet);
        }
        /// <summary>
        /// Removes the scripting define symbol "CONSOLE" to the script define symbols for the project. This is useful for deploying to a system where console commands are not ideal.
        /// </summary>
        [MenuItem("Edit/Project Settings/Console/Disable")]
        public static void DisableConsole()
        {
            UnityEditor.BuildTargetGroup selectedBuildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string scriptDefineSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup);
            string defineSymbolsToSet = scriptDefineSymbols;
            defineSymbolsToSet = defineSymbolsToSet.Replace("CONSOLE", "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup, defineSymbolsToSet);
        }
    }
}
#endif