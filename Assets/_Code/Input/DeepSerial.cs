using System;
using UnityEngine;
[System.Serializable]
public class DeepSerial : ISerializationCallbackReceiver
{
    [SerializeField] private string serials;
    [SerializeField] private string derivedClassName = "";
    private Type derivedClassType { get { if (derivedClassName == "") { derivedClassName = GetType().Name; }return Type.GetType(derivedClassName); } set { if(value == null) { derivedClassName = ""; } else { derivedClassName = value.Name; } } }
    private bool isDeserializing = false;
    private bool isSerializing = false;

    private Type DerivedClassType
    {
        get
        {
            if(derivedClassType == null) { derivedClassType = GetType(); }
            return derivedClassType;
        }
    }

    public static DeepSerial DeserializeToProperType(DeepSerial deepSerial)
    {
        if(deepSerial.GetType() != deepSerial.DerivedClassType)
        {
            return (DeepSerial) JsonUtility.FromJson(deepSerial.serials, deepSerial.DerivedClassType);
        }
        else
        {
            return deepSerial;
        }
    }

    public void OnAfterDeserialize()
    {
        if (!isDeserializing)
        {
            isDeserializing = true;
            string temp = serials;
            JsonUtility.FromJsonOverwrite(serials, this);
            serials = temp;
            isDeserializing = false;
        }
    }

    public void OnBeforeSerialize()
    {
        if (!isSerializing)
        {
            isSerializing = true;
            serials = "";
            serials = JsonUtility.ToJson(this);
            //Debug.Log(serials);
            isSerializing = false;
        }
    }
}
