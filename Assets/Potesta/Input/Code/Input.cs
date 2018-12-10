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
using System.Collections.Specialized;
#if UNITY_EDITOR
using UnityEditor;
#endif
public partial class Input 
{
    private static readonly string[] axesNames = new string[] { "mouse_axis_0", "mouse_axis_1", "mouse_axis_2", "joy_0_axis_0", "joy_0_axis_1", "joy_0_axis_2", "joy_0_axis_3", "joy_0_axis_4", "joy_0_axis_5", "joy_0_axis_6", "joy_0_axis_7", "joy_0_axis_8", "joy_0_axis_9", "joy_0_axis_10", "joy_0_axis_11", "joy_0_axis_12", "joy_0_axis_13", "joy_0_axis_14", "joy_0_axis_15", "joy_0_axis_16", "joy_0_axis_17", "joy_0_axis_18", "joy_0_axis_19", "joy_0_axis_20", "joy_0_axis_21", "joy_0_axis_22", "joy_0_axis_23", "joy_0_axis_24", "joy_0_axis_25", "joy_0_axis_26", "joy_0_axis_27", "joy_1_axis_0", "joy_1_axis_1", "joy_1_axis_2", "joy_1_axis_3", "joy_1_axis_4", "joy_1_axis_5", "joy_1_axis_6", "joy_1_axis_7", "joy_1_axis_8", "joy_1_axis_9", "joy_1_axis_10", "joy_1_axis_11", "joy_1_axis_12", "joy_1_axis_13", "joy_1_axis_14", "joy_1_axis_15", "joy_1_axis_16", "joy_1_axis_17", "joy_1_axis_18", "joy_1_axis_19", "joy_1_axis_20", "joy_1_axis_21", "joy_1_axis_22", "joy_1_axis_23", "joy_1_axis_24", "joy_1_axis_25", "joy_1_axis_26", "joy_1_axis_27", "joy_2_axis_0", "joy_2_axis_1", "joy_2_axis_2", "joy_2_axis_3", "joy_2_axis_4", "joy_2_axis_5", "joy_2_axis_6", "joy_2_axis_7", "joy_2_axis_8", "joy_2_axis_9", "joy_2_axis_10", "joy_2_axis_11", "joy_2_axis_12", "joy_2_axis_13", "joy_2_axis_14", "joy_2_axis_15", "joy_2_axis_16", "joy_2_axis_17", "joy_2_axis_18", "joy_2_axis_19", "joy_2_axis_20", "joy_2_axis_21", "joy_2_axis_22", "joy_2_axis_23", "joy_2_axis_24", "joy_2_axis_25", "joy_2_axis_26", "joy_2_axis_27", "joy_3_axis_0", "joy_3_axis_1", "joy_3_axis_2", "joy_3_axis_3", "joy_3_axis_4", "joy_3_axis_5", "joy_3_axis_6", "joy_3_axis_7", "joy_3_axis_8", "joy_3_axis_9", "joy_3_axis_10", "joy_3_axis_11", "joy_3_axis_12", "joy_3_axis_13", "joy_3_axis_14", "joy_3_axis_15", "joy_3_axis_16", "joy_3_axis_17", "joy_3_axis_18", "joy_3_axis_19", "joy_3_axis_20", "joy_3_axis_21", "joy_3_axis_22", "joy_3_axis_23", "joy_3_axis_24", "joy_3_axis_25", "joy_3_axis_26", "joy_3_axis_27", "joy_4_axis_0", "joy_4_axis_1", "joy_4_axis_2", "joy_4_axis_3", "joy_4_axis_4", "joy_4_axis_5", "joy_4_axis_6", "joy_4_axis_7", "joy_4_axis_8", "joy_4_axis_9", "joy_4_axis_10", "joy_4_axis_11", "joy_4_axis_12", "joy_4_axis_13", "joy_4_axis_14", "joy_4_axis_15", "joy_4_axis_16", "joy_4_axis_17", "joy_4_axis_18", "joy_4_axis_19", "joy_4_axis_20", "joy_4_axis_21", "joy_4_axis_22", "joy_4_axis_23", "joy_4_axis_24", "joy_4_axis_25", "joy_4_axis_26", "joy_4_axis_27", "joy_5_axis_0", "joy_5_axis_1", "joy_5_axis_2", "joy_5_axis_3", "joy_5_axis_4", "joy_5_axis_5", "joy_5_axis_6", "joy_5_axis_7", "joy_5_axis_8", "joy_5_axis_9", "joy_5_axis_10", "joy_5_axis_11", "joy_5_axis_12", "joy_5_axis_13", "joy_5_axis_14", "joy_5_axis_15", "joy_5_axis_16", "joy_5_axis_17", "joy_5_axis_18", "joy_5_axis_19", "joy_5_axis_20", "joy_5_axis_21", "joy_5_axis_22", "joy_5_axis_23", "joy_5_axis_24", "joy_5_axis_25", "joy_5_axis_26", "joy_5_axis_27", "joy_6_axis_0", "joy_6_axis_1", "joy_6_axis_2", "joy_6_axis_3", "joy_6_axis_4", "joy_6_axis_5", "joy_6_axis_6", "joy_6_axis_7", "joy_6_axis_8", "joy_6_axis_9", "joy_6_axis_10", "joy_6_axis_11", "joy_6_axis_12", "joy_6_axis_13", "joy_6_axis_14", "joy_6_axis_15", "joy_6_axis_16", "joy_6_axis_17", "joy_6_axis_18", "joy_6_axis_19", "joy_6_axis_20", "joy_6_axis_21", "joy_6_axis_22", "joy_6_axis_23", "joy_6_axis_24", "joy_6_axis_25", "joy_6_axis_26", "joy_6_axis_27", "joy_7_axis_0", "joy_7_axis_1", "joy_7_axis_2", "joy_7_axis_3", "joy_7_axis_4", "joy_7_axis_5", "joy_7_axis_6", "joy_7_axis_7", "joy_7_axis_8", "joy_7_axis_9", "joy_7_axis_10", "joy_7_axis_11", "joy_7_axis_12", "joy_7_axis_13", "joy_7_axis_14", "joy_7_axis_15", "joy_7_axis_16", "joy_7_axis_17", "joy_7_axis_18", "joy_7_axis_19", "joy_7_axis_20", "joy_7_axis_21", "joy_7_axis_22", "joy_7_axis_23", "joy_7_axis_24", "joy_7_axis_25", "joy_7_axis_26", "joy_7_axis_27", "joy_8_axis_0", "joy_8_axis_1", "joy_8_axis_2", "joy_8_axis_3", "joy_8_axis_4", "joy_8_axis_5", "joy_8_axis_6", "joy_8_axis_7", "joy_8_axis_8", "joy_8_axis_9", "joy_8_axis_10", "joy_8_axis_11", "joy_8_axis_12", "joy_8_axis_13", "joy_8_axis_14", "joy_8_axis_15", "joy_8_axis_16", "joy_8_axis_17", "joy_8_axis_18", "joy_8_axis_19", "joy_8_axis_20", "joy_8_axis_21", "joy_8_axis_22", "joy_8_axis_23", "joy_8_axis_24", "joy_8_axis_25", "joy_8_axis_26", "joy_8_axis_27", "joy_9_axis_0", "joy_9_axis_1", "joy_9_axis_2", "joy_9_axis_3", "joy_9_axis_4", "joy_9_axis_5", "joy_9_axis_6", "joy_9_axis_7", "joy_9_axis_8", "joy_9_axis_9", "joy_9_axis_10", "joy_9_axis_11", "joy_9_axis_12", "joy_9_axis_13", "joy_9_axis_14", "joy_9_axis_15", "joy_9_axis_16", "joy_9_axis_17", "joy_9_axis_18", "joy_9_axis_19", "joy_9_axis_20", "joy_9_axis_21", "joy_9_axis_22", "joy_9_axis_23", "joy_9_axis_24", "joy_9_axis_25", "joy_9_axis_26", "joy_9_axis_27", "joy_10_axis_0", "joy_10_axis_1", "joy_10_axis_2", "joy_10_axis_3", "joy_10_axis_4", "joy_10_axis_5", "joy_10_axis_6", "joy_10_axis_7", "joy_10_axis_8", "joy_10_axis_9", "joy_10_axis_10", "joy_10_axis_11", "joy_10_axis_12", "joy_10_axis_13", "joy_10_axis_14", "joy_10_axis_15", "joy_10_axis_16", "joy_10_axis_17", "joy_10_axis_18", "joy_10_axis_19", "joy_10_axis_20", "joy_10_axis_21", "joy_10_axis_22", "joy_10_axis_23", "joy_10_axis_24", "joy_10_axis_25", "joy_10_axis_26", "joy_10_axis_27", "joy_-1_axis_0", "joy_-1_axis_1", "joy_-1_axis_2", "joy_-1_axis_3", "joy_-1_axis_4", "joy_-1_axis_5", "joy_-1_axis_6", "joy_-1_axis_7", "joy_-1_axis_8", "joy_-1_axis_9", "joy_-1_axis_10", "joy_-1_axis_11", "joy_-1_axis_12", "joy_-1_axis_13", "joy_-1_axis_14", "joy_-1_axis_15", "joy_-1_axis_16", "joy_-1_axis_17", "joy_-1_axis_18", "joy_-1_axis_19", "joy_-1_axis_20", "joy_-1_axis_21", "joy_-1_axis_22", "joy_-1_axis_23", "joy_-1_axis_24", "joy_-1_axis_25", "joy_-1_axis_26", "joy_-1_axis_27", "Submit", "Cancel" };

