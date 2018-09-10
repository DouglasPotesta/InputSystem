using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeepScriptableObject : ScriptableObject, ISerializationCallbackReceiver {


    private static readonly Type serialAttribute = typeof(SerializeField);
    private static readonly Type nonSerialAttribute = typeof(NonSerializedAttribute);


    private FieldInfo[] allSerializableFields;

    public FieldInfo[] GetallSerializableFields()
    {
        if (allSerializableFields == null) { allSerializableFields = GetAllDeepSerializableFields(); }
        return allSerializableFields;
    }

    public FieldInfo[] GetAllDeepSerializableFields()
    {
        Type parentClass = GetType();
        List<FieldInfo> fieldInfos = new List<FieldInfo>();
        while (parentClass != typeof(DeepScriptableObject))
        {
            fieldInfos.AddRange(parentClass.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x =>x.FieldType.IsSubclassOf(typeof(DeepSerial))&& IsSerializable(x)));
            parentClass = parentClass.BaseType;
        }
        return fieldInfos.ToArray();
    }



    public static bool IsSerializable(FieldInfo fieldInfo)
    {

        object[] attributes = fieldInfo.GetCustomAttributes(true);
        if (fieldInfo.IsPublic)
        {
            return fieldInfo.FieldType.IsSerializable && !attributes.Any(x => x.GetType() == nonSerialAttribute);
        }
        else
        {
            return attributes.Any(x => x.GetType() == serialAttribute);
        }
    }


    public void OnAfterDeserialize()
    {

    }



    public void OnBeforeSerialize()
    {
        for (int i = 0; i < GetallSerializableFields().Length; i++)
        {
            GetallSerializableFields()[i].SetValue(this, DeepSerial.DeserializeToProperType(((DeepSerial)GetallSerializableFields()[i].GetValue(this))));
        }
    }
}
