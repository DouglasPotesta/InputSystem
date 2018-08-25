using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UInput = UnityEngine.Input;

public partial class Input : MonoBehaviour
{
    static readonly string[] axesNames = new string[] { "mouse_axis_0", "mouse_axis_1", "mouse_axis_2", "joy_0_axis_0", "joy_0_axis_1", "joy_0_axis_2", "joy_0_axis_3", "joy_0_axis_4", "joy_0_axis_5", "joy_0_axis_6", "joy_0_axis_7", "joy_0_axis_8", "joy_0_axis_9", "joy_0_axis_10", "joy_0_axis_11", "joy_0_axis_12", "joy_0_axis_13", "joy_0_axis_14", "joy_0_axis_15", "joy_0_axis_16", "joy_0_axis_17", "joy_0_axis_18", "joy_0_axis_19", "joy_0_axis_20", "joy_0_axis_21", "joy_0_axis_22", "joy_0_axis_23", "joy_0_axis_24", "joy_0_axis_25", "joy_0_axis_26", "joy_0_axis_27", "joy_1_axis_0", "joy_1_axis_1", "joy_1_axis_2", "joy_1_axis_3", "joy_1_axis_4", "joy_1_axis_5", "joy_1_axis_6", "joy_1_axis_7", "joy_1_axis_8", "joy_1_axis_9", "joy_1_axis_10", "joy_1_axis_11", "joy_1_axis_12", "joy_1_axis_13", "joy_1_axis_14", "joy_1_axis_15", "joy_1_axis_16", "joy_1_axis_17", "joy_1_axis_18", "joy_1_axis_19", "joy_1_axis_20", "joy_1_axis_21", "joy_1_axis_22", "joy_1_axis_23", "joy_1_axis_24", "joy_1_axis_25", "joy_1_axis_26", "joy_1_axis_27", "joy_2_axis_0", "joy_2_axis_1", "joy_2_axis_2", "joy_2_axis_3", "joy_2_axis_4", "joy_2_axis_5", "joy_2_axis_6", "joy_2_axis_7", "joy_2_axis_8", "joy_2_axis_9", "joy_2_axis_10", "joy_2_axis_11", "joy_2_axis_12", "joy_2_axis_13", "joy_2_axis_14", "joy_2_axis_15", "joy_2_axis_16", "joy_2_axis_17", "joy_2_axis_18", "joy_2_axis_19", "joy_2_axis_20", "joy_2_axis_21", "joy_2_axis_22", "joy_2_axis_23", "joy_2_axis_24", "joy_2_axis_25", "joy_2_axis_26", "joy_2_axis_27", "joy_3_axis_0", "joy_3_axis_1", "joy_3_axis_2", "joy_3_axis_3", "joy_3_axis_4", "joy_3_axis_5", "joy_3_axis_6", "joy_3_axis_7", "joy_3_axis_8", "joy_3_axis_9", "joy_3_axis_10", "joy_3_axis_11", "joy_3_axis_12", "joy_3_axis_13", "joy_3_axis_14", "joy_3_axis_15", "joy_3_axis_16", "joy_3_axis_17", "joy_3_axis_18", "joy_3_axis_19", "joy_3_axis_20", "joy_3_axis_21", "joy_3_axis_22", "joy_3_axis_23", "joy_3_axis_24", "joy_3_axis_25", "joy_3_axis_26", "joy_3_axis_27", "joy_4_axis_0", "joy_4_axis_1", "joy_4_axis_2", "joy_4_axis_3", "joy_4_axis_4", "joy_4_axis_5", "joy_4_axis_6", "joy_4_axis_7", "joy_4_axis_8", "joy_4_axis_9", "joy_4_axis_10", "joy_4_axis_11", "joy_4_axis_12", "joy_4_axis_13", "joy_4_axis_14", "joy_4_axis_15", "joy_4_axis_16", "joy_4_axis_17", "joy_4_axis_18", "joy_4_axis_19", "joy_4_axis_20", "joy_4_axis_21", "joy_4_axis_22", "joy_4_axis_23", "joy_4_axis_24", "joy_4_axis_25", "joy_4_axis_26", "joy_4_axis_27", "joy_5_axis_0", "joy_5_axis_1", "joy_5_axis_2", "joy_5_axis_3", "joy_5_axis_4", "joy_5_axis_5", "joy_5_axis_6", "joy_5_axis_7", "joy_5_axis_8", "joy_5_axis_9", "joy_5_axis_10", "joy_5_axis_11", "joy_5_axis_12", "joy_5_axis_13", "joy_5_axis_14", "joy_5_axis_15", "joy_5_axis_16", "joy_5_axis_17", "joy_5_axis_18", "joy_5_axis_19", "joy_5_axis_20", "joy_5_axis_21", "joy_5_axis_22", "joy_5_axis_23", "joy_5_axis_24", "joy_5_axis_25", "joy_5_axis_26", "joy_5_axis_27", "joy_6_axis_0", "joy_6_axis_1", "joy_6_axis_2", "joy_6_axis_3", "joy_6_axis_4", "joy_6_axis_5", "joy_6_axis_6", "joy_6_axis_7", "joy_6_axis_8", "joy_6_axis_9", "joy_6_axis_10", "joy_6_axis_11", "joy_6_axis_12", "joy_6_axis_13", "joy_6_axis_14", "joy_6_axis_15", "joy_6_axis_16", "joy_6_axis_17", "joy_6_axis_18", "joy_6_axis_19", "joy_6_axis_20", "joy_6_axis_21", "joy_6_axis_22", "joy_6_axis_23", "joy_6_axis_24", "joy_6_axis_25", "joy_6_axis_26", "joy_6_axis_27", "joy_7_axis_0", "joy_7_axis_1", "joy_7_axis_2", "joy_7_axis_3", "joy_7_axis_4", "joy_7_axis_5", "joy_7_axis_6", "joy_7_axis_7", "joy_7_axis_8", "joy_7_axis_9", "joy_7_axis_10", "joy_7_axis_11", "joy_7_axis_12", "joy_7_axis_13", "joy_7_axis_14", "joy_7_axis_15", "joy_7_axis_16", "joy_7_axis_17", "joy_7_axis_18", "joy_7_axis_19", "joy_7_axis_20", "joy_7_axis_21", "joy_7_axis_22", "joy_7_axis_23", "joy_7_axis_24", "joy_7_axis_25", "joy_7_axis_26", "joy_7_axis_27", "joy_8_axis_0", "joy_8_axis_1", "joy_8_axis_2", "joy_8_axis_3", "joy_8_axis_4", "joy_8_axis_5", "joy_8_axis_6", "joy_8_axis_7", "joy_8_axis_8", "joy_8_axis_9", "joy_8_axis_10", "joy_8_axis_11", "joy_8_axis_12", "joy_8_axis_13", "joy_8_axis_14", "joy_8_axis_15", "joy_8_axis_16", "joy_8_axis_17", "joy_8_axis_18", "joy_8_axis_19", "joy_8_axis_20", "joy_8_axis_21", "joy_8_axis_22", "joy_8_axis_23", "joy_8_axis_24", "joy_8_axis_25", "joy_8_axis_26", "joy_8_axis_27", "joy_9_axis_0", "joy_9_axis_1", "joy_9_axis_2", "joy_9_axis_3", "joy_9_axis_4", "joy_9_axis_5", "joy_9_axis_6", "joy_9_axis_7", "joy_9_axis_8", "joy_9_axis_9", "joy_9_axis_10", "joy_9_axis_11", "joy_9_axis_12", "joy_9_axis_13", "joy_9_axis_14", "joy_9_axis_15", "joy_9_axis_16", "joy_9_axis_17", "joy_9_axis_18", "joy_9_axis_19", "joy_9_axis_20", "joy_9_axis_21", "joy_9_axis_22", "joy_9_axis_23", "joy_9_axis_24", "joy_9_axis_25", "joy_9_axis_26", "joy_9_axis_27", "joy_10_axis_0", "joy_10_axis_1", "joy_10_axis_2", "joy_10_axis_3", "joy_10_axis_4", "joy_10_axis_5", "joy_10_axis_6", "joy_10_axis_7", "joy_10_axis_8", "joy_10_axis_9", "joy_10_axis_10", "joy_10_axis_11", "joy_10_axis_12", "joy_10_axis_13", "joy_10_axis_14", "joy_10_axis_15", "joy_10_axis_16", "joy_10_axis_17", "joy_10_axis_18", "joy_10_axis_19", "joy_10_axis_20", "joy_10_axis_21", "joy_10_axis_22", "joy_10_axis_23", "joy_10_axis_24", "joy_10_axis_25", "joy_10_axis_26", "joy_10_axis_27" };
    public static int JoystickCount { get { return UInput.GetJoystickNames().Where(x => (x != null && x != "")).Count(); } }

