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
#if UNITY_EDITOR
using UnityEditor;
#endif
public partial class Input
{
#if UNITY_EDITOR_WIN || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WINRT || UNITY_WINRT_10_0
    float AxisInputExceptions(string axisName)
    {
        throw new NotImplementedException();
    }
    bool ButtonInputExceptions(string buttonName)
    {
        throw new NotImplementedException();
    }
#endif
}
#endif