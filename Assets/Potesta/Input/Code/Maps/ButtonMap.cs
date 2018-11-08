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
        private bool fixed_UP = false;
        private bool fixed_DOWN = false;
        private bool fixed_Val = false;

        private float currentFixedUpdateInterval = 0;
        private int latestUpdateRead = 0;
        private int lastUpdate = 0;


        public ButtonMap(ButtonMapData _buttonMapData, InputController _config) : base(_config)
        {
            buttonMapData = _buttonMapData;
            if (buttonMapData.buttonMapName == null) { buttonMapData.buttonMapName = "empty"; }
            MapMessage = ButtonMapName;
            if (Application.isPlaying)
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

        private bool RawUp()
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
            return RawValue(b);
        }

        private static bool RawValue(ButtonMap b)
        {
            return UnityEngine.Input.GetKey(b.ButtonString);
        }

        public bool Down()
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