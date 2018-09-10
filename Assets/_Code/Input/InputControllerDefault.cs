using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
[CreateAssetMenu(menuName = "Input/Platform Defaults")]
public class InputControllerDefault : DeepScriptableObject {

    public Type InputControllerType
    {
        get { if (string.IsNullOrEmpty(inputControllerTypeName)) { return null; } else { return Type.GetType(inputControllerTypeName); } }
        set { if (value == null) { inputControllerTypeName = ""; } else { inputControllerTypeName = value.Name; } }
    }
    public string inputControllerTypeName;

    public InputController inputController;

    public RuntimePlatform runtimePlatform;


    public void GenerateDefaults()
    {

    }
}