    public static int ControllerCount { get { return Mathf.Min(JoystickCount + (Num.All(x=>x.Gamepad) ? 0 : 1), 4); } }

    public static bool IsTestingForInput { get { return InputMap.IsTestingForInput; } }

    private static InputController[] num;

    public static InputController[] Num
    {
        get
        {
            if (num == null)
            {
                LoadSettings();
            }
            return num;
        }

        private set
        {
            num = value;
        }
    }

    public static Action<int> OnControllerDisconnected;
    public static Action<int> OnControllerConnected;

    public static bool CheckInputInFixedUpdate = false;

    private const string INVALID_CONTROLLER = "Wireless Controller";

    private static Input singleton;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(Num.Length < 4) { LoadSettings(); }
        for(int i = 0; i < Num.Length; i++)
        {
            Num[i].controllerStatus = CheckStatus(Num[i]);
        }
    }

    private static ControllerStatus CheckIfControllerIsConnected(int i)
    {
        return GetJoystickNames().Length > i ? !string.IsNullOrEmpty(GetJoystickNames()[i]) && (GetJoystickNames()[i] !=INVALID_CONTROLLER) ? ControllerStatus.Connected : ControllerStatus.Disconnected : ControllerStatus.Disconnected;
    }

    private static ControllerStatus CheckStatus(InputController inputController)
    {
        if (!inputController.Gamepad)
        {
            return ControllerStatus.Connected;
        }
        return CheckIfControllerIsConnected(inputController.controllerNumber);
    }

    int joyLength = 0;

    private void Update()
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

    private void UpdateJoyConfiguration(string[] currentJoyNames)
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
                    if (!string.IsNullOrEmpty(currentJoyNames[x]) && currentJoyNames[x] != INVALID_CONTROLLER)
                    {
                        bool isFree = true;
                        for (int ii = 0; ii < 4; ii++)
                        {
                            isFree &= Num[ii].controllerNumber != x;
                        }
                        if (isFree)
                        {
                            Num[i].controllerNumber = x;
                            Num[i].InitializeControls();
                            Num[i].controllerStatus = ControllerStatus.Connected;
                            JoystickAdded(i);
                        }
                    }
                }
            }
        }
    }

    private static void JoystickAdded(int controllerNum)
    {
        if(OnControllerConnected != null)
        OnControllerConnected.Invoke(controllerNum);
    }

    private static void JoystickRemoved(int controllerNum)
    {
        if(OnControllerDisconnected != null)
        OnControllerDisconnected.Invoke(controllerNum);
    }

    public static string DetectAxes(InputController _inputController)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("joy_" + _inputController.controllerNumber.ToString() + "_axis_") };
        }
        else
        {
            restrictions = new string[] { ("joy_") };
        }
        return DetectAxes(requirements, restrictions);
    }

    public static string DetectAxes(InputController _inputController, out float axisValue)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("joy_" + _inputController.controllerNumber.ToString() + "_axis_") };
        }
        else
        {
            restrictions = new string[] { ("joy_") };
        }
        return DetectAxes(out axisValue, requirements, restrictions);
    }

    public static string DetectAxes(params string[] requirements)
    {
        return DetectAxes(new string[0], requirements);
    }

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

    public static string DetectAxes(string[] required, params string[] restrictions)
    {
        float axisValue;
        return DetectAxes(out axisValue, required, restrictions);
    }

    public static string DetectButton(InputController _inputController)
    {
        string[] restrictions = new string[0];
        string[] requirements = new string[0];
        if (_inputController.Gamepad)
        {
            requirements = new string[] { ("Joystick" + (_inputController.controllerNumber + 1).ToString() + "Button") };
        }
        else
        {
            restrictions = new string[] { ("Joystick") };
        }
        return DetectButton(requirements, restrictions);
    }

    public static string DetectButton(params string[] requirements)
    {
        return DetectButton(new string[0], requirements);
    }

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

    public static string GetJoyAxisString(int JoyNum, int AxisNum)
    {
        return "joy_" + JoyNum.ToString() + "_axis_" + AxisNum.ToString();
    }

    public static string GetJoyButtonString(int JoyNum, int ButtonNum)
    {
        return "joystick " + (JoyNum + 1).ToString() + " button " + ButtonNum.ToString();
    }


    //************Save & Load********************//


    private static string SavePath { get { return Application.persistentDataPath + "/InputConfig.dat"; } }

    public static void SaveSettings()
    {
        InputController[] data;
        if (num != null)
        {
            data = (InputController[])num.Clone();
        }
        else
        {
            data = InputController.DefaultInputControllers();
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(SavePath, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadSettings()
    {
        if(singleton == null) { singleton = new GameObject("Input").AddComponent<Input>(); singleton.gameObject.hideFlags = HideFlags.HideAndDontSave; }
        if (File.Exists(SavePath))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            file = File.Open(SavePath, FileMode.Open);
            try
            {
                num = (InputController[])bf.Deserialize(file);
                file.Close();
                //Debug.Log("Loaded Save at: " + SavePath);
            }
            catch
            {
                Debug.LogError("Corrupted Save or Out of Date. It has been deleted sorry.");
                file.Close();
                ResetAllSettings();
            }
        }
        else
        {
            num = InputController.DefaultInputControllers();
            //Debug.Log("LoadedDefaults");
        }
        SaveSettings();
        //Debug.LogError(new String(Input.GetJoystickNames().SelectMany(x => "JoyStickName: " + x + "\n").ToArray()));
    }

    public static void ResetAllSettings()
    {
        File.Delete(SavePath);
        LoadSettings();
    }
}

[System.Serializable]
public struct InputConfig
{
    public bool Gamepad;
    public int controllerNumber;
    public ButtonMapData Start;
    public ButtonMapData AltStart;
    public ButtonMapData Confirm;
    public ButtonMapData Back;
    public ButtonMapData Jump;
    public ButtonMapData Reload;
    public ButtonMapData SwapPrimary;
    public ButtonMapData SwapSecondary;
    public ButtonMapData Crouch;
    public ButtonMapData Zoom;
    public AxisMapData UsePrimary;
    public AxisMapData UseSecondary;
    public DualAxisMapData Movement;
    public DualAxisMapData Look;

    public static InputConfig Default()
    {
        return new InputConfig()
        {
            Gamepad = true,
            controllerNumber = 1,
            Start = new ButtonMapData() { name = "Start", buttonMapName = "7" },
            AltStart = new ButtonMapData() { name = "Alt Start", buttonMapName = "6" },
            Confirm = new ButtonMapData() { name = "Confirm", buttonMapName = "0" },
            Back = new ButtonMapData() { name = "Back", buttonMapName = "1" },
            Jump = new ButtonMapData() { name = "Jump", buttonMapName = "0" },
            Reload = new ButtonMapData() { name = "Reload/Interact", buttonMapName = "2" },
            SwapPrimary = new ButtonMapData() { name = "Swap Primary", buttonMapName = "3" },
            SwapSecondary = new ButtonMapData() { name = "Swap Secondary", buttonMapName = "1" },
            Crouch = new ButtonMapData() { name = "Crouch", buttonMapName = "8" },
            Zoom = new ButtonMapData() { name = "Zoom", buttonMapName = "9" },
            UsePrimary = new AxisMapData() { name = "Use Primary", positiveAxisName = "9", is0To1Axis = true},
            UseSecondary = new AxisMapData() { name = "Use Secondary", positiveAxisName = "8", is0To1Axis = true},
            Movement = new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "1"} },
            Look = new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "3" }, yAxisMapData = new AxisMapData() { positiveAxisName = "4" } }
        };
    }

    public static InputConfig DefaultPC()
    {
        return new InputConfig()
        {
            Gamepad = false,
            controllerNumber = 0,
            Start = new ButtonMapData() { name = "Start", buttonMapName = "escape" },
            AltStart = new ButtonMapData() { name = "Alt Start", buttonMapName = "tab" },
            Confirm = new ButtonMapData() { name = "Confirm", buttonMapName = "enter" },
            Back = new ButtonMapData() { name = "Back", buttonMapName = "backspace" },
            Jump = new ButtonMapData() { name = "Jump", buttonMapName = "space" },
            Reload = new ButtonMapData() { name = "Reload/Interact", buttonMapName = "r" },
            SwapPrimary = new ButtonMapData() { name = "Swap Primary", buttonMapName = "q" },
            SwapSecondary = new ButtonMapData() { name = "Swap Secondary", buttonMapName = "e" },
            Crouch = new ButtonMapData() { name = "Crouch", buttonMapName = "left ctrl" },
            Zoom = new ButtonMapData() { name = "Zoom", buttonMapName = "mouse 2" },
            UsePrimary = new AxisMapData() { name = "Use Primary", positiveAxisName = "mouse 0", isVirtual = true, is0To1Axis = true},
            UseSecondary = new AxisMapData() { name = "Use Secondary", positiveAxisName = "mouse 1", isVirtual = true, is0To1Axis = true},
            Movement = new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "d", negativeAxisName = "a", isVirtual =true }, yAxisMapData = new AxisMapData() { positiveAxisName = "w", negativeAxisName = "s", isVirtual = true } },
            Look = new DualAxisMapData() { name = "Look", xAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_1" } }
        };
    }

    public static InputConfig[] DefaultInputConfigs()
    {
#if UNITY_STANDALONE
        InputConfig[] configs = (new InputConfig[] { InputConfig.DefaultPC() }).Concat(Enumerable.Repeat(InputConfig.Default(), 3)).ToArray();
        for (int i = 0; i < configs.Length; i++)
        {
            configs[i].controllerNumber = Mathf.Max(i-1,0);
        }
        return configs;
#else
        InputConfig[] configs = Enumerable.Repeat(InputConfig.Default(), 4).ToArray();
        for(int i = 0; i < configs.Length; i++)
        {
            configs[i].controllerNumber = i;
        }
        return configs;
#endif
    }
}

