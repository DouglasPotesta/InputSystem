using Potesta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public partial class Console {

    static Console singleton;
    bool displayConsole = false;
    string consoleLog = "";
    Texture2D grayLine = new Texture2D(1, 1);
    Color backgroundColor = new Color(0, 0, 0, 0.75f);
    string userInput = "";
    GUILayoutOption width= GUILayout.Width(Screen.width * 0.99f);
    Vector2 scrollPos = new Vector2();
    GUIStyle style;

    private void InitGUI()
    {
        singleton.grayLine.SetPixel(0, 0, Color.gray);
    }

    private void Update()
    {
        if (Input.GetKeyDown("`"))
        {
            ToggleConsole();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            displayConsole = false;
            GameInitializer.OnGUIUpdate -= OnGUI;
        }
    }

    private void ToggleConsole()
    {
        displayConsole = !displayConsole;
        if (displayConsole)
        {
            GameInitializer.OnGUIUpdate += OnGUI;
        }
        else
        {
            GameInitializer.OnGUIUpdate -= OnGUI;
        }
    }

    private void OnGUI()
    {
        if(style == null)
        {
            style = new GUIStyle (GUI.skin.textArea);
            style.alignment = TextAnchor.LowerLeft;
        }
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = backgroundColor;
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height / 2));

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.TextArea(consoleLog, style, GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2));
        GUILayout.Box(grayLine, width, GUILayout.Height(1));
        Rect rect = GUILayoutUtility.GetRect(1, 1);
        GUI.SetNextControlName("User Input");
        userInput = GUILayout.TextArea(userInput, width);
        GUI.backgroundColor = originalColor;
        GUI.FocusControl("User Input");
        GUILayout.EndArea();
        ContextGUI();

        if (userInput.Contains("\n"))
        {
            string message = "\n"+RunUserInputCommand(userInput);
            ClearUserInput();
            if(message != null)
            {
                consoleLog += message;
            }
            Debug.Log(message);
            scrollPos = Vector2.up * Screen.height;
        }
        if (userInput.Contains("`"))
        {
            userInput = "";
            ToggleConsole();
        }
    }

    private void ContextGUI()
    {
        if (userInput.Length > 0)
        {
            Rect contextRect = new Rect(Vector2.up * Screen.height / 2 - Vector2.up * 300, Vector2.one * 300);
            GUI.TextArea(contextRect, new string(methodDictionary.Keys.Where(x => x.StartsWith(userInput)).Select(x => x.Substring(0, x.Length - 1) + "\n").Distinct().SelectMany(x => x).ToArray()), style);
        }
    }

    private void ClearUserInput()
    {
        userInput = userInput.TrimEnd('\r', '\n');
        consoleLog += "\n\n" + userInput;
        userInput = "";
    }



}
