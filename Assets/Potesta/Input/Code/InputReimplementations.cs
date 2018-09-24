using System;
using UnityEngine;

// Unity functions
public partial class Input
{
    //
    // Summary:
    //     Enables/Disables mouse simulation with touches. By default this option is enabled.
    public static bool simulateMouseWithTouches { get { return UnityEngine.Input.simulateMouseWithTouches; } set { UnityEngine.Input.simulateMouseWithTouches = value; } }
    //
    // Summary:
    //     Is any key or mouse button currently held down? (Read Only)
    public static bool anyKey { get { return UnityEngine.Input.anyKey; } }
    //
    // Summary:
    //     Returns true the first frame the user hits any key or mouse button. (Read Only)
    public static bool anyKeyDown { get { return UnityEngine.Input.anyKeyDown; } }
    //
    // Summary:
    //     Returns the keyboard input entered this frame. (Read Only)
    public static string inputString { get { return UnityEngine.Input.inputString; } }
    //
    // Summary:
    //     Last measured linear acceleration of a device in three-dimensional space. (Read
    //     Only)
    public static Vector3 acceleration { get { return UnityEngine.Input.acceleration; } }
    //
    // Summary:
    //     Returns list of acceleration measurements which occurred during the last frame.
    //     (Read Only) (Allocates temporary variables).
    public static AccelerationEvent[] accelerationEvents { get { return UnityEngine.Input.accelerationEvents; } }
    //
    // Summary:
    //     Number of acceleration measurements which occurred during last frame.
    public static int accelerationEventCount { get { return UnityEngine.Input.accelerationEventCount; } }
    //
    // Summary:
    //     Returns list of objects representing status of all touches during last frame.
    //     (Read Only) (Allocates temporary variables).
    public static Touch[] touches { get { return UnityEngine.Input.touches; } }
    //
    // Summary:
    //     Number of touches. Guaranteed not to change throughout the frame. (Read Only)
    public static int touchCount { get { return UnityEngine.Input.touchCount; } }
    //
    // Summary:
    //     Indicates if a mouse device is detected.
    public static bool mousePresent { get { return UnityEngine.Input.mousePresent; } }
    //
    // Summary:
    //     Property indicating whether keypresses are eaten by a textinput if it has focus
    //     (default true).
    [Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
    public static bool eatKeyPressOnTextFieldFocus { get { return UnityEngine.Input.eatKeyPressOnTextFieldFocus; } set { UnityEngine.Input.eatKeyPressOnTextFieldFocus = value; } }
    //
    // Summary:
    //     Returns true when Stylus Touch is supported by a device or platform.
    public static bool stylusTouchSupported { get { return UnityEngine.Input.stylusTouchSupported; } }
    //
    // Summary:
    //     Returns whether the device on which application is currently running supports
    //     touch input.
    public static bool touchSupported { get { return UnityEngine.Input.touchSupported; } }
    //
    // Summary:
    //     Property indicating whether the system handles multiple touches.
    public static bool multiTouchEnabled { get { return UnityEngine.Input.multiTouchEnabled; } set { UnityEngine.Input.multiTouchEnabled = value; } }
    //
    // Summary:
    //     Property for accessing device location (handheld devices only). (Read Only)
    public static LocationService location { get { return UnityEngine.Input.location; } }
    //
    // Summary:
    //     Property for accessing compass (handheld devices only). (Read Only)
    public static Compass compass { get { return UnityEngine.Input.compass; } }
    //
    // Summary:
    //     Device physical orientation as reported by OS. (Read Only)
    public static DeviceOrientation deviceOrientation { get { return UnityEngine.Input.deviceOrientation; } }
    //
    // Summary:
    //     Controls enabling and disabling of IME input composition.
    public static IMECompositionMode imeCompositionMode { get { return UnityEngine.Input.imeCompositionMode; } set { UnityEngine.Input.imeCompositionMode = value; } }
    //
    // Summary:
    //     The current IME composition string being typed by the user.
    public static string compositionString { get { return UnityEngine.Input.compositionString; } }
    //
    // Summary:
    //     Does the user have an IME keyboard input source selected?
    public static bool imeIsSelected { get { return UnityEngine.Input.imeIsSelected; } }
    //
    // Summary:
    //     Bool value which let's users check if touch pressure is supported.
    public static bool touchPressureSupported { get { return UnityEngine.Input.touchPressureSupported; } }
    //
    // Summary:
    //     The current mouse scroll delta. (Read Only)
    public static Vector2 mouseScrollDelta { get { return UnityEngine.Input.mouseScrollDelta; } }
    //
    // Summary:
    //     The current mouse position in pixel coordinates. (Read Only)
    public static Vector3 mousePosition { get { return UnityEngine.Input.mousePosition; } }
    //
    // Summary:
    //     Returns default gyroscope.
    public static Gyroscope gyro { get { return UnityEngine.Input.gyro; } }
    //
    // Summary:
    //     The current text input position used by IMEs to open windows.
    public static Vector2 compositionCursorPos { get { return UnityEngine.Input.compositionCursorPos; } set { UnityEngine.Input.compositionCursorPos = value; } }
    //
    // Summary:
    //     Should Back button quit the application? Only usable on Android, Windows Phone
    //     or Windows Tablets.
    public static bool backButtonLeavesApp { get { return UnityEngine.Input.backButtonLeavesApp; } set { UnityEngine.Input.backButtonLeavesApp = value; } }
    [Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
    public static bool isGyroAvailable { get { return UnityEngine.Input.isGyroAvailable; } }
    //
    // Summary:
    //     This property controls if input sensors should be compensated for screen orientation.
    public static bool compensateSensors { get { return UnityEngine.Input.compensateSensors; } set { UnityEngine.Input.compensateSensors = value; } }

    //
    // Summary:
    //     Returns specific acceleration measurement which occurred during last frame. (Does
    //     not allocate temporary variables).
    //
    // Parameters:
    //   index:
    public static AccelerationEvent GetAccelerationEvent(int index) { return UnityEngine.Input.GetAccelerationEvent(index); }
    //
    // Summary:
    //     Returns the value of the virtual axis identified by axisName.
    //
    // Parameters:
    //   axisName:
    public static float GetAxis(string axisName) { return UnityEngine.Input.GetAxis(axisName); }
    //
    // Summary:
    //     Returns the value of the virtual axis identified by axisName with no smoothing
    //     filtering applied.
    //
    // Parameters:
    //   axisName:
    public static float GetAxisRaw(string axisName) { return UnityEngine.Input.GetAxisRaw(axisName); }
    //
    // Summary:
    //     Returns true while the virtual button identified by buttonName is held down.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButton(string buttonName) { return UnityEngine.Input.GetButton(buttonName); }
    //
    // Summary:
    //     Returns true during the frame the user pressed down the virtual button identified
    //     by buttonName.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButtonDown(string buttonName) { return UnityEngine.Input.GetButtonDown(buttonName); }
    //
    // Summary:
    //     Returns true the first frame the user releases the virtual button identified
    //     by buttonName.
    //
    // Parameters:
    //   buttonName:
    public static bool GetButtonUp(string buttonName) { return UnityEngine.Input.GetButtonUp(buttonName); }
    //
    // Summary:
    //     Returns an array of strings describing the connected joysticks.
    public static string[] GetJoystickNames() { return UnityEngine.Input.GetJoystickNames(); }
    //
    // Summary:
    //     Returns true while the user holds down the key identified by name. Think auto
    //     fire.
    //
    // Parameters:
    //   name:
    public static bool GetKey(string name) { return UnityEngine.Input.GetKey(name); }
    //
    // Summary:
    //     Returns true while the user holds down the key identified by the key KeyCode
    //     enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKey(KeyCode key) { return UnityEngine.Input.GetKey(key); }
    //
    // Summary:
    //     Returns true during the frame the user starts pressing down the key identified
    //     by the key KeyCode enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKeyDown(KeyCode key) { return UnityEngine.Input.GetKeyDown(key); }
    //
    // Summary:
    //     Returns true during the frame the user starts pressing down the key identified
    //     by name.
    //
    // Parameters:
    //   name:
    public static bool GetKeyDown(string name) { return UnityEngine.Input.GetKeyDown(name); }
    //
    // Summary:
    //     Returns true during the frame the user releases the key identified by the key
    //     KeyCode enum parameter.
    //
    // Parameters:
    //   key:
    public static bool GetKeyUp(KeyCode key) { return UnityEngine.Input.GetKeyUp(key); }
    //
    // Summary:
    //     Returns true during the frame the user releases the key identified by name.
    //
    // Parameters:
    //   name:
    public static bool GetKeyUp(string name) { return UnityEngine.Input.GetKeyUp(name); }
    //
    // Summary:
    //     Returns whether the given mouse button is held down.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButton(int button) { return UnityEngine.Input.GetMouseButton(button); }
    //
    // Summary:
    //     Returns true during the frame the user pressed the given mouse button.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButtonDown(int button) { return UnityEngine.Input.GetMouseButtonDown(button); }
    //
    // Summary:
    //     Returns true during the frame the user releases the given mouse button.
    //
    // Parameters:
    //   button:
    public static bool GetMouseButtonUp(int button) { return UnityEngine.Input.GetMouseButtonUp(button); }
    //
    // Summary:
    //     Returns object representing status of a specific touch. (Does not allocate temporary
    //     variables).
    //
    // Parameters:
    //   index:
    public static Touch GetTouch(int index) { return UnityEngine.Input.GetTouch(index); }
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
        return UnityEngine.Input.IsJoystickPreconfigured(joystickName);
#else
        return false;
#endif
    }
    //
    // Summary:
    //     Resets all input. After ResetInputAxes all axes return to 0 and all buttons return
    //     to 0 for one frame.
    public static void ResetInputAxes() { UnityEngine.Input.ResetInputAxes(); }
}