    private static readonly string[] INVALID_CONTROLLERS = new string[] { "Wireless Controller", null, "" };

    private static int joyLength = 0;

    private static InputController[] num;
    private static InputController[] Num
    {
        get
        {
            if (num == null)
            {
                LoadSettings();
            }
            return num;
        }

        set
        {
            num = value;
        }
    }

    private static InputControllerDefault defaultInputController;
    private static InputControllerDefault DefaultInputController
    {
        get
        {
            if (defaultInputController == null)
            {

                defaultInputController = GameInitializer.GetAllAssets<InputControllerDefault>().FirstOrDefault(x => x.runtimePlatform == Application.platform);
                if (defaultInputController == null)
                {
                    Debug.LogError("No input controller defaults found for this platform.");
                    return null;
                }
            }
            return defaultInputController;
        }
    }

    /// <summary>
    /// Whether input should be updated in the FixedUpdate loop.
    ///  Useful if all input is applied in a fixedUpdate loop.
    /// </summary>
    public static bool CheckInputInFixedUpdate = false;

    /// <summary>
    /// Whether the Input system is testing input for input mapping.
    /// </summary>
    public static bool IsTestingForInput { get { return InputMap.IsTestingForInput; } }

    /// <summary>
    /// The number of joysticks connected and valid.
    /// </summary>
    public static int JoystickCount { get { return UInput.GetJoystickNames().Where(x => !INVALID_CONTROLLERS.Contains(x)).Count(); } }

