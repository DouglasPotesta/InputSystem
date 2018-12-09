#if CONSOLE
using System.Linq;
using UnityEngine;
namespace Potesta.Console
{
    public partial class Console : MonoBehaviour
    {
        static Console singleton;
        bool displayConsole = false;
        string consoleLog = "";
        Texture2D grayLine;
        Color backgroundColor = new Color(0, 0, 0, 0.75f);
        string userInput = "";
        GUILayoutOption width = GUILayout.Width(Screen.width * 0.99f);
        Vector2 scrollPos = new Vector2();
        GUIStyle style;
        /// <summary>
        /// Initializes the gui aspect of the system.
        /// </summary>
        private void InitGUI()
        {
            singleton.grayLine = new Texture2D(1, 1);
            singleton.grayLine.SetPixel(0, 0, Color.gray);
        }
        /// <summary>
        /// A simple implementation of opening and closing the console.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown("`"))
            {
                ToggleConsole();
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                displayConsole = false;
            }
        }
        /// <summary>
        /// A public way to toggle the console.
        /// </summary>
        public static void ToggleConsole()
        {
            singleton.displayConsole = !singleton.displayConsole;
        }
        /// <summary>
        /// The core GUI loop.
        /// </summary>
        private void OnGUI()
        {
            if (!displayConsole)
            {
                return;
            }
            Color originalColor = SetStyle();
            ConsoleHistoryGUI();
            PlayerInputGUI();
            ContextGUI();
            GUI.backgroundColor = originalColor;
            ProcessInput();
        }
        /// <summary>
        /// Sets the style for the Console. Then returns the original background color.
        /// </summary>
        /// <returns></returns>
        private Color SetStyle()
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.textArea);
                style.alignment = TextAnchor.LowerLeft;
            }
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = backgroundColor;
            return originalColor;
        }
        /// <summary>
        /// The GUI layout of the console history.
        /// </summary>
        private void ConsoleHistoryGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height / 2));
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.TextArea(consoleLog, style, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        /// <summary>
        /// The method for clearing gui input and calling the console commands.
        /// </summary>
        private void ProcessInput()
        {
            if (userInput.Contains("\n"))
            {
                string inputCopy = userInput;
                ClearUserInput();
                string message = "\n" + RunCommand(inputCopy);
                if (message != null)
                {
                    consoleLog += message;
                }
                Debug.Log(message);
                scrollPos = Vector2.up * Screen.height;
            }
            if (userInput.Contains("`"))
            {
                userInput = "";
                ToggleConsole();
            }
        }
        /// <summary>
        /// creates the typeable input.
        /// </summary>
        private void PlayerInputGUI()
        {
            GUILayout.BeginArea(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2));
            GUILayout.Box(grayLine, width, GUILayout.Height(1));
            GUI.SetNextControlName("User Input");
            userInput = GUILayout.TextArea(userInput, width);
            GUI.FocusControl("User Input");
            GUILayout.EndArea();
        }
        /// <summary>
        /// The context window that popups and provides console command context.
        /// </summary>
        private void ContextGUI()
        {
            if (userInput.Length > 0)
            {
                Rect contextRect = new Rect(Vector2.up * Screen.height / 2 + style.lineHeight * Vector2.up * 1.4f, Vector2.one * 300);
                string searchContext = new string(methodDictionary.Keys.Where(x => x.StartsWith(userInput)).Select(x => x.Substring(0, x.Length - 1) + "\n").Distinct().SelectMany(x => x).ToArray());
                GUI.TextArea(contextRect, searchContext);
            }
        }
        /// <summary>
        /// simple method for clearing the user input.
        /// </summary>
        private void ClearUserInput()
        {
            userInput = userInput.TrimEnd('\r', '\n');
            consoleLog += "\n\n" + userInput;
            userInput = "";
        }
    }
}
#endif