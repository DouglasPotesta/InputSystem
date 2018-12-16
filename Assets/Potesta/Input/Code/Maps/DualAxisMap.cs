#if FLEXINPUT
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
        public float x { get { return ((Vector2)this).x; } }
        public float y { get { return ((Vector2)this).y; } }

        [SerializeField]
        private Vector2 serialValue = Vector2.zero;
        public Vector2 SerialValue { get { return serialValue;}private set { serialValue = value; } }

        [SerializeField]
        private Vector2 deltaValue = Vector2.zero;
        public Vector2 DeltaValue
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
            return b.RawValue();
        }

        public virtual Vector2 RawValue()
        {
            return controller.IsSerial ? SerialValue : new Vector2(xAxisMap, yAxisMap);
        }

        public static implicit operator float(DualAxisMap b)
        {
            return b.RawMagnitude();
        }

        public virtual float RawMagnitude()
        {
            return RawValue().magnitude;
        }

        public Vector2 ExtrapolatedValue(int tTicksAhead)
        {
            return Vector2.ClampMagnitude(Vector3.SlerpUnclamped(Vector2.ClampMagnitude(SerialValue - DeltaValue,1),Vector2.ClampMagnitude(SerialValue,1), 1 + tTicksAhead),(Mathf.Clamp01 (Mathf.LerpUnclamped((SerialValue-DeltaValue).magnitude,SerialValue.magnitude,1+tTicksAhead))));
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
            Vector2 rawValue = (Vector2)this;
            DeltaValue = rawValue - SerialValue;
            SerialValue = rawValue;
            xAxisMap.SerializeValues();
            yAxisMap.SerializeValues();
        }
    }
}
#endif