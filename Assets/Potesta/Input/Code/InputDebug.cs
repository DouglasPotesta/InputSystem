
#if FLEXINPUT
#if CONSOLE
using System;
using System.Linq;
using Potesta.Console;

public class InputDebug {
    [ConsoleComand]
    public static string GetJoyStringList()
    {
        return String.Join("\n", Input.GetJoystickNames());
    }

    [ConsoleComand]
    public static string GetJoyStringList(int integer, string str, float flo)
    {
        return String.Join("\n", Input.GetJoystickNames());
    }
}
#endif
#endif