[System.Serializable]
public struct ButtonMapData
{
    public string name;
    public string buttonMapName;
}

[System.Serializable]
public struct AxisMapData
{
    public string name;
    public bool isInverted;
    public bool isVirtual;
    public bool is0To1Axis;
    public string positiveAxisName;
    public bool isPositiveAxisInverted;
    public string negativeAxisName;
    public bool isNegativeAxisInverted;
    public int sensitivity;
}

[System.Serializable]
public struct DualAxisMapData
{
    public string name;
    public AxisMapData xAxisMapData;
    public AxisMapData yAxisMapData;
}
public enum ControllerStatus { Disconnected, Connected }

[System.Serializable]
public class InputController
{
    public ControllerStatus controllerStatus;
    public bool Gamepad = true;
    public int controllerNumber = 0;
    [NonSerialized]
    private ButtonMap[] buttonMaps;
    [NonSerialized]
    private AxisMap[] axisMaps;
    [NonSerialized]
    private DualAxisMap[] dualAxisMaps;
    [NonSerialized]
    private InputMap[] inputMaps;
    public ButtonMap Start;
    public ButtonMap AltStart;
    public ButtonMap Confirm;
    public ButtonMap Back;
    public ButtonMap Jump;
    public ButtonMap Reload;
    public ButtonMap SwapPrimary;
    public ButtonMap SwapSecondary;
    public ButtonMap Crouch;
    public ButtonMap Zoom;
    public AxisMap UsePrimary;
    public AxisMap UseSecondary;
    public DualAxisMap Movement;
    public DualAxisMap Look;

    public ButtonMap[] ButtonMaps
    {
        get
        {
            if(buttonMaps == null)
            {
                buttonMaps = new ButtonMap[] { Start, AltStart, Confirm, Back, Jump, Reload, SwapPrimary, SwapSecondary, Crouch, Zoom };
            }
            return buttonMaps;
        }
    }

    public AxisMap[] AxisMaps
    {
        get
        {
            if(axisMaps == null)
            {
                axisMaps = new AxisMap[] { UsePrimary, UseSecondary };
            }
            return axisMaps;
        }

        set
        {
            axisMaps = value;
        }
    }

    public DualAxisMap[] DualAxisMaps
    {
        get
        {
            if(dualAxisMaps == null)
            {
                dualAxisMaps = new DualAxisMap[] { Look, Movement };
            }
            return dualAxisMaps;
        }

        set
        {
            dualAxisMaps = value;
        }
    }

