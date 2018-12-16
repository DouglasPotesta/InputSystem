#if FLEXINPUT
using System.Collections;
using UnityEngine;
namespace Potesta.FlexInput
{
    [System.Serializable]
    public struct ButtonMapData
    {
        public string name;
        public string buttonMapName;
    }

    [System.Serializable]
    public class ButtonMap : InputMap
    {
        public override string Name { get { return buttonMapData.name; } }
        public string ButtonMapName { get { return buttonMapData.buttonMapName; } }

        public ButtonMapData ButtonMapData { get { return buttonMapData; } }
        [SerializeField]
        private ButtonMapData buttonMapData;

        private string ButtonString = "";

        // FixedUpdate Checking
        [SerializeField]
        private bool fixed_UP = false;
        [SerializeField]
        private bool fixed_DOWN = false;
        [SerializeField]
        private bool fixed_Val = false;

        private float currentFixedUpdateInterval = 0;
        private int latestUpdateRead = 0;
        private int lastUpdate = 0;
        [SerializeField]
        private int ticksUnchanged = 0;
        private int framesUnchanged = 0;

        public ButtonMap(ButtonMapData _buttonMapData, InputController _config) : base(_config)
        {
            buttonMapData = _buttonMapData;
            if (buttonMapData.buttonMapName == null) { buttonMapData.buttonMapName = "empty"; }
            MapMessage = ButtonMapName;
            if (GameInitializer.HasInitialized)
            {
                UpdateButtonString();
            }
        }

        public override void ReInit(InputController _inputController)
        {
            base.ReInit(_inputController);
            MapMessage = ButtonMapName;
            UpdateButtonString();
        }

        private void UpdateButtonString()
        {
            int buttonNum;
            if (int.TryParse(ButtonMapName, out buttonNum))
            {
                ButtonString = Input.GetJoyButtonString(controller.RawControllerNumber, buttonNum);
            }
            else
            {
                ButtonString = ButtonMapName;
            }
        }

        public bool Up()
        {
            if (controller.IsSerial) { return fixed_UP; }
            if (CheckForFixedUpdate())
            {
                return fixed_UP;
            }
            if (IsTestingForInput) return false;
            return RawUp();
        }

        public virtual bool RawUp()
        {
            return UnityEngine.Input.GetKeyUp(ButtonString);
        }

        public static implicit operator bool(ButtonMap b)
        {
            if (b.controller.IsSerial) { return b.fixed_Val; }
            if (b.CheckForFixedUpdate())
            {
                return b.fixed_Val;
            }
            if (IsTestingForInput) return false;
            return b.RawValue();
        }

        public virtual bool RawValue()
        {
            return UnityEngine.Input.GetKey(ButtonString);
        }

        public virtual bool Down()
        {
            if (controller.IsSerial) { return fixed_DOWN; }
            if (CheckForFixedUpdate())
            {
                return fixed_DOWN;
            }
            if (IsTestingForInput) return false;
            return RawDown();
        }

        private bool RawDown()
        {
            return UnityEngine.Input.GetKeyDown(ButtonString);
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
                bool rawUp = RawUp();
                bool rawDown = RawDown();
                bool rawValue = RawValue();
                if (lastUpdate > latestUpdateRead && currentFixedUpdateInterval == Time.fixedTime) // the subsequent extra frames between ticks
                {
                    if (rawValue == fixed_Val)
                    {// Only the framesUnchanged gets ticked because the this section represents extra frames between ticks.
                        framesUnchanged += 1;
                    }
                    else
                    {// if it was changed between ticks then ticks unchanged still needs to be reset to 0.
                        framesUnchanged = 0;
                        ticksUnchanged = 0;
                    }
                    lastUpdate = Time.frameCount;
                    fixed_UP |= rawUp;
                    fixed_DOWN |= rawDown;
                    fixed_Val |= rawValue;

                }
                else
                { // the first frame before a tick.
                    if(rawValue == fixed_Val)
                    {
                        ticksUnchanged += 1;
                        framesUnchanged += 1;
                    }
                    else
                    {
                        framesUnchanged = 0;
                        ticksUnchanged = 0;
                    }
                    lastUpdate = Time.frameCount;
                    fixed_UP = rawUp;
                    fixed_DOWN = rawDown;
                    fixed_Val = rawValue;
                }
            }
            return true;
        }

        public bool ExtrapolatedValue(int tframesAhead)
        {
            return fixed_Val;
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
                    buttonMapData.buttonMapName = Input.DetectButton(controller);
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
            if ((!Application.isPlaying ? false : Input.GetJoystickNames().Length > controller.RawControllerNumber) && Input.GetJoystickNames()[controller.RawControllerNumber].ToLower().Contains("box"))
            {
                return Input.JoyButtonNumToXboxControllerMap(MapMessage);
            }
            return MapMessage;
        }

        public override void SerializeValues()
        {
            if (!Input.CheckInputInFixedUpdate)
            {
                fixed_UP = Up();
                fixed_Val = this;
                fixed_DOWN = Down();
            }
        }
    }
}
#endif