    /// <summary>
    /// The number of input device schems that are connected and valid. (Max is 4)
    /// </summary>
    public static int ControllerCount { get { return Mathf.Min(JoystickCount + (Num.All(x => x.Gamepad) ? 0 : 1), 4); } }

    /// <summary>
    /// Event that is called when a controller is disconnected
    /// </summary>
    public static Action<int> OnControllerDisconnected;
    /// <summary>
    /// Event that is called when a controller is connected.
    /// </summary>
    public static Action<int> OnControllerConnected;

    private static void Update()
    {
        string[] currentJoyNames = Input.GetJoystickNames();
        if (joyLength != currentJoyNames.Length)
        {
            UpdateJoyConfiguration(currentJoyNames);
        }
        if (CheckInputInFixedUpdate) 
        {
            for(int i = 0; i < 4; i++)
            {
                if (Num[i].controllerStatus == ControllerStatus.Connected)
                {
                    Num[i].UpdateFixedInput();
                }
            }
        }
    }

    private static void JoystickAdded(int controllerNum)
    {
        if (OnControllerConnected != null)
            OnControllerConnected.Invoke(controllerNum);
    }

    private static void JoystickRemoved(int controllerNum)
    {
        if (OnControllerDisconnected != null)
            OnControllerDisconnected.Invoke(controllerNum);
    }