    public InputMap[] InputMaps
    {
        get
        {
            if (inputMaps == null)
            {
                inputMaps = new InputMap[] { Start, AltStart, Confirm, Back, Jump, Reload, SwapPrimary, SwapSecondary, Crouch, Zoom, UsePrimary, UseSecondary, Look, Movement };
            }
            return inputMaps;
        }

        set
        {
            inputMaps = value;
        }
    }

    public InputController() { }

    public void InitializeControls()
    {
        Start .ReInit();
        AltStart .ReInit();
        Confirm .ReInit();
        Back .ReInit();
        Jump .ReInit();
        Reload .ReInit();
        SwapPrimary .ReInit();
        SwapSecondary .ReInit();
        Crouch .ReInit();
        Zoom .ReInit();
        UsePrimary .ReInit();
        UseSecondary .ReInit();
        Movement .ReInit();
        Look .ReInit();
    }

    public void SwitchToGamePad()
    {
        if (Input.Num[0] == this)
        {
            RevertToDefaultControls();
            InitializeControls();
            if (Input.JoystickCount < 4)
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (Input.Num[i].controllerStatus == ControllerStatus.Connected)
                    {
                        InitializeControls();
                        controllerStatus = ControllerStatus.Connected;
                        Input.Num[i].DisconnectController();
                        break;
                    }
                }
            }
        }
    }

    public void SwitchToPC()
    {
        if (Input.Num[0] == this)
        {
            RevertToDefaultPCControls();
            InitializeControls();
            controllerStatus = ControllerStatus.Connected;
        }
    }

    public void DisconnectController()
    {
        controllerNumber = 10;
        InitializeControls();
    }

    public void UpdateFixedInput()
    {
        for (int i = 0; i < ButtonMaps.Length; i++)
        {
            ButtonMaps[i].Up();
        }
    }

    public int[] SerializeToInts()
    {
        List<int> datas = new List<int>(ButtonMaps.Length*3+AxisMaps.Length*3 + DualAxisMaps.Length*6);

        for(int i = 0; i < InputMaps.Length; i++)
        {
            datas.AddRange(InputMaps[i].SerializeToInts());
        }
        return datas.ToArray();
    }

    private void RevertToDefaultControls()
    {
        Gamepad = true;
        controllerNumber = 1;
        Start = new ButtonMap(new ButtonMapData() { name = "Start", buttonMapName = "7" }, this);
        AltStart = new ButtonMap(new ButtonMapData() { name = "Alt Start", buttonMapName = "6" }, this);
        Confirm = new ButtonMap(new ButtonMapData() { name = "Confirm", buttonMapName = "0" }, this);
        Back = new ButtonMap(new ButtonMapData() { name = "Back", buttonMapName = "1" }, this);
        Jump = new ButtonMap(new ButtonMapData() { name = "Jump", buttonMapName = "0" }, this);
        Reload = new ButtonMap(new ButtonMapData() { name = "Reload/Interact", buttonMapName = "2" }, this);
        SwapPrimary = new ButtonMap(new ButtonMapData() { name = "Swap Primary", buttonMapName = "3" }, this);
        SwapSecondary = new ButtonMap(new ButtonMapData() { name = "Swap Secondary", buttonMapName = "1" }, this);
        Crouch = new ButtonMap(new ButtonMapData() { name = "Crouch", buttonMapName = "8" }, this);
        Zoom = new ButtonMap(new ButtonMapData() { name = "Zoom", buttonMapName = "9" }, this);
        UsePrimary = new AxisMap(new AxisMapData() { name = "Use Primary", positiveAxisName = "9", is0To1Axis = true }, this);
        UseSecondary = new AxisMap(new AxisMapData() { name = "Use Secondary", positiveAxisName = "8", is0To1Axis = true }, this);
        Movement = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "1" } }, this);
        Look = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "3" }, yAxisMapData = new AxisMapData() { positiveAxisName = "4" } }, this);
    }

    private void RevertToDefaultPCControls()
    {
        Gamepad = false;
        controllerNumber = 0;
        Start = new ButtonMap(new ButtonMapData() { name = "Start", buttonMapName = "escape" }, this);
        AltStart = new ButtonMap(new ButtonMapData() { name = "Alt Start", buttonMapName = "tab" }, this);
        Confirm = new ButtonMap(new ButtonMapData() { name = "Confirm", buttonMapName = "enter" }, this);
        Back = new ButtonMap(new ButtonMapData() { name = "Back", buttonMapName = "backspace" }, this);
        Jump = new ButtonMap(new ButtonMapData() { name = "Jump", buttonMapName = "space" }, this);
        Reload = new ButtonMap(new ButtonMapData() { name = "Reload/Interact", buttonMapName = "r" }, this);
        SwapPrimary = new ButtonMap(new ButtonMapData() { name = "Swap Primary", buttonMapName = "q" }, this);
        SwapSecondary = new ButtonMap(new ButtonMapData() { name = "Swap Secondary", buttonMapName = "e" }, this);
        Crouch = new ButtonMap(new ButtonMapData() { name = "Crouch", buttonMapName = "left ctrl" }, this);
        Zoom = new ButtonMap(new ButtonMapData() { name = "Zoom", buttonMapName = "mouse 2" }, this);
        UsePrimary = new AxisMap(new AxisMapData() { name = "Use Primary", positiveAxisName = "mouse 0", isVirtual = true, is0To1Axis = true }, this);
        UseSecondary = new AxisMap(new AxisMapData() { name = "Use Secondary", positiveAxisName = "mouse 1", isVirtual = true, is0To1Axis = true }, this);
        Movement = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "d", negativeAxisName = "a", isVirtual = true }, yAxisMapData = new AxisMapData() { positiveAxisName = "w", negativeAxisName = "s", isVirtual = true } }, this);
        Look = new DualAxisMap(new DualAxisMapData() { name = "Look", xAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_1" } }, this);
    }

        public static InputController Default()
    {
        InputController inputController = new InputController();
        inputController.RevertToDefaultControls();
        return inputController;
    }

    public static InputController DefaultPC()
    {
        InputController inputController = new InputController();
        inputController.RevertToDefaultPCControls();
        return inputController;
    }

    public static InputController[] DefaultInputControllers()
    {
#if UNITY_STANDALONE
        InputController[] configs = (new InputController[] { InputController.DefaultPC() }).Concat(Enumerable.Repeat(InputController.Default(), 3)).ToArray();
        for (int i = 0; i < configs.Length; i++)
        {
            configs[i].controllerNumber = Mathf.Max(i - 1, 0);
        }
        return configs;
#else
        InputController[] configs = Enumerable.Repeat(InputController.Default(), 4).ToArray();
        for(int i = 0; i < configs.Length; i++)
        {
            configs[i].controllerNumber = i;
        }
        return configs;
#endif
    }

}
[System.Serializable]
public abstract class InputMap
{
    protected InputController config;
    public static bool IsTestingForInput { get; protected set; }
    public string MapMessage { get; protected set; }
    protected const int InputCheckTime = 5;

    public InputMap(InputController _inputController)
    {
        config = _inputController;
    }

    public abstract IEnumerator TestForInput();
    public abstract int[] SerializeToInts();
    }
