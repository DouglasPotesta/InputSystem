using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace Potesta.Serialization
{
    [System.Serializable]
    public class DeepSerial : ISerializationCallbackReceiver
    {
        [SerializeField] private string serials;
        [SerializeField] private string derivedClassName = "";
        private Type derivedClassType { get { if (derivedClassName == "") { derivedClassName = GetType().FullName; } return Type.GetType(derivedClassName); } set { if (value == null) { derivedClassName = ""; } else { derivedClassName = value.FullName; } } }
        private bool isDeserializing = false;
        private bool isSerializing = false;

        private Type DerivedClassType
        {
            get
            {
                if (derivedClassType == null) { derivedClassType = GetType(); }
                return derivedClassType;
            }
        }

        public static DeepSerial DeserializeToProperType(DeepSerial deepSerial)
        {
            if(deepSerial == null) { return null; }
            if (deepSerial.GetType() != deepSerial.DerivedClassType)
            {
                return (DeepSerial)JsonUtility.FromJson(deepSerial.serials, deepSerial.DerivedClassType);
            }
            else
            {
                return deepSerial;
            }
        }

        public DeepSerial Deserialize()
        {
            return (DeepSerial)JsonUtility.FromJson(serials, DerivedClassType);
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
        public T DeepClone<T>()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}