#if FLEXINPUT
using System;
using System.Collections;
using UnityEngine;

namespace Potesta.FlexInput
{
    [System.Serializable]
    public class InputMap : ICloneable
    {
        [SerializeField]
        protected InputController controller;
        public static bool IsTestingForInput { get; protected set; }
        public virtual string Name { get { return ""; } }
        public string MapMessage { get { return mapMessage; } protected set { mapMessage = value; } }
        [SerializeField]
        private string mapMessage = "InputMap";
        protected const int InputCheckTime = 5;

        public InputMap(InputController _inputController)
        {
            controller = _inputController;
        }

        public virtual IEnumerator TestForInput() { return null; }
        public virtual void SerializeValues() { }
        public virtual void ReInit(InputController _inputController) { controller = _inputController; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
#endif