[System.Serializable]
public class ButtonMap : InputMap
{
    public string Name { get { return buttonMapData.name; } }
    public string ButtonMapName { get { return buttonMapData.buttonMapName; } }
    public ButtonMapData ButtonMapData { get { return buttonMapData; } }
    private ButtonMapData buttonMapData;

    private string ButtonString = "";

    // FixedUpdate Checking
    private bool fixed_UP = false;
    private bool fixed_DOWN = false;
    private bool fixed_Val = false;

    private float currentFixedUpdateInterval = 0;
    private int latestUpdateRead = 0;
    private int lastUpdate = 0;
    

    public ButtonMap(ButtonMapData _buttonMapData, InputController _config):base(_config)
    {
        buttonMapData = _buttonMapData;
        MapMessage = ButtonMapName;
        UpdateButtonString();
    }

    public void ReInit()
    {
        MapMessage = ButtonMapName;
        UpdateButtonString();
    }

    private void UpdateButtonString()
    {
        int buttonNum;
        if (int.TryParse(ButtonMapName, out buttonNum))
        {
            ButtonString = Input.GetJoyButtonString(config.controllerNumber, buttonNum);
        }
        else
        {
            ButtonString = ButtonMapName;
        }
    }

    public bool Up()
    {
        if (CheckForFixedUpdate())
        {
            return fixed_UP;
        }
        if (IsTestingForInput) return false;
        return RawUp();
    }

    private bool RawUp()
    {
        return UInput.GetKeyUp(ButtonString);
    }

    public static implicit operator bool(ButtonMap b)
    {
        if (b.CheckForFixedUpdate())
        {
            return b.fixed_Val;
        }
        if (IsTestingForInput) return false;
        return RawValue(b);
    }

    private static bool RawValue(ButtonMap b)
    {
        return UInput.GetKey(b.ButtonString);
    }

    public bool Down()
    {
        if (CheckForFixedUpdate())
        {
            return fixed_DOWN;
        }
        if (IsTestingForInput) return false;
        return RawDown();
    }

    private bool RawDown()
    {
        return UInput.GetKeyDown(ButtonString);
    }

    private bool CheckForFixedUpdate()
    {
        if (!Input.CheckInputInFixedUpdate) { return false; }
        if (Time.inFixedTimeStep)
        {
            bool hasCheckedLatestInputAlready = latestUpdateRead == lastUpdate;
            bool isCheckingLatestInputInSameTime = Time.fixedTime == currentFixedUpdateInterval;
            if (isCheckingLatestInputInSameTime)
            {
            }
            else if (!hasCheckedLatestInputAlready)
            {
                latestUpdateRead = lastUpdate;
                currentFixedUpdateInterval = Time.fixedTime;
            }
            else
            {
                fixed_UP = false;
                fixed_DOWN = false;
            }
        }
        else if (lastUpdate != Time.frameCount)
        {
            if (lastUpdate > latestUpdateRead && currentFixedUpdateInterval == Time.fixedTime)
            {
                lastUpdate = Time.frameCount;
                fixed_UP |= RawUp();
                fixed_DOWN |= RawDown();
                fixed_Val |= RawValue(this);

            }
            else
            {
                lastUpdate = Time.frameCount;
                fixed_UP = RawUp();
                fixed_DOWN = RawDown();
                fixed_Val = RawValue(this);
            }
        }
        return true;
    }

    public override IEnumerator TestForInput()
    {
        if (!IsTestingForInput)
        {
            IsTestingForInput = true;
            ButtonMapData originalData = buttonMapData;
            buttonMapData.buttonMapName = "";
            MapMessage = "Press the button...Ready";
            yield return new WaitForSeconds(2f);
            float startTime = Time.time;
            yield return new WaitWhile(() =>
            {
                buttonMapData.buttonMapName = Input.DetectButton(config);
                MapMessage = "Press the button..." + Mathf.RoundToInt(InputCheckTime - (Time.time - startTime)).ToString();
                return buttonMapData.buttonMapName == "" && Time.time - startTime < InputCheckTime;
            });
            if (buttonMapData.buttonMapName == "") { buttonMapData = originalData; }
            MapMessage = ButtonMapData.buttonMapName;
            IsTestingForInput = false;
            UpdateButtonString();
        }
    }

    public override string ToString()
    {
        if (Input.GetJoystickNames().Length > config.controllerNumber && Input.GetJoystickNames()[config.controllerNumber].ToLower().Contains("box"))
        {
            return Input.JoyButtonNumToXboxControllerMap(MapMessage);
        }
        return MapMessage;
    }

    public override int[] SerializeToInts()
    {
        return new int[] { fixed_DOWN ? 1 : 0, fixed_UP ? 1 : 0, fixed_Val ? 1 : 0 };
    }
}
[System.Serializable]
public class AxisMap : InputMap
{
    
    private const string PositiveAxisMessage = "Push in the positive direction...";
    private const string NegativeAxisMessage = "Push in the negative direction...";

    public string Name { get { return axisMapData.name; } }
    public bool IsInverted { get { return axisMapData.isInverted; } set { axisMapData.isInverted = value; } }
    public bool IsVirtual { get { return axisMapData.isVirtual; } }
    public string PositiveAxisName { get { return axisMapData.positiveAxisName; } }
    private float PositiveDir { get { return axisMapData.isPositiveAxisInverted? -1:1; } }
    public string NegativeAxisName { get { return axisMapData.negativeAxisName; } }
    private float NegativeDir { get { return axisMapData.isNegativeAxisInverted? -1 : 1; } }
    public int Sensitivity { get { return axisMapData.sensitivity; } set { axisMapData.sensitivity = value; } }
    public AxisMapData AxisMapData { get { return axisMapData; } }
    private AxisMapData axisMapData;

    private string PostiveAxisString = "";
    private string NegativeAxisString = "";

    public AxisMap(AxisMapData _axisMapData, InputController _inputController):base(_inputController)
    {
        axisMapData = _axisMapData;
        if(axisMapData.sensitivity == 0) { axisMapData.sensitivity = 1; }
        if(Name == null) { axisMapData.name = ""; }
        MapMessage = axisMapData.negativeAxisName == null ? PositiveAxisName : "(-)" + NegativeAxisName + " | (+)" + PositiveAxisName;
        UpdateAxisStrings();
    }

    
    public void ReInit()
    {
        if (axisMapData.sensitivity == 0) { axisMapData.sensitivity = 1; }
        if (Name == null) { axisMapData.name = ""; }
        MapMessage = axisMapData.negativeAxisName == null ? PositiveAxisName : "(-)" + NegativeAxisName + " | (+)" + PositiveAxisName;
        UpdateAxisStrings();
    }

