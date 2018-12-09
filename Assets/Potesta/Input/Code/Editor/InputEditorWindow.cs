#if FLEXINPUT
using Potesta;
using Potesta.FlexInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using UObject = UnityEngine.Object;

public class InputEditorWindow : EditorWindow {

    public static InputEditorWindow editorWindow;

    [MenuItem("Edit/Project Settings/Input/Defaults %i")]
    public static void CreateWindow()
    {
        editorWindow = GetWindow<InputEditorWindow>();
        editorWindow.Show();
    }

    InputControllerDefault currentID;
    List<InputControllerDefault> inputDefaults = new List<InputControllerDefault>();
    Dictionary<InputControllerDefault, DefaultsEditor> idGUIS = new Dictionary<InputControllerDefault, DefaultsEditor>();

    public Vector2 scrollPosition = Vector2.zero;
    public Vector2 horizontalScrollPost = Vector2.zero;

    public void OnEnable()
    {
        bool loadFromTemp = InputBuildingUtility.IsTempUpToDate();
        inputDefaults = (inputDefaults == null || inputDefaults.Count ==0)? InputBuildingUtility.ImportDefaultInputs(loadFromTemp).ToList():inputDefaults.Where(x=>x!=null).ToList();
        //string[] assetGUIDS = AssetDatabase.FindAssets("t: InputControllerDefault");
        //for(int i = 0; i < assetGUIDS.Length; i++)
        //{
        //    InputControllerDefault item = AssetDatabase.LoadAssetAtPath<InputControllerDefault>(AssetDatabase.GUIDToAssetPath(assetGUIDS[i]));
        //    if (item != null)
        //    {
        //        inputDefaults.Add(item);
        //    }
        //}
        idGUIS = new Dictionary<InputControllerDefault, DefaultsEditor>();
        for (int i = 0; i < inputDefaults.Count; i++)
        {
            idGUIS.Add(inputDefaults[i], new DefaultsEditor (inputDefaults[i]));
            idGUIS[inputDefaults[i]].OnEnable();
        }
        currentID = inputDefaults.Count>0? inputDefaults[0]:null;
        Undo.undoRedoPerformed += OnUndo;
        UnitySave.OnSaveAssets += SaveSettings;
    }

    public void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndo;
        UnitySave.OnSaveAssets -= SaveSettings;
    }

    public void OnUndo()
    {
        InputBuildingUtility.ExportDefaultInputs(inputDefaults.ToArray(), true);
        Repaint();
    }

    public void ReInit()
    {
        OnDisable();
        OnEnable();
    }


    public void OnGUI()
    {
        if (currentID == null) { ReInit(); return; }

        EditorGUI.BeginChangeCheck();
        RuntimePlatformTabGUI();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        idGUIS[currentID].OnInspectorGUI();
        Color ogContentColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.8f, 0.4f, 0.4f, 1);
        if (GUILayout.Button("Delete"))
        {
            string assetPath = AssetDatabase.GetAssetPath(currentID);
            if (EditorUtility.DisplayDialog("Delete asset?", assetPath + "\n\n You cannot undo this action.", "Delete", "Cancel"))
            {
                inputDefaults.Remove(currentID);
                Undo.ClearUndo(currentID);
                DestroyImmediate(currentID);
            }
        }
        GUI.backgroundColor = ogContentColor;
        EditorGUILayout.EndScrollView();
        if (currentID == null || currentID.name == null) { ReInit(); }
        if (EditorGUI.EndChangeCheck())
        {
            InputBuildingUtility.ExportDefaultInputs(inputDefaults.ToArray(), true);
        }
    }



    private void SaveSettings()
    {
        InputBuildingUtility.ExportDefaultInputs(inputDefaults.ToArray(), false);
    }

    private void RuntimePlatformTabGUI()
    {
        float width = position.width;
        horizontalScrollPost = EditorGUILayout.BeginScrollView(horizontalScrollPost, GUILayout.Height(EditorGUIUtility.singleLineHeight*2.5f));
        GUILayoutOption[] buttonTabGUILO = new GUILayoutOption[] { GUILayout.Width(width / 4), GUILayout.MinWidth(120) };
        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("+", EditorStyles.miniButton))
        {
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
            InputControllerDefault inputControllerDefault = CreateInstance<InputControllerDefault>();
            Undo.RegisterCreatedObjectUndo(inputControllerDefault, "Create Platform Default");
            inputDefaults.Insert(0,inputControllerDefault);
            ReInit();
            return;
        }

        for (int i = 0; i < inputDefaults.Count; i++)
        {
            if(inputDefaults[i] == null) { ReInit(); goto End; }
            EditorGUI.BeginDisabledGroup(inputDefaults[i] == currentID);
            if (GUILayout.Button(Enum.GetName(typeof(RuntimePlatform), ((InputControllerDefault)idGUIS[inputDefaults[i]].target).runtimePlatform), buttonTabGUILO))
            {
                currentID = inputDefaults[i];
            }
            EditorGUI.EndDisabledGroup();
        }
        End:
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

}
public class UnitySave : UnityEditor.AssetModificationProcessor
{
    public static Action OnSaveAssets;

    static string[] OnWillSaveAssets(string[] paths)
    {
        if(OnSaveAssets != null) { OnSaveAssets(); }
        return paths;
    }
}
#endif