    private static void UpdateJoyConfiguration(string[] currentJoyNames)
    {
        joyLength = currentJoyNames.Length;
        for (int i = 0; i < 4; i++)
        {
            if (CheckStatus(Num[i]) == ControllerStatus.Disconnected)
            {
                if (Num[i].controllerStatus == ControllerStatus.Connected)
                {
                    JoystickRemoved(i);
                }
                for (int x = 0; x < joyLength; x++)
                {
                    if (!INVALID_CONTROLLERS.Contains(currentJoyNames[x]))
                    {
                        bool isFree = true;
                        for (int ii = 0; ii < 4; ii++)
                        {
                            isFree &= Num[ii].RawControllerNumber != x;
                        }
                        if (isFree)
                        {
                            Num[i].SetRawControllerIndex(x);
                            Num[i].controllerStatus = ControllerStatus.Connected;
                            JoystickAdded(i);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Returns an InputController for controllers 0,1,2,3
    /// </summary>
    /// <param name="controllerNum"></param>
    /// <returns></returns>
    public static InputController GetController(int controllerNum)
    {
        return Num[controllerNum];
    }
    /// <summary>
    /// Returns an InputController for controllers One, Two, Three, or Four
    /// </summary>
    /// <param name="controllerNum"></param>
    /// <returns></returns>
    public static InputController GetController(ControllerNum controllerNum)
    {
        return Num[(int)controllerNum];
    }
    /// <summary>
    /// Returns an InputController of type T for controllers 0,1,2,3
    /// </summary>
    /// <param name="controllerNum"></param>
    /// <returns></returns>
    public static T GetController<T>(int controllerNum) where T : InputController
    {
        return (T)Num[controllerNum];
    }
    /// <summary>
    /// Returns an InputController of type T for controllers One, Two, Three, or, Four
    /// </summary>
    /// <param name="controllerNum"></param>
    /// <returns></returns>
    public static T GetController<T>(ControllerNum controllerNum) where T : InputController
    {
        return (T)Num[(int)controllerNum];
    }
    /// <summary>
    /// Checks the connection status of the specified controller.
    /// </summary>
    /// <param name="inputController"></param>
    /// <returns></returns>
    public static ControllerStatus CheckStatus(InputController inputController)
    {
        if (!inputController.Gamepad || inputController.IsSerial)
        {
            return ControllerStatus.Connected;
        }
        int i = inputController.RawControllerNumber;
        return GetJoystickNames().Length > i ? !INVALID_CONTROLLERS.Contains(GetJoystickNames()[i]) ? ControllerStatus.Connected : ControllerStatus.Disconnected : ControllerStatus.Disconnected;
    }

    /// <summary>
    /// Triggers a controller disconnect. Useful for reordering controllers.
    /// </summary>
    /// <param name="controllerNum"></param>
    public static void DisconnectController(int controllerNum)
    {
        Num[controllerNum].SetRawControllerIndex(10);
        JoystickRemoved(controllerNum);
    }
    /// <summary>
    /// Triggers a controller disconnect. Useful for reordering controllers.
    /// </summary>
    /// <param name="controllerNum"></param>
    public static void DisconnectController(InputController inputController)
    {
        DisconnectController(Num.ToList().IndexOf(inputController));
    }

    /// <summary>
    /// Detect which Axis is actively returning a value for the specified inputController.
    /// </summary>
    /// <param name="_inputController"></param>
    /// <returns></returns>
    public static string DetectAxes(InputController _inputController)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("joy_" + _inputController.RawControllerNumber.ToString() + "_axis_") };
        }
        else
        {
            restrictions = new string[] { ("joy_") };
        }
        return DetectAxes(requirements, restrictions);
    }
    /// <summary>
    /// Detect which Axis is actively returning a value for the specified inputController.
    /// </summary>
    /// <param name="_inputController"></param>
    /// <param name="axisValue"></param>
    /// <returns></returns>
    public static string DetectAxes(InputController _inputController, out float axisValue)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("joy_" + _inputController.RawControllerNumber.ToString() + "_axis_") };
        }
        else
        {
            restrictions = new string[] { ("joy_") };
        }
        return DetectAxes(out axisValue, requirements, restrictions);
    }

