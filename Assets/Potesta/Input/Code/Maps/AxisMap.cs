#if FLEXINPUT
using System;
using System.Collections;
using UnityEngine;

namespace Potesta.FlexInput
{

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
    public class AxisMap : InputMap
    {

        private const string PositiveAxisMessage = "Push in the positive direction...";
        private const string NegativeAxisMessage = "Push in the negative direction...";

        public override string Name { get { return axisMapData.name; } }
        public bool IsInverted { get { return axisMapData.isInverted; } set { axisMapData.isInverted = value; } }
        public bool IsVirtual { get { return axisMapData.isVirtual; } }
        public string PositiveAxisName { get { return axisMapData.positiveAxisName; } }
        private float PositiveDir { get { return axisMapData.isPositiveAxisInverted ? -1 : 1; } }
        public string NegativeAxisName { get { return axisMapData.negativeAxisName; } }
        private float NegativeDir { get { return axisMapData.isNegativeAxisInverted ? -1 : 1; } }
        public int Sensitivity { get { return axisMapData.sensitivity; } set { axisMapData.sensitivity = value; } }
        public AxisMapData AxisMapData { get { return axisMapData; } }

        public float DeltaValue
        {
            get
            {
                return deltaValue;
            }

            private set
            {
                deltaValue = value;
            }
        }

        [SerializeField]
        private AxisMapData axisMapData;

        private float serialValue = 0;
        private float deltaValue = 0;

        private string PositiveAxisString = "";
        private string NegativeAxisString = "";
        private Func<float, float> PositiveAxisModifier = (float rawValue)=> {return rawValue; };
        private Func<float, float> NegativeAxisModifier = (float rawValue)=> { return rawValue; };

        public AxisMap(AxisMapData _axisMapData, InputController _inputController) : base(_inputController)
        {
            axisMapData = _axisMapData;
            if (axisMapData.sensitivity == 0) { axisMapData.sensitivity = 1; }
            if (Name == null) { axisMapData.name = ""; }
            MapMessage = axisMapData.negativeAxisName == null ? PositiveAxisName : "(-)" + NegativeAxisName + " | (+)" + PositiveAxisName;
            UpdateAxisStrings();
        }


        public override void ReInit(InputController _inputController)
        {
            base.ReInit(_inputController);
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
                    PositiveAxisString = Input.GetJoyButtonString(controller.RawControllerNumber, buttonNum);
                }
                else
                {
                    PositiveAxisString = PositiveAxisName;
                }
                if (NegativeAxisName != null)
                {
                    if (int.TryParse(NegativeAxisName, out buttonNum))
                    {
                        NegativeAxisString = Input.GetJoyButtonString(controller.RawControllerNumber, buttonNum);
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
                    PositiveAxisString = Input.GetJoyAxisString(controller.RawControllerNumber, buttonNum);
                }
                else
                {
                    PositiveAxisString = PositiveAxisName;
                }
                PositiveAxisModifier = controller.AxisCalibrations.ContainsKey(PositiveAxisName) ? controller.AxisCalibrations[PositiveAxisName] : SameValue; 
                if (NegativeAxisName != null)
                {
                    if (int.TryParse(NegativeAxisName, out buttonNum))
                    {
                        NegativeAxisString = Input.GetJoyAxisString(controller.RawControllerNumber, buttonNum);
                    }
                    else
                    {
                        NegativeAxisString = NegativeAxisName;
                    }
                }
                NegativeAxisModifier = NegativeAxisName != null && controller.AxisCalibrations.ContainsKey(NegativeAxisName) ? controller.AxisCalibrations[NegativeAxisName] : SameValue;
            }
        }

        public float AxisMultiplier()
        {
            return (IsInverted ? -1f : 1f) * (1 + Sensitivity / 5);
        }

        public static implicit operator float(AxisMap b)
        {
            if (IsTestingForInput) return 0;
            return b.controller.IsSerial ? b.serialValue : b.GetRawAxis() * b.AxisMultiplier();
        }

        public virtual float GetRawAxis()
        {
            float value = 0;
            if (IsVirtual)
            {
                value += UnityEngine.Input.GetKey(PositiveAxisString) ? 1 : 0;
                if (NegativeAxisName != null)
                {
                    value += UnityEngine.Input.GetKey(NegativeAxisString) ? -1 : 0;
                }
                return value;
            }
            value += PositiveAxisModifier(UnityEngine.Input.GetAxis(PositiveAxisString)) * PositiveDir;
            if (NegativeAxisName != null)
            {
                value += NegativeAxisModifier(UnityEngine.Input.GetAxis(NegativeAxisString)) * NegativeDir;
            }
            return value;
        }

        public float ExtrapolatedValue(int tTicksAhead)
        {
            return Mathf.Clamp01(DeltaValue * tTicksAhead + serialValue);
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
                axisMapData.positiveAxisName = Input.DetectAxes(controller, out positiveValue);
                if (axisMapData.positiveAxisName == "")
                {
                    axisMapData.positiveAxisName = Input.DetectButton(controller);
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
                        axisMapData.negativeAxisName = Input.DetectButton(controller);
                        negativeValue = -1;
                    }
                    else
                    {
                        axisMapData.negativeAxisName = Input.DetectAxes(controller, out negativeValue);
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
            MapMessage = axisMapData.negativeAxisName == null ? (axisMapData.isPositiveAxisInverted ? ("Inverted ") : "") + PositiveAxisName : "(-)" + (axisMapData.isNegativeAxisInverted ? ("Inverted ") : "") + NegativeAxisName + " | (+)" + (axisMapData.isPositiveAxisInverted ? ("Inverted ") : "") + PositiveAxisName;
        }

        public override string ToString()
        {
            if (Input.GetJoystickNames().Length > controller.RawControllerNumber && Input.GetJoystickNames()[controller.RawControllerNumber].ToLower().Contains("box"))
            {
                return Input.JoyAxisNumToXboxControllerMap(MapMessage);
            }
            return MapMessage;
        }

        /// <summary>
        /// Assumes serialized every tick
        /// </summary>
        public override void SerializeValues()
        {
            DeltaValue = this - serialValue;
            serialValue = this;
        }

        private static float SameValue(float value)
        {
            return value;
        }
    }
}
#endif