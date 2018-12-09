#if FLEXINPUT
using System.Collections.Generic;
using UnityEngine;

namespace Potesta.FlexInput
{

    public class JoyPlatformMaps
    {
        public static readonly Dictionary<string, string> GenericButtons = new Dictionary<string, string>
        {
            {"0","0" },
            {"1","1"},
            {"2","2" },
            {"3","3"},
            {"4","4" },
            {"5","5" },
            {"6","6" },
            {"7","7"},
            {"8","8" },
            {"9","9"},
            {"10","10" },
            {"11","11"},
            {"12","12" },
            {"13","13" },
            {"14","14" },
            {"15","15"},
            {"16","16" },
            {"17","17"},
            {"18","18" },
            {"19","19"},
        };

        public static readonly Dictionary<string, string> XboxWindowsAxes = new Dictionary<string, string>
        {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
            {"6","D-Pad ↓↑" },
            {"5","D-Pad ←→" },
            {"9","Right Trigger →" },
            {"8","Left Trigger →" },
            {"10","Right Trigger →" },
        };

        public static readonly Dictionary<string, string> XboxMacAxes = new Dictionary<string, string>
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ↓↑" },
            {"4","Right Stick ←→" },
            {"5","Left Trigger →" },
            {"6","Right Trigger →" },
    };

        public static readonly Dictionary<string, string> XboxLinuxAxes = new Dictionary<string, string>()
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"4","Right Stick ↓↑" },
            {"5","Right Stick ←→" },
            {"7","D-Pad ↓↑" },
            {"8","D-Pad ←→" },
            {"3","Left Trigger →" },
            {"6","Right Trigger →" },
    };

        public static readonly Dictionary<string, string> GenericAxes = new Dictionary<string, string>()
    {
            {"0","0" },
            {"1","1"},
            {"2","2" },
            {"3","3"},
            {"4","4" },
            {"5","5" },
            {"6","6" },
            {"7","7"},
            {"8","8" },
            {"9","9"},
            {"10","10" },
            {"11","11"},
            {"12","12" },
            {"13","13" },
            {"14","14" },
            {"15","15"},
            {"16","16" },
            {"17","17"},
            {"18","18" },
            {"19","19"},
            {"20","20" },
            {"21","21" },
            {"22","22" },
            {"23","23"},
            {"24","24" },
            {"25","25"},
            {"26","26" },
            {"27","27"},
    };

        public static readonly Dictionary<string, string> WebGLAxes = 
            new Dictionary<string, string>()
            {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
            };

        public static readonly Dictionary<string, string> XboxWindowsButtons =
            new Dictionary<string, string>
            {
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Back Button" },
            {"7","Start Button" },
            {"8","Left Stick Click" },
            {"9","Right Stick Click" }
            };
        public static readonly Dictionary<string, string> XboxMacButtons =
        new Dictionary<string, string>
        {
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
        };
        public static readonly Dictionary<string, string> XboxLinuxButtons =
        new Dictionary<string, string>
        {
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
            };

        public static readonly Dictionary<string, string> XboxWebGLButtons =
    new Dictionary<string, string>
    {
            {"0","A" },
            {"1","B" },
            {"2","X" },
            {"3","Y" },
            {"4","Left Bumper" },
            {"5","Right Bumper" },
            {"6","Left Trigger" },
            {"7","Right Trigger" },
            {"8","Back" },
            {"9","Start" },
            {"10","Left Stick Click" },
            {"11","Right Stick Click" },
            {"12","D-Pad Up" },
            {"13","D-Pad Down" },
            {"14","D-Pad Left" },
            {"15","D-Pad Right" }
        };

        public static readonly Dictionary<string, string> PS4WinButtons =
new Dictionary<string, string>
{
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
};
        public static readonly Dictionary<string, string> PS4MacButtons =
new Dictionary<string, string>
{
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
};
        public static readonly Dictionary<string, string> PS4LinuxButtons =