    private void UpdateAxisStrings()
    {
        if (axisMapData.isVirtual)
        {
            int buttonNum;
            if (int.TryParse(PositiveAxisName, out buttonNum))
            {
                PostiveAxisString = Input.GetJoyButtonString(config.controllerNumber, buttonNum);
            }
            else
            {
                PostiveAxisString = PositiveAxisName;
            }
            if (NegativeAxisName != null)
            {
                if (int.TryParse(NegativeAxisName, out buttonNum))
                {
                    NegativeAxisString = Input.GetJoyButtonString(config.controllerNumber, buttonNum);
                }
                else
                {
                    NegativeAxisString = NegativeAxisName;
                }
            }
        }
        else
        {
            int buttonNum;
            if (int.TryParse(PositiveAxisName, out buttonNum))
            {
                PostiveAxisString = Input.GetJoyAxisString(config.controllerNumber, buttonNum);
            }
            else
            {
                PostiveAxisString = PositiveAxisName;
            }
            if (NegativeAxisName != null)
            {
                if (int.TryParse(NegativeAxisName, out buttonNum))
                {
                    NegativeAxisString = Input.GetJoyAxisString(config.controllerNumber, buttonNum);
                }
                else
                {
                    NegativeAxisString = NegativeAxisName;
                }
            }
        }
    }

    public float AxisMultiplier()
    {
        return (IsInverted ? -1f : 1f) *(1 + Sensitivity / 5);
    }

    public static implicit operator float(AxisMap b)
    {
        if (IsTestingForInput) return 0;
        return b.GetRawAxis() * b.AxisMultiplier();
    }

    public float GetRawAxis()
    {
        float value = 0;
        if (IsVirtual)
        {
            value += UInput.GetKey(PostiveAxisString) ? 1 : 0;
            if (NegativeAxisName != null)
            {
                value += UInput.GetKey(NegativeAxisString) ? -1 : 0;
            }
            return value;
        }
        value += UInput.GetAxis(PostiveAxisString) * PositiveDir;
        if (NegativeAxisName != null)
        {
            value += UInput.GetAxis(NegativeAxisString) * NegativeDir;
        }
        return value;
    }

    public override IEnumerator TestForInput()
    {
        if (!IsTestingForInput)
        {
            IsTestingForInput = true;
            IEnumerator inputMeat = TestForInputCore();
            while (inputMeat.MoveNext())
            {
                yield return inputMeat.Current; 
            }
            IsTestingForInput = false;
        }
    }
    public IEnumerator TestForInputCore()
    {
        AxisMapData originalData = axisMapData;
        MapMessage = PositiveAxisMessage + " Ready";
        yield return new WaitForSeconds(2f);
        float positiveValue = 0;
        float startTime = Time.time;
        yield return new WaitWhile(() =>
        {
            MapMessage = PositiveAxisMessage + Mathf.RoundToInt(InputCheckTime - (Time.time - startTime)).ToString();
            axisMapData.positiveAxisName = Input.DetectAxes(config, out positiveValue);
            if(axisMapData.positiveAxisName == "")
            {
                axisMapData.positiveAxisName = Input.DetectButton(config);
                if (axisMapData.positiveAxisName != "")
                {
                    axisMapData.isVirtual = true;
                    positiveValue = 1;
                }
            }
            axisMapData.isPositiveAxisInverted = positiveValue < 0;
            return axisMapData.positiveAxisName == "" && Time.time - startTime < InputCheckTime;
        });
        if (axisMapData.positiveAxisName == "") { axisMapData = originalData; }

        originalData = axisMapData;
        if (!axisMapData.is0To1Axis)
        {
            MapMessage = NegativeAxisMessage + " Ready";
            yield return new WaitForSeconds(2f);
            float negativeValue = 0;
            startTime = Time.time;
            yield return new WaitWhile(() =>
            {
            MapMessage = NegativeAxisMessage + Mathf.RoundToInt(InputCheckTime - (Time.time - startTime)).ToString();
                if (axisMapData.isVirtual)
                {
                    axisMapData.negativeAxisName = Input.DetectButton(config);
                    negativeValue = -1;
                }
                else
                {
                    axisMapData.negativeAxisName = Input.DetectAxes(config, out negativeValue);
                    axisMapData.isNegativeAxisInverted = negativeValue > 0;
                }
                return (axisMapData.negativeAxisName == "" || axisMapData.negativeAxisName == null) && Time.time - startTime < InputCheckTime;
            });
            if (axisMapData.negativeAxisName == "" || axisMapData.negativeAxisName == axisMapData.positiveAxisName)
            {
                axisMapData = originalData;
                axisMapData.negativeAxisName = null;
            }
        }
        UpdateAxisStrings();
        MapMessage = axisMapData.negativeAxisName == null ? (axisMapData.isPositiveAxisInverted ? ("Inverted ") : "") + PositiveAxisName : "(-)" +(axisMapData.isNegativeAxisInverted?("Inverted "):"") + NegativeAxisName + " | (+)" + (axisMapData.isPositiveAxisInverted? ("Inverted ") : "") + PositiveAxisName;
    }

    public override string ToString()
    {
        if (Input.GetJoystickNames().Length> config.controllerNumber&& Input.GetJoystickNames()[config.controllerNumber].ToLower().Contains("box"))
        {
            return Input.JoyAxisNumToXboxControllerMap(MapMessage);
        }
        return MapMessage;
    }

    public override int[] SerializeToInts()
    {
        return new int[] { (int)this*1000 };
    }
}
[System.Serializable]
public class DualAxisMap:InputMap
{
    public string Name { get { return dualAxisMapData.name; } }

    private const string HORIZONTAL_MESSAGE = "Horizontally ";
    private const string VERTICAL_MESSAGE = "Vertically ";
    public AxisMap xAxisMap;
    public AxisMap yAxisMap;
    public DualAxisMapData DualAxisMapData { get { return dualAxisMapData; } }
    private DualAxisMapData dualAxisMapData;

    public DualAxisMap(DualAxisMapData _dualAxisMapData, InputController _inputController) : base(_inputController)
    {
        dualAxisMapData = _dualAxisMapData;
        xAxisMap = new AxisMap(dualAxisMapData.xAxisMapData, config);
        yAxisMap = new AxisMap(dualAxisMapData.yAxisMapData, config);
        MapMessage = "X Axis: " + xAxisMap.ToString() + "\nY Axis: " + yAxisMap.ToString();
    }

    public void ReInit()
    {
        xAxisMap .ReInit();
        yAxisMap .ReInit();
        MapMessage = "X Axis: " + xAxisMap.ToString() + "\nY Axis: " + yAxisMap.ToString();
    }

    public static explicit operator Vector2(DualAxisMap b)
    {
        return new Vector2(b.xAxisMap,b.yAxisMap);
    }

    public static implicit operator float(DualAxisMap b)
    {
        return ((Vector2)b).magnitude;
    }

    public override IEnumerator TestForInput()
    {
        if (!IsTestingForInput)
        {
            IsTestingForInput = true;
            IEnumerator xAxisMapEnumerator = xAxisMap.TestForInputCore();
            while (xAxisMapEnumerator.MoveNext())
            {
                yield return xAxisMapEnumerator.Current;
            }
            IEnumerator yAxisMapEnumerator = yAxisMap.TestForInputCore();
            while (yAxisMapEnumerator.MoveNext())
            {
                yield return yAxisMapEnumerator.Current;
            }
            IsTestingForInput = false;
        }
    }
    

