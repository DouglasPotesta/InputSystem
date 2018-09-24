using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using Potesta.Serialization;
namespace Potesta.FlexInput
{
    [System.Serializable]
    public class InputController : DeepSerial
    {
        public ControllerStatus controllerStatus;
        public bool Gamepad { get { return gamepad; } private set { gamepad = value; } }
        [SerializeField]
        private bool gamepad = true;
        public int ControllerNumber { get { return controllerNumber; } /*TODO make this set private by moving controllerdisconnect logic in this class*/set { controllerNumber = value; } }
        private int controllerNumber = 10;
        public bool IsSerial { get { return isSerial; } private set { isSerial = value; } }
        private bool isSerial;
        [NonSerialized]
        private ButtonMap[] buttonMaps;
        [NonSerialized]
        private AxisMap[] axisMaps;
        [NonSerialized]
        private DualAxisMap[] dualAxisMaps;
        [NonSerialized]
        private InputMap[] inputMaps;

        private FieldInfo[] InputFields { get { if (inputFields == null) { inputFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType.IsSubclassOf(typeof(InputMap))).OrderBy(y => y.Name).ToArray(); } return inputFields; } }
        private FieldInfo[] inputFields;
        private FieldInfo[] ButtonFields { get { if (buttonFields == null) { buttonFields = InputFields.Where(x => x.FieldType == typeof(ButtonMap)).ToArray(); } return buttonFields; } }
        private FieldInfo[] buttonFields;
        private FieldInfo[] AxisFields { get { if (axisFields == null) { axisFields = InputFields.Where(x => x.FieldType == typeof(AxisMap)).ToArray(); } return axisFields; } }
        private FieldInfo[] axisFields;
        private FieldInfo[] DualAxisFields { get { if (dualAxisFields == null) { dualAxisFields = InputFields.Where(x => x.FieldType == typeof(DualAxisMap)).ToArray(); } return dualAxisFields; } }
        private FieldInfo[] dualAxisFields;

        /// <summary>
        /// Accessor for all button maps on the controller
        /// </summary>
        public ButtonMap[] ButtonMaps
        {
            get
            {
                if (buttonMaps == null)
                {
                    buttonMaps = ButtonFields.Select(y =>
                    {
                        object val = y.GetValue(this);
                        if (val == null)
                        {
                            val = new ButtonMap(new ButtonMapData() { name = y.Name, buttonMapName = "0" }, this);

                            y.SetValue(this, val);
                        }
                        return (ButtonMap)y.GetValue(this);
                    }).ToArray();
                }
                return buttonMaps;
            }
        }

        /// <summary>
        /// Accessor for all single Axis maps on the controller
        /// </summary>
        public AxisMap[] AxisMaps
        {
            get
            {
                if (axisMaps == null)
                {
                    axisMaps = AxisFields.Select(y =>
                    {
                        object val = y.GetValue(this);
                        if (val == null)
                        {
                            val = new AxisMap(new AxisMapData() { name = y.Name, negativeAxisName = "", positiveAxisName = "0" }, this);
                            y.SetValue(this, val);
                        }
                        return (AxisMap)y.GetValue(this);
                    }).ToArray();
                }
                return axisMaps;
            }
        }

        /// <summary>
        /// Accessor for all Dual Axis maps on the controller
        /// </summary>
        public DualAxisMap[] DualAxisMaps
        {
            get
            {
                if (dualAxisMaps == null)
                {
                    dualAxisMaps = DualAxisFields.Select(y =>
                    {
                        object val = y.GetValue(this);
                        if (val == null)
                        {
                            val = new DualAxisMap(new DualAxisMapData() { name = y.Name, xAxisMapData = new AxisMapData() { negativeAxisName = "", positiveAxisName = "0", name = "X-Axis" }, yAxisMapData = new AxisMapData() { negativeAxisName = "", positiveAxisName = "1", name = "Y-Axis" } }, this);
                            y.SetValue(this, val);
                        }
                        return (DualAxisMap)y.GetValue(this);
                    }).ToArray();
                }
                return dualAxisMaps;
            }
        }

        /// <summary>
        /// Accessor for all maps on the controller
        /// </summary>
        public InputMap[] InputMaps
        {
            get
            {
                if (inputMaps == null)
                {
                    inputMaps = new InputMap[0].Concat(ButtonMaps).Concat(AxisMaps).Concat(DualAxisMaps).ToArray();
                }
                return inputMaps;
            }
        }

        /// <summary>
        /// Initializes input maps for getting their string names.
        /// </summary>
        public void InitializeControls()
        {
            for (int i = 0; i < InputMaps.Length; i++)
            {
                InputMaps[i].ReInit(this);
            }
        }

        /// <summary>
        /// Disconnects a players controller.(*Note only works with Gamepads)
        /// </summary>
        public void DisconnectController()
        {
            ControllerNumber = 10;
            InitializeControls();
        }

        /// <summary>
        /// Updates inputs for fixedUpdate checking
        /// </summary>
        public void UpdateFixedInput()
        {
            for (int i = 0; i < ButtonMaps.Length; i++)
            {
                ButtonMaps[i].Up();
            }
        }

        /// <summary>
        /// Used for freezing a controller input state when serializing and sending a controller state with an rpc.
        /// UnFreeze must be called to resume checking for input.
        /// </summary>
        public void Freeze()
        {
            for (int i = 0; i < InputMaps.Length; i++)
            {
                InputMaps[i].SerializeValues();
            }
            IsSerial = true;
        }

        /// <summary>
        /// Resumes input checking and unfreezes the input state.
        /// </summary>
        public void UnFreeze()
        {
            IsSerial = false;
        }
    }
}