new Dictionary<string, string>
{
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
};
        public static readonly Dictionary<string, string> PS4WindowsAxes = new Dictionary<string, string>
        {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ←→" },
            {"4","Right Stick ↓↑" },
            {"6","D-Pad ↓↑)" },
            {"5","D-Pad ←→" },
            {"9","Right Trigger →" },
            {"8","Left Trigger →" },
            {"10","Right Trigger →" },
        };

        public static readonly Dictionary<string, string> PS4MacAxes = new Dictionary<string, string>
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"3","Right Stick ↓↑" },
            {"4","Right Stick ←→" },
            {"5","Left Trigger →" },
            {"6","Right Trigger →" },
    };

        public static readonly Dictionary<string, string> PS4LinuxAxes = new Dictionary<string, string>()
    {
            {"0","Left Stick ←→" },
            {"1","Left Stick ↓↑" },
            {"4","Right Stick ↓↑" },
            {"5","Right Stick ←→" },
            {"7","D-Pad ↓↑" },
            {"8","D-Pad ←→" },
            {"3","Left Trigger →" },
            {"6","Right Trigger →" },
    };

        public static Dictionary<string, string> GetButtonsForPlatform(RuntimePlatform runtimePlatform)
        {
            Dictionary<string, string> buttonMapStrings;
            switch (runtimePlatform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    buttonMapStrings = JoyPlatformMaps.XboxMacButtons;
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    buttonMapStrings = JoyPlatformMaps.XboxWindowsButtons;
                    break;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    buttonMapStrings = JoyPlatformMaps.XboxLinuxButtons;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    buttonMapStrings = JoyPlatformMaps.XboxWebGLButtons;
                    break;
                case RuntimePlatform.PS4:
                    // TODO add custom PS4 Support
                    buttonMapStrings = JoyPlatformMaps.GenericButtons;
                    break;
                case RuntimePlatform.XboxOne:
                    // TODO add custom Xbox Support
                    buttonMapStrings = JoyPlatformMaps.GenericButtons;
                    break;
                case RuntimePlatform.Switch:
                    // TODO add custom Switch Support
                    buttonMapStrings = JoyPlatformMaps.GenericButtons;
                    break;
                default:
                    buttonMapStrings = JoyPlatformMaps.GenericButtons;
                    break;
            }
            return buttonMapStrings;
        }
        public static Dictionary<string, string> GetAxisForPlatform(RuntimePlatform runtimePlatform)
        {
            Dictionary<string, string> axisMapStrings;
            switch (runtimePlatform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    axisMapStrings = JoyPlatformMaps.XboxMacAxes;
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    axisMapStrings = JoyPlatformMaps.XboxWindowsAxes;
                    break;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    axisMapStrings = JoyPlatformMaps.XboxLinuxAxes;
                    break;
                case RuntimePlatform.WebGLPlayer:
                    axisMapStrings = JoyPlatformMaps.GenericAxes;
                    break;
                case RuntimePlatform.PS4:
                    // TODO add custom PS4 Support
                    axisMapStrings = JoyPlatformMaps.GenericAxes;
                    break;
                case RuntimePlatform.XboxOne:
                    // TODO add custom Xbox Support
                    axisMapStrings = JoyPlatformMaps.GenericAxes;
                    break;
                case RuntimePlatform.Switch:
                    // TODO add custom Switch Support
                    axisMapStrings = JoyPlatformMaps.GenericAxes;
                    break;
                default:
                    axisMapStrings = JoyPlatformMaps.GenericAxes;
                    break;
            }
            return axisMapStrings;
        }
        private static class FilterType { public const string Unfiltered = "Unfiltered", Xbox_One = "Xbox_One", DualShock = "DualShock"; }
        public static string[] GetPlatformFilters(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.OSXEditor:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.OSXPlayer:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.WindowsPlayer:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.WindowsEditor:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.LinuxPlayer:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.LinuxEditor:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.WebGLPlayer:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One, FilterType.DualShock };
                case RuntimePlatform.PSP2:
                    return new string[] { FilterType.Unfiltered, FilterType.DualShock };
                case RuntimePlatform.PS4:
                    return new string[] { FilterType.Unfiltered, FilterType.DualShock };
                case RuntimePlatform.PSM:
                    return new string[] { FilterType.Unfiltered, FilterType.DualShock };
                case RuntimePlatform.XboxOne:
                    return new string[] { FilterType.Unfiltered, FilterType.Xbox_One };
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                case RuntimePlatform.TizenPlayer:
                case RuntimePlatform.WiiU:
                case RuntimePlatform.tvOS:
                case RuntimePlatform.Switch:
                default:
                    return new string[] { FilterType.Unfiltered };
            }
        }
    }
}
#endif