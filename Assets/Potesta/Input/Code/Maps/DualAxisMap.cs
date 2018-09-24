using System.Collections;
using UnityEngine;

namespace Potesta.FlexInput
{
    [System.Serializable]
    public struct DualAxisMapData
    {
        public string name;
        public AxisMapData xAxisMapData;
        public AxisMapData yAxisMapData;
    }


    [System.Serializable]
    public class DualAxisMap : InputMap
    {
        public override string Name { get { return dualAxisMapData.name; } }

        private const string HORIZONTAL_MESSAGE = "Horizontally ";
        private const string VERTICAL_MESSAGE = "Vertically ";
        public AxisMap xAxisMap;
        public AxisMap yAxisMap;
        public DualAxisMapData DualAxisMapData { get { return dualAxisMapData; } }
        [SerializeField]
        private DualAxisMapData dualAxisMapData;

        private Vector2 serialValue { get { return new Vector2(xAxisMap, yAxisMap); } }

        public DualAxisMap(DualAxisMapData _dualAxisMapData, InputController _inputController) : base(_inputController)
        {
            dualAxisMapData = _dualAxisMapData;
            xAxisMap = new AxisMap(dualAxisMapData.xAxisMapData, controller);
            yAxisMap = new AxisMap(dualAxisMapData.yAxisMapData, controller);
            MapMessage = "X Axis: " + xAxisMap.MapMessage + "\nY Axis: " + yAxisMap.MapMessage;
        }

        public override void ReInit(InputController _inputController)
        {
            base.ReInit(_inputController);
            xAxisMap.ReInit(_inputController);
            yAxisMap.ReInit(_inputController);
            MapMessage = "X Axis: " + xAxisMap.ToString() + "\nY Axis: " + yAxisMap.ToString();
        }

        public static explicit operator Vector2(DualAxisMap b)
        {
            return b.controller.IsSerial ? b.serialValue : new Vector2(b.xAxisMap, b.yAxisMap);
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
        public override void SerializeValues()
        {
            xAxisMap.SerializeValues();
            yAxisMap.SerializeValues();
        }
    }
}