    public static string DetectRawAxes(InputController _inputController, out float axisValue, params string[] axesToIgnore)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("joy_" + _inputController.RawControllerNumber.ToString() + "_axis_") };
        }
        else
        {
            restrictions = new string[] { ("joy_") }.Concat(axesToIgnore).ToArray();
        }
        return DetectRawAxes(out axisValue, requirements, restrictions);
    }

    public static string DetectRawAxes(out float axisValue, string[] required, params string[] restrictions)
    {
        int length = axesNames.Length;
        KeyValuePair<string, float> highestValueAxis = new KeyValuePair<string, float>("", 0);
        int joystickCount = JoystickCount;
        axisValue = 0;

        for (int i = 0; i < length; i++)
        {
            if (axesNames[i].Contains("joy_"))
            {
                string[] splitName = axesNames[i].Split('_');
                int joyNum;
                if (int.TryParse(splitName[1], out joyNum) && joyNum > joystickCount)
                {
                    break;
                }
            }
            if (!restrictions.Any(x => axesNames[i].Contains(x)) && required.All(y => axesNames[i].Contains(y)))
            {
                axisValue = UInput.GetAxis(axesNames[i]);
                float magnitudeOfAxis = Mathf.Abs(UInput.GetAxis(axesNames[i]));
                if (magnitudeOfAxis > Mathf.Abs(highestValueAxis.Value) && magnitudeOfAxis > 0)
                {
                    highestValueAxis = new KeyValuePair<string, float>(axesNames[i], axisValue);
                }
            }
        }
        axisValue = highestValueAxis.Value;
        string finalAxisString = highestValueAxis.Key;
        for (int ii = 0; ii < required.Length; ii++) { finalAxisString = finalAxisString.Replace(required[ii], ""); }
        return finalAxisString.ToLower();
    }

    /// <summary>
    /// Detect which Axis is actively returning a value for the specified inputController.
    /// </summary>
    /// <param name="requirements"></param>
    /// <returns></returns>
    public static string DetectAxes(params string[] requirements)
    {
        return DetectAxes(new string[0], requirements);
    }
    /// <summary>
    /// Detect which Axis is actively returning a value for the specified inputController.
    /// </summary>
    /// <param name="axisValue"></param>
    /// <param name="required"></param>
    /// <param name="restrictions"></param>
    /// <returns></returns>
    public static string DetectAxes(out float axisValue, string[] required, params string[] restrictions)
    {
        int length = axesNames.Length;
        KeyValuePair<string, float> highestValueAxis = new KeyValuePair<string, float>("", 0);
        int joystickCount = JoystickCount;
        axisValue = 0;

        for (int i = 0; i < length; i++)
        {
            if (axesNames[i].Contains("joy_"))
            {
                string[] splitName = axesNames[i].Split('_');
                int joyNum;
                if (int.TryParse(splitName[1], out joyNum) && joyNum > joystickCount)
                {
                    break;
                }
            }
            if (!restrictions.Any(x => axesNames[i].Contains(x)) && required.All(y => axesNames[i].Contains(y)))
            {
                axisValue = UInput.GetAxis(axesNames[i]);
                float magnitudeOfAxis = Mathf.Abs(UInput.GetAxis(axesNames[i]));
                if (magnitudeOfAxis > Mathf.Abs(highestValueAxis.Value) && magnitudeOfAxis >= (axesNames[i].Contains("mouse") ? 10 : 0.8f))
                {
                    highestValueAxis = new KeyValuePair<string, float>(axesNames[i], axisValue);

                }
            }
        }
        axisValue = highestValueAxis.Value;
        string finalAxisString = highestValueAxis.Key;
        for (int ii = 0; ii < required.Length; ii++) { finalAxisString = finalAxisString.Replace(required[ii], ""); }
        return finalAxisString.ToLower();
    }
    /// <summary>
    /// Detect which Axis with the specifiers that is actively returning a value.
    /// </summary>
    /// <param name="required"></param>
    /// <param name="restrictions"></param>
    /// <returns></returns>
    public static string DetectAxes(string[] required, params string[] restrictions)
    {
        float axisValue;
        return DetectAxes(out axisValue, required, restrictions);
    }

    /// <summary>
    /// Detects which button is actively returning a value.
    /// </summary>
    /// <param name="_inputController"></param>
    /// <returns></returns>
    public static string DetectButton(InputController _inputController)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("Joystick" + (_inputController.RawControllerNumber + 1).ToString() + "Button") };
        }
        else
        {
            restrictions = new string[] { ("Joystick") };
        }
        return DetectButton(requirements, restrictions);
    }

    /// <summary>
    /// Detects which button is actively returning a value.
    /// </summary>
    /// <param name="requirements"></param>
    /// <returns></returns>
    public static string DetectButton(params string[] requirements)
    {
        return DetectButton(new string[0], requirements);
    }

    /// <summary>
    /// Detects which button is actively returning a value.
    /// </summary>
    /// <param name="required"></param>
    /// <param name="restrictions"></param>
    /// <returns></returns>
    public static string DetectButton(string[] required, params string[] restrictions) 
    {
        int[] keyCodeValues = (int[])Enum.GetValues(typeof(KeyCode));
        for (int i = 0; i < keyCodeValues.Length; i++)
        {
            if (UInput.GetKeyDown((KeyCode)keyCodeValues[i]))
            {
                string keyName = Enum.GetName(typeof(KeyCode), (KeyCode)keyCodeValues[i]);
                if (required.All(y=> keyName.ToLower().Contains(y.ToLower())) && !restrictions.Any(x => keyName.ToLower().Contains(x.ToLower())))
                {
                    for(int ii = 0; ii < required.Length; ii++) { keyName = keyName.Replace(required[ii], ""); }
                    return keyName.ToLower();
                }
            }
        }
        return "";
    }

    /// <summary>
    /// Creates a string that can be used with standard Unity Input system.(EG Input.GetAxis(GetJoyAxisString(2,2))
    /// </summary>
    /// <param name="JoyNum">Unity Joystick Number (-1 is all joysticks, 0 is joystick 1, etc.</param>
    /// <param name="AxisNum">Unity Axis Number(x Axis is 0, y Axis is 1, 3rd Axis is 2, 4th Axis 3 etc.)</param>
    /// <returns></returns>
    public static string GetJoyAxisString(int JoyNum, int AxisNum)
    {
        return "joy_" + JoyNum.ToString() + "_axis_" + AxisNum.ToString();
    }

    /// <summary>
    /// Creates a string that can be used with standard Unity Input system. (EG Input.GetButton(GetJoyButtonSTring(2,2))
    /// </summary>
    /// <param name="JoyNum">Unity Joystick Number (-1 is all joysticks, 0 is joystick 1, etc.</param>
    /// <param name="ButtonNum">Unity Button Number(0 is 0, 1 is 1, etc.)</param>
    /// <returns></returns>
    public static string GetJoyButtonString(int JoyNum, int ButtonNum)
    {
        //Check if the value is negative one if so we need to remove the joynumb specification to read input from all controllers.
        // KeyCode.Joystick1Button0 vs KeyCode.JoystickButton0
        string joystickNumber = JoyNum==-1?"": (JoyNum + 1).ToString();
        return "joystick " + joystickNumber + " button " + ButtonNum.ToString();
    }
}

public enum ControllerNum { One, Two, Tnree, Four}
public enum ControllerStatus { Disconnected, Connected }
#endif