    public override string ToString()
    {
        return "X Axis: " + xAxisMap.ToString() + "\nY Axis: " + yAxisMap.ToString();
    }
    public override int[] SerializeToInts()
    {
        return new int[] { (int)xAxisMap * 1000, (int)yAxisMap * 1000 };
    }
}

public partial class Input : MonoBehaviour
{
    public static readonly Dictionary<string, string> JoyAxisToXboxMap =
    new Dictionary<string, string>
    {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
            {"6","D-Pad ↓↑" },
            {"5","D-Pad ←→" },
            {"9","Right Trigger" },
            {"8","Left Trigger" },
            {"10","Right Trigger" },
#endif
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ↓↑" },
            {"4","Right Stick ←→" },
            {"5","Left Trigger" },
            {"6","Right Trigger" },
#endif
#if UNITY_STANDALONE_LINUX
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"4","Right Stick ↓↑" },
            {"5","Right Stick ←→" },
            {"7","D-Pad ↓↑" },
            {"8","D-Pad ←→" },
            {"3","Left Trigger" },
            {"6","Right Trigger" },
#endif
    };

    public static readonly Dictionary<string, string> JoyButtonToXboxMap =
        new Dictionary<string, string>
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"8","Left Stick Click" },
            {"9","Right Stick Click" },
#endif
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            {"16","A" },
            {"17","B" },
            {"18","X" },
            {"19","Y" },
            {"13","Left Bumper" },
            {"14","Right Bumper" },
            {"10","Back Button" },
            {"9","Start Button" },
            {"11","Left Stick Click" },
            {"12","Right Stick Click" },
            {"5","D-Pad Up" },
            {"6","D-Pad Down" },
            {"7","D-Pad Left" },
            {"8","D-Pad Right" },
            {"15","Xbox Button" }
#endif
#if UNITY_STANDALONE_LINUX
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"9","Left Stick Click" },
            {"10","Right Stick Click" },
            {"13","D-Pad Up" },
            {"14","D-Pad Down" },
            {"11","D-Pad Left" },
            {"12","D-Pad Right" }
#endif
        };

    public static readonly Dictionary<string, string> JoyNumToPS4Map =
    new Dictionary<string, string>
    {
            #if UNITY_STANDALONE_WIN
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"8","Left Stick Click" },
            {"9","Right Stick Click" },
#endif
#if UNITY_STANDALONE_OSX
            {"16","A" },
            {"17","B" },
            {"18","X" },
            {"19","Y" },
            {"13","Left Bumper" },
            {"14","Right Bumper" },
            {"10","Back Button" },
            {"9","Start Button" },
            {"11","Left Stick Click" },
            {"12","Right Stick Click" },
            {"5","D-Pad Up" },
            {"6","D-Pad Down" },
            {"7","D-Pad Left" },
            {"8","D-Pad Right" },
            {"15","Xbox Button" }
#endif
#if UNITY_STANDALONE_LINUX
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"9","Left Stick Click" },
            {"10","Right Stick Click" },
            {"13","D-Pad Up" },
            {"14","D-Pad Down" },
            {"11","D-Pad Left" },
            {"12","D-Pad Right" }
#endif
    };

    public static string JoyButtonNumToXboxControllerMap(int i)
    {
        return JoyButtonNumToXboxControllerMap(i.ToString());
    }
    public static string JoyButtonNumToXboxControllerMap(string i)
    {
        if (JoyButtonToXboxMap.ContainsKey(i))
        {
            return JoyButtonToXboxMap[i];
        }
        else
        {
            return i;
        }
    }
    public static string JoyAxisNumToXboxControllerMap(int buttonNum)
    {
        return JoyAxisNumToXboxControllerMap(buttonNum.ToString());
    }
    public static string JoyAxisNumToXboxControllerMap(string buttonNum)
    {
        if (JoyAxisToXboxMap.ContainsKey(buttonNum))
        {
            return JoyAxisToXboxMap[buttonNum];
        }else if (buttonNum.Contains('-'))
        {
            string buttonNumCopy = buttonNum;
            buttonNum = buttonNum.Replace("(-)", "");
            buttonNum = buttonNum.Replace("(+)", "");
            buttonNum = buttonNum.Replace(" ", "");
            string[] splitI = buttonNum.Split('|');
            if (splitI.Length == 2)
            {
                buttonNum = "(-): " + (splitI[0].Contains("Inverted") ? "Inverted " : "") + JoyAxisNumToXboxControllerMap(splitI[0].Replace("Inverted", "")) + " | (+): " + (splitI[1].Contains("Inverted") ? "Inverted " : "") + JoyAxisNumToXboxControllerMap(splitI[1].Replace("Inverted", ""));
                return buttonNum;
            }

            buttonNum = buttonNumCopy;
        }
        else if (JoyAxisToXboxMap.ContainsKey(buttonNum.Replace("Inverted ", "")))
        {
            buttonNum = (buttonNum.Contains("Inverted ") ? "Inverted " : "") + JoyAxisNumToXboxControllerMap(buttonNum.Replace("Inverted ", ""));
            return buttonNum;
        }
        return buttonNum;
    }
}

