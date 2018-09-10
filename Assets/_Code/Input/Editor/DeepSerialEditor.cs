using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(DeepSerial), true)]
//public class DeepSerialEditor : PropertyDrawer {
//    DeepSerial target;
//    FieldInfo[] fields;
//    public void OnEnable()
//    {

//        fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//    }
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        if(target == null)
//        {
//            target = (DeepSerial)(object) fieldInfo.GetValue(property.serializedObject.targetObject);
//            OnEnable();
//        }
//        EditorGUILayout.PropertyField(property.FindPropertyRelative("serials"), GUILayout.Height(100));
//    }
//}
