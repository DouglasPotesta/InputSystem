#if FLEXINPUT
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Input 
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
        }
        else if (buttonNum.Contains('-'))
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
#endif