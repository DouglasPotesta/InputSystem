using System;
using UnityEngine;
using Potesta.Serialization;
namespace Potesta.FlexInput
{
    [CreateAssetMenu(menuName = "Input/Platform Defaults")]
    public class InputControllerDefault : DeepScriptableObject
    {

        public Type InputControllerType
        {
            get { if (string.IsNullOrEmpty(inputControllerTypeName)) { return null; } else { return Type.GetType(inputControllerTypeName); } }
            set { if (value == null) { inputControllerTypeName = ""; } else { inputControllerTypeName = value.FullName; } }
        }
        public string inputControllerTypeName;

        public InputController inputController;

        public RuntimePlatform runtimePlatform;


        public void GenerateDefaults()
        {

        }
    }
}