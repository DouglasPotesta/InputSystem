using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using UInput = UnityEngine.Input;

[System.Serializable]
public class InputController : ScriptableObject
{
    public ControllerStatus controllerStatus;
    public bool Gamepad { get { return gamepad; } private set { gamepad = value; } }
    private bool gamepad = true;
    public int ControllerNumber { get { return controllerNumber; } /*TODO make this set private by moving controllerdisconnect logic in this class*/set { controllerNumber = value; } }
    private int controllerNumber = 0;
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


    //public ButtonMap Start;
    //public ButtonMap AltStart;
    //public ButtonMap Confirm;
    //public ButtonMap Back;
    //public ButtonMap Jump;
    //public ButtonMap Reload;
    //public ButtonMap SwapPrimary;
    //public ButtonMap SwapSecondary;
    //public ButtonMap Crouch;
    //public ButtonMap Zoom;
    //public AxisMap UsePrimary;
    //public AxisMap UseSecondary;
    //public DualAxisMap Movement;
    //public DualAxisMap Look;
    
    /// <summary>
    /// Accessor for all button maps on the controller
    /// </summary>
    public ButtonMap[] ButtonMaps
    {
        get
        {
                FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType == typeof(ButtonMap)).OrderBy(y => y.Name).ToArray();
                buttonMaps = fields.Select(y => {
                    object val = y.GetValue(this);
                    if (val == null)
                    {
                        val = new ButtonMap(new ButtonMapData(), this);
                        y.SetValue(this, val);
                    }
                    return (ButtonMap)y.GetValue(this);
                }).ToArray();
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
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType == typeof(AxisMap)).OrderBy(y => y.Name).ToArray();
            axisMaps = fields.Select(y => {
                object val = y.GetValue(this);
                if (val == null)
                {
                    val = new AxisMap(new AxisMapData(), this);
                    y.SetValue(this, val);
                }
                return (AxisMap)y.GetValue(this);
            }).ToArray();
            return axisMaps;
        }

        set
        {
            axisMaps = value;
        }
    }

    /// <summary>
    /// Accessor for all Dual Axis maps on the controller
    /// </summary>
    public DualAxisMap[] DualAxisMaps
    {
        get
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType == typeof(DualAxisMap)).OrderBy(y => y.Name).ToArray();
            dualAxisMaps = fields.Select(y => {
                object val = y.GetValue(this);
                if (val == null)
                {
                    val = new DualAxisMap(new DualAxisMapData(), this);
                    y.SetValue(this, val);
                }
                return (DualAxisMap)y.GetValue(this);
            }).ToArray();
            return dualAxisMaps;
        }

        set
        {
            dualAxisMaps = value;
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
                inputMaps = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => x.FieldType.IsSubclassOf(typeof(InputMap))).OrderBy(y => y.Name).Select(y => (InputMap)y.GetValue(this)).ToArray();
            }
            return inputMaps;
        }

        set
        {
            inputMaps = value;
        }
    }
    /// <summary>
    /// Initializes input maps for getting their string names.
    /// </summary>
    public void InitializeControls()
    {
        for (int i = 0; i < ButtonMaps.Length; i++)
        {
            if (ButtonMaps[i] == null)
                ButtonMaps[i] = new ButtonMap(new ButtonMapData(), this);
        }
        for (int i = 0; i < AxisMaps.Length; i++)
        {
            if (AxisMaps[i] == null)
                AxisMaps[i] = new AxisMap(new AxisMapData(), this);
        }
        for (int i = 0; i < DualAxisMaps.Length; i++)
        {
            if (DualAxisMaps[i] == null)
                DualAxisMaps[i] = new DualAxisMap(new DualAxisMapData(), this);
        }
        for (int i = 0; i < InputMaps.Length; i++)
        {
            InputMaps[i].ReInit();
        }
        //Start.ReInit();
        //AltStart.ReInit();
        //Confirm.ReInit();
        //Back.ReInit();
        //Jump.ReInit();
        //Reload.ReInit();
        //SwapPrimary.ReInit();
        //SwapSecondary.ReInit();
        //Crouch.ReInit();
        //Zoom.ReInit();
        //UsePrimary.ReInit();
        //UseSecondary.ReInit();
        //Movement.ReInit();
        //Look.ReInit();
    }

    /// <summary>
    /// Switches the controllers input back to default gamepad controls.
    /// </summary>
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

    /// <summary>
    /// Switches the controllers input back to pc controls
    /// </summary>
    public void SwitchToPC()
    {
        if (Input.Num[0] == this)
        {
            RevertToDefaultPCControls();
            InitializeControls();
            controllerStatus = ControllerStatus.Connected;
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
    private void RevertToDefaultControls()
    {
        Gamepad = true;
        ControllerNumber = 1;
        //Start = new ButtonMap(new ButtonMapData() { name = "Start", buttonMapName = "7" }, this);
        //AltStart = new ButtonMap(new ButtonMapData() { name = "Alt Start", buttonMapName = "6" }, this);
        //Confirm = new ButtonMap(new ButtonMapData() { name = "Confirm", buttonMapName = "0" }, this);
        //Back = new ButtonMap(new ButtonMapData() { name = "Back", buttonMapName = "1" }, this);
        //Jump = new ButtonMap(new ButtonMapData() { name = "Jump", buttonMapName = "0" }, this);
        //Reload = new ButtonMap(new ButtonMapData() { name = "Reload/Interact", buttonMapName = "2" }, this);
        //SwapPrimary = new ButtonMap(new ButtonMapData() { name = "Swap Primary", buttonMapName = "3" }, this);
        //SwapSecondary = new ButtonMap(new ButtonMapData() { name = "Swap Secondary", buttonMapName = "1" }, this);
        //Crouch = new ButtonMap(new ButtonMapData() { name = "Crouch", buttonMapName = "8" }, this);
        //Zoom = new ButtonMap(new ButtonMapData() { name = "Zoom", buttonMapName = "9" }, this);
        //UsePrimary = new AxisMap(new AxisMapData() { name = "Use Primary", positiveAxisName = "9", is0To1Axis = true }, this);
        //UseSecondary = new AxisMap(new AxisMapData() { name = "Use Secondary", positiveAxisName = "8", is0To1Axis = true }, this);
        //Movement = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "1" } }, this);
        //Look = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "3" }, yAxisMapData = new AxisMapData() { positiveAxisName = "4" } }, this);
    }

    private void RevertToDefaultPCControls()
    {
        Gamepad = false;
        ControllerNumber = 0;
        //Start = new ButtonMap(new ButtonMapData() { name = "Start", buttonMapName = "escape" }, this);
        //AltStart = new ButtonMap(new ButtonMapData() { name = "Alt Start", buttonMapName = "tab" }, this);
        //Confirm = new ButtonMap(new ButtonMapData() { name = "Confirm", buttonMapName = "enter" }, this);
        //Back = new ButtonMap(new ButtonMapData() { name = "Back", buttonMapName = "backspace" }, this);
        //Jump = new ButtonMap(new ButtonMapData() { name = "Jump", buttonMapName = "space" }, this);
        //Reload = new ButtonMap(new ButtonMapData() { name = "Reload/Interact", buttonMapName = "r" }, this);
        //SwapPrimary = new ButtonMap(new ButtonMapData() { name = "Swap Primary", buttonMapName = "q" }, this);
        //SwapSecondary = new ButtonMap(new ButtonMapData() { name = "Swap Secondary", buttonMapName = "e" }, this);
        //Crouch = new ButtonMap(new ButtonMapData() { name = "Crouch", buttonMapName = "left ctrl" }, this);
        //Zoom = new ButtonMap(new ButtonMapData() { name = "Zoom", buttonMapName = "mouse 2" }, this);
        //UsePrimary = new AxisMap(new AxisMapData() { name = "Use Primary", positiveAxisName = "mouse 0", isVirtual = true, is0To1Axis = true }, this);
        //UseSecondary = new AxisMap(new AxisMapData() { name = "Use Secondary", positiveAxisName = "mouse 1", isVirtual = true, is0To1Axis = true }, this);
        //Movement = new DualAxisMap(new DualAxisMapData() { name = "Movement", xAxisMapData = new AxisMapData() { positiveAxisName = "d", negativeAxisName = "a", isVirtual = true }, yAxisMapData = new AxisMapData() { positiveAxisName = "w", negativeAxisName = "s", isVirtual = true } }, this);
        //Look = new DualAxisMap(new DualAxisMapData() { name = "Look", xAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_0" }, yAxisMapData = new AxisMapData() { positiveAxisName = "mouse_axis_1" } }, this);
    }

    /// <summary>
    /// Returns a default gamepad controller
    /// </summary>
    /// <returns></returns>
    public static InputController Default()
    {
        InputController inputController = CreateInstance<InputController>();
        inputController.RevertToDefaultControls();
        return inputController;
    }
    /// <summary>
    /// returns a default pc controller
    /// </summary>
    /// <returns></returns>
    public static InputController DefaultPC()
    {
        InputController inputController = CreateInstance<InputController>();
        inputController.RevertToDefaultPCControls();
        return inputController;
    }
    /// <summary>
    /// Returns the default settings for all controllers
    /// </summary>
    /// <returns></returns>
    public static InputController[] DefaultInputControllers()
    {
#if UNITY_STANDALONE
        InputController[] configs = (new InputController[] { InputController.DefaultPC() }).Concat(Enumerable.Repeat(InputController.Default(), 3)).ToArray();
        for (int i = 0; i < configs.Length; i++)
        {
            configs[i].ControllerNumber = Mathf.Max(i - 1, 0);
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