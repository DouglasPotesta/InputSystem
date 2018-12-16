#if FLEXINPUT

using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using Potesta.Serialization;
using System.Collections.Generic;

namespace Potesta.FlexInput
{
    [System.Serializable]
    public class InputController : DeepSerial
    {
        [SerializeField]
        private bool gamepad = true;
        private int rawControllerNumber = 10;
        private bool isSerial;
        [NonSerialized]
        private ButtonMap[] buttonMaps;
        [NonSerialized]
        private AxisMap[] axisMaps;
        [NonSerialized]
        private DualAxisMap[] dualAxisMaps;
        [NonSerialized]
        private InputMap[] inputMaps;

        private FieldInfo[] InputFields { get { if (inputFields == null) { inputFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType.IsSubclassOf(typeof(InputMap))).ToArray(); } return inputFields; } }
        private FieldInfo[] inputFields;
        private FieldInfo[] ButtonFields { get { if (buttonFields == null) { buttonFields = InputFields.Where(x => x.FieldType == typeof(ButtonMap)).ToArray(); } return buttonFields; } }
        private FieldInfo[] buttonFields;
        private FieldInfo[] AxisFields { get { if (axisFields == null) { axisFields = InputFields.Where(x => x.FieldType == typeof(AxisMap)).ToArray(); } return axisFields; } }
        private FieldInfo[] axisFields;
        private FieldInfo[] DualAxisFields { get { if (dualAxisFields == null) { dualAxisFields = InputFields.Where(x => x.FieldType == typeof(DualAxisMap)).ToArray(); } return dualAxisFields; } }
        private FieldInfo[] dualAxisFields;

        private Dictionary<string, Func<float, float>> axisCalibrations = new Dictionary<string, Func<float, float>>();

        public Dictionary<string, Func<float, float>> AxisCalibrations {
            get { if (axisCalibrations == null) { axisCalibrations = new Dictionary<string, Func<float, float>>(); } return axisCalibrations; }
            private set { axisCalibrations = value; } }


        private string message;

        public string Message
        {
            get
            {
                if(string.IsNullOrEmpty(message)) { message = "Controller " + (Input.ControllerNum(this)+1).ToString(); }
                return message;
            }

            private set
            {
                message = value;
            }
        }

        /// <summary>
        /// Whether the controller is connected or not.
        /// </summary>
        public ControllerStatus controllerStatus;

        /// <summary>
        /// Whether this is a Gamepad. If false, it will assume it is a keyboard and mouse scheme.
        /// </summary>
        public bool Gamepad { get { return gamepad; } private set { gamepad = value; } }

        /// <summary>
        /// The raw joystick number unity is using to read values from for this inputController.
        /// </summary>
        public int RawControllerNumber { get { return rawControllerNumber; } }


        /// <summary>
        /// Whether this is a live instance of a controller, or a serialized instance. (Generally used for networking).
        /// </summary>
        public bool IsSerial { get { return isSerial; } private set { isSerial = value; } }

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

        

        public InputController()
        {
            var x = InputMaps;
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
        /// A setter for assigning this controller to read inputs from a specific unity joystick number. 
        /// (ie. the value passed will be used in place of the '#' in internal uses of Input such as"Input.GetButton(joystick#button6);")
        /// Only use this when an inputController is reading input from the wrong controller.
        /// Setting this to a value that is already currently being used by another inputController will result in one controller
        ///     providing input to both inputControllers.
        /// Note: Setting this to -1 will cause the inputController to read input from all controller.
        ///         This would be useful if you want everyone's input to affect something.
        /// </summary>
        /// <param name="_rawControllerIndex"></param>
        public void SetRawControllerIndex(int _rawControllerIndex)
        {
            rawControllerNumber = _rawControllerIndex;
            InitializeControls();
            Input.CheckStatus(this);
        }

        /// <summary>
        /// Disconnects a players controller.(*Note only works with Gamepads)
        /// </summary>
        public void Disconnect()
        {
            if (gamepad)
            {
                Input.DisconnectController(this);
            }
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
        private class MB : MonoBehaviour { }
        public void Calibrate()
        {
            MB mb = new GameObject().AddComponent<MB>();
            mb.gameObject.hideFlags = HideFlags.HideAndDontSave;
            mb.StartCoroutine(Calibration(mb.gameObject));
        }

        private System.Collections.IEnumerator Calibration(GameObject go)
        {
            Message = "Initializing Calibration.\n\nDo not touch the controller.";
            yield return new WaitForSecondsRealtime(3); 
            Message = "!!! CALIBRATING !!!\n\nDo not touch the controller.";
            // TODO: implement a detect axis that accepts an axis callibration dictionary as a parameter and adjusts values as needed.
            // TODO: implement a detect all axis function. this will be helpful in generating the dictionary.
            /* IMPLEMENTATION: have a function for fixing a calibration value of 0.8 or greater, -0.8 or less, and 0.3 or less 
                    ✔0.8: func should return one minus value. and dead zone it based off how far off from 1 it is on avg.
                    ✔-0.8: func should take value add one and divide by two, and deadzone value based off how far off crom -1 it is on avg.
                    ✔0.3: func should floor value if value is less than or equal to avg.
             */

            if (!Input.IsTestingForInput)
            {

                Message = "Please Wait";
                yield return new WaitForSeconds(3);
                float realStartTime = Time.realtimeSinceStartup;
                float timePassed = 0;
                AxisCalibrations.Clear();
                while (timePassed < 3)
                {
                    Message = ("Do not touch the controller.\nCalibrating..."+((int)(3.5 - timePassed)).ToString());
                    float value;
                    string axisString = Input.DetectRawAxes(this, out value, AxisCalibrations.Keys.ToArray());
                    if (!AxisCalibrations.ContainsKey(axisString)&& !string.IsNullOrEmpty(axisString))
                    {
                        if (value >= 0.8f)
                        {
                            AxisCalibrations.Add(axisString,  OneMinus);
                        }
                        else if (value <= -0.8f)
                        {
                            AxisCalibrations.Add(axisString, Add1DivedBy2);
                        }
                        else if (value <= 0.3f)
                        {
                            AxisCalibrations.Add(axisString, DeadZone);
                        }
                    }
                    timePassed = Time.realtimeSinceStartup - realStartTime;
                    yield return null;
                }
            }
                Message = "Completed Calibration.";
            yield return new WaitForSeconds(3);
            Message = "Controller " + Input.ControllerNum(this).ToString();
            yield return null;
            GameObject.Destroy(go);
        }
        private static float Add1DivedBy2(float value) { return ((value + 1) / 2); }
        private static float OneMinus(float value) { return (1 - value); }
        private static float DeadZone(float value) { return (Mathf.Floor(value * 5) / 5); }

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
#endif