using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
[CreateAssetMenu(menuName = "Input/Platform Defaults")]
public class InputControllerDefault : ScriptableObject, ISerializationCallbackReceiver {
    

    public Type inputControllerType;



    public Dictionary<RuntimePlatform, InputController> PlatformInputs = new Dictionary<RuntimePlatform, InputController>();

    [SerializeField][HideInInspector]private List<RuntimePlatform> _keys = new List<RuntimePlatform>();
    [SerializeField][HideInInspector]private List<InputController> _values = new List<InputController>();





    public void GenerateDefaults()
    {

    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in PlatformInputs)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        PlatformInputs = new Dictionary<RuntimePlatform, InputController>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            PlatformInputs.Add(_keys[i], _values[i]);
    }
}