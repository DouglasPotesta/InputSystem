
namespace Potesta.FlexInput
{
    [System.Serializable]
    public class BasicInput : InputController
    {
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
    }
}