// Unity functions
public partial class Input : MonoBehaviour
{
    //
    // Summary:
    //     Enables/Disables mouse simulation with touches. By default this option is enabled.
    public static bool simulateMouseWithTouches { get { return UInput.simulateMouseWithTouches; } set { UInput.simulateMouseWithTouches = value; } }
    //
    // Summary:
    //     Is any key or mouse button currently held down? (Read Only)
    public static bool anyKey { get { return UInput.anyKey; } }
    //
    // Summary:
    //     Returns true the first frame the user hits any key or mouse button. (Read Only)
    public static bool anyKeyDown { get { return UInput.anyKeyDown; } }
    //
    // Summary:
    //     Returns the keyboard input entered this frame. (Read Only)
    public static string inputString { get { return UInput.inputString; } }
    //
    // Summary:
    //     Last measured linear acceleration of a device in three-dimensional space. (Read
    //     Only)
    public static Vector3 acceleration { get { return UInput.acceleration; } }
    //
    // Summary:
    //     Returns list of acceleration measurements which occurred during the last frame.
    //     (Read Only) (Allocates temporary variables).
    public static AccelerationEvent[] accelerationEvents { get { return UInput.accelerationEvents; } }
    //
    // Summary:
    //     Number of acceleration measurements which occurred during last frame.
    public static int accelerationEventCount { get { return UInput.accelerationEventCount; } }
    //
    // Summary:
    //     Returns list of objects representing status of all touches during last frame.
    //     (Read Only) (Allocates temporary variables).
    public static Touch[] touches { get { return UInput.touches; } }
    //
    // Summary:
    //     Number of touches. Guaranteed not to change throughout the frame. (Read Only)
    public static int touchCount { get { return UInput.touchCount; } }
    //
    // Summary:
    //     Indicates if a mouse device is detected.
    public static bool mousePresent { get { return UInput.mousePresent; } }
    //
    // Summary:
    //     Property indicating whether keypresses are eaten by a textinput if it has focus
    //     (default true).
    [Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
    public static bool eatKeyPressOnTextFieldFocus { get { return UInput.eatKeyPressOnTextFieldFocus; } set { UInput.eatKeyPressOnTextFieldFocus = value; } }
    //
    // Summary:
    //     Returns true when Stylus Touch is supported by a device or platform.
    public static bool stylusTouchSupported { get { return UInput.stylusTouchSupported; } }
    //
    // Summary:
    //     Returns whether the device on which application is currently running supports
    //     touch input.
    public static bool touchSupported { get { return UInput.touchSupported; } }
    //
    // Summary:
    //     Property indicating whether the system handles multiple touches.
    public static bool multiTouchEnabled { get { return UInput.multiTouchEnabled; } set { UInput.multiTouchEnabled = value; } }
    //
    // Summary:
    //     Property for accessing device location (handheld devices only). (Read Only)
    public static LocationService location { get { return UInput.location; } }
    //
    // Summary:
    //     Property for accessing compass (handheld devices only). (Read Only)
    public static Compass compass { get { return UInput.compass; } }
    //
    // Summary:
    //     Device physical orientation as reported by OS. (Read Only)
    public static DeviceOrientation deviceOrientation { get { return UInput.deviceOrientation; } }
    //
    // Summary:
    //     Controls enabling and disabling of IME input composition.
    public static IMECompositionMode imeCompositionMode { get { return UInput.imeCompositionMode; } set { UInput.imeCompositionMode = value; } }
    //
    // Summary:
    //     The current IME composition string being typed by the user.
    public static string compositionString { get { return UInput.compositionString; } }
    //
    // Summary:
    //     Does the user have an IME keyboard input source selected?
    public static bool imeIsSelected { get { return UInput.imeIsSelected; } }
    //
    // Summary:
    //     Bool value which let's users check if touch pressure is supported.
    public static bool touchPressureSupported { get { return UInput.touchPressureSupported; } }
    //
    // Summary:
    //     The current mouse scroll delta. (Read Only)
    public static Vector2 mouseScrollDelta { get { return UInput.mouseScrollDelta; } }
    //
    // Summary:
    //     The current mouse position in pixel coordinates. (Read Only)
    public static Vector3 mousePosition { get { return UInput.mousePosition; } }
    //
    // Summary:
    //     Returns default gyroscope.
    public static Gyroscope gyro { get { return UInput.gyro; } }
    //
    // Summary:
    //     The current text input position used by IMEs to open windows.
    public static Vector2 compositionCursorPos { get { return UInput.compositionCursorPos; } set { UInput.compositionCursorPos = value; } }
    //
    // Summary:
    //     Should Back button quit the application? Only usable on Android, Windows Phone
    //     or Windows Tablets.
    public static bool backButtonLeavesApp { get { return UInput.backButtonLeavesApp; } set { UInput.backButtonLeavesApp = value; } }
    [Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
    public static bool isGyroAvailable { get { return UInput.isGyroAvailable; } }
    //
    // Summary:
    //     This property controls if input sensors should be compensated for screen orientation.
    public static bool compensateSensors { get { return UInput.compensateSensors; } set { UInput.compensateSensors = value; } }

    //
    // Summary:
    //     Returns specific acceleration measurement which occurred during last frame. (Does
    //     not allocate temporary variables).
    //
    // Parameters:
    //   index:
    public static AccelerationEvent GetAccelerationEvent(int index) { return UInput.GetAccelerationEvent(index); }
    //
    // Summary:
    //     Returns the value of the virtual axis identified by axisName.
    //
    // Parameters:
    //   axisName:
    public static float GetAxis(string axisName) { return UInput.GetAxis(axisName); }
    //
    // Summary:
    //     Returns the value of the virtual axis identified by axisName with no smoothing
    //     filtering applied.
    //
    // Parameters:
    //   axisName:
    public static float GetAxisRaw(string axisName) { return UInput.GetAxisRaw(axisName); }
    //
    // Summary:
    //     Returns true while the virtual button identified by buttonName is held down.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButton(string buttonName) { return UInput.GetButton(buttonName); }
    //
    // Summary:
    //     Returns true during the frame the user pressed down the virtual button identified
    //     by buttonName.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButtonDown(string buttonName) { return UInput.GetButtonDown(buttonName); }
    //
    // Summary:
    //     Returns true the first frame the user releases the virtual button identified
    //     by buttonName.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButtonUp(string buttonName) { return UInput.GetButtonUp(buttonName); }
    //
    // Summary:
    //     Returns an array of strings describing the connected joysticks.
    public static string[] GetJoystickNames() { return UInput.GetJoystickNames(); }
    //
    // Summary:
    //     Returns true while the user holds down the key identified by name. Think auto
    //     fire.
    //
    // Parameters:
    //   name:
    public static bool GetKey(string name) { return UInput.GetKey(name); }
    //
    // Summary:
    //     Returns true while the user holds down the key identified by the key KeyCode
    //     enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKey(KeyCode key) { return UInput.GetKey(key); }
    //
    // Summary:
    //     Returns true during the frame the user starts pressing down the key identified
    //     by the key KeyCode enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKeyDown(KeyCode key) { return UInput.GetKeyDown(key); }
    //
    // Summary:
    //     Returns true during the frame the user starts pressing down the key identified
    //     by name.
    //
    // Parameters:
    //   name:
    public static bool GetKeyDown(string name) { return UInput.GetKeyDown(name); }
    //
    // Summary:
    //     Returns true during the frame the user releases the key identified by the key
    //     KeyCode enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKeyUp(KeyCode key) { return UInput.GetKeyUp(key); }
    //
    // Summary:
    //     Returns true during the frame the user releases the key identified by name.
    //
    // Parameters:
    //   name:
    public static bool GetKeyUp(string name) { return UInput.GetKeyUp(name); }
    //
    // Summary:
    //     Returns whether the given mouse button is held down.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButton(int button) { return UInput.GetMouseButton(button); }
    //
    // Summary:
    //     Returns true during the frame the user pressed the given mouse button.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButtonDown(int button) { return UInput.GetMouseButtonDown(button); }
    //
    // Summary:
    //     Returns true during the frame the user releases the given mouse button.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButtonUp(int button) { return UInput.GetMouseButtonUp(button); }
    //
    // Summary:
    //     Returns object representing status of a specific touch. (Does not allocate temporary
    //     variables).
    //
    // Parameters:
    //   index:
    public static Touch GetTouch(int index) { return UInput.GetTouch(index); }
    //
    // Summary:
    //     Determine whether a particular joystick model has been preconfigured by Unity.
    //     (Linux-only).
    //
    // Parameters:
    //   joystickName:
    //     The name of the joystick to check (returned by Input.GetJoystickNames).
    //
    // Returns:
    //     True if the joystick layout has been preconfigured; false otherwise.
    public static bool IsJoystickPreconfigured(string joystickName)
    {
#if UNITY_STANDALONE_LINUX
        return UInput.IsJoystickPreconfigured(joystickName);
#else
        return false;
#endif
    }
    //
    // Summary:
    //     Resets all input. After ResetInputAxes all axes return to 0 and all buttons return
    //     to 0 for one frame.
    public static void ResetInputAxes() { UInput.ResetInputAxes(); }
}