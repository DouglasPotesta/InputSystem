using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// This class loads managers, settings, and other elements that last the lifetime of the application.
/// </summary>
public class GameInitializer : MonoBehaviour {
    private static GameInitializer singleton;
    internal static System.Action OnUpdate;
    [SerializeField]
    private List<UnityEngine.Object> Assets = new List<Object>();

    private static GameInitializer Singleton
    {
        get
        {
            if(singleton == null)
            {
                InitializeGame();
            }
            return singleton;
        }

        set
        {
            singleton = value;
        }
    }


#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void ConfigureEditorForInitialization()
    {
        UnityEditor.EditorBuildSettingsScene[] sceneSetups = UnityEditor.EditorBuildSettings.scenes;
        if (!sceneSetups.Any(x => x.path.Contains("GameInitializationScene.unity")))
        {
            string path = AssetDatabase.FindAssets("GameInitializationScene").Where(x => AssetDatabase.GUIDToAssetPath(x).Contains("/GameInitializationScene.unity")).First();
            path = AssetDatabase.GUIDToAssetPath(path);
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Concat(new EditorBuildSettingsScene[] { new EditorBuildSettingsScene(path, true) }).ToArray();
            Debug.Log("Added GameInitializationScene scene to build settings.");
        }
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGame()
    {
        if (singleton == null)
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameInitializationScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            asyncOperation.completed += (AsyncOperation op) =>
            {
                Singleton = FindObjectOfType<GameInitializer>();
                if (singleton == null)
                {
                    Debug.LogWarning("Failed to initialize properly.");
                }
                else
                {
                    Singleton.gameObject.hideFlags = HideFlags.HideAndDontSave;
                    RunOnGameInitializedAttribute.CallAllMethods();
                }
            };
        }
    }

    void Start () {
        DontDestroyOnLoad(this);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("GameInitializationScene");
    }
	
	void Update () {
		if(OnUpdate != null)
        {
            OnUpdate();
        }
	}

    /// <summary>
    /// Get first Initialization Asset of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetAsset<T>() where T : UnityEngine.Object
    {
        return (T)Singleton.Assets.FirstOrDefault(x => x is T);
    }
    /// <summary>
    /// Get all Initialization Assets of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] GetAllAssets<T>() where T : UnityEngine.Object
    {
        return Singleton.Assets.Where(x => x is T).Select(x => (T)x).ToArray();
    }
    /// <summary>
    /// Get Initialization Asset with type typeName
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetAsset(string typeName)
    {
        System.Type type = System.Type.GetType(typeName);
        return Singleton.Assets.FirstOrDefault(x => x.GetType() == type);
    }
    /// <summary>
    /// Get all Initialization Assets with type typeName
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static UnityEngine.Object[] GetAllAssets(string typeName)
    {
        System.Type type = System.Type.GetType(typeName);
        return Singleton.Assets.Where(x => x.GetType() == type).ToArray();
    }
    /// <summary>
    /// Get first Initialization Assets of specified type with specified name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="AssetName"></param>
    /// <returns></returns>
    public static T GetAssetByName<T>(string AssetName) where T : UnityEngine.Object
    {
        return (T)GetAllAssets<T>().FirstOrDefault(x => x.name == AssetName);
    }
    /// <summary>
    /// Get first Initialization Assets of specified type with specified name.
    /// </summary>
    /// <param name="AssetName"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetAssetByName(string AssetName, string typeName)
    {
        return GetAllAssets(typeName).FirstOrDefault(x => x.name == AssetName);
    }

    /// <summary>
    /// Get first Initialization Assets with specified name.
    /// </summary>
    /// <param name="AssetName"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetAssetByName(string AssetName)
    {
        return Singleton.Assets.FirstOrDefault(x => x.name == AssetName);
    }
}

/// <summary>
/// Used to call a paramterless static void method after Game Initialization is completed. 
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RunOnGameInitializedAttribute : System.Attribute
{
    public int order = 0;

    public RunOnGameInitializedAttribute()
    {
        order = 0;
    }
    public RunOnGameInitializedAttribute(int _order)
    {
        order = _order;
    }
    public static MethodInfo[] CallAllMethods()
    {
        System.Type type = typeof(RunOnGameInitializedAttribute);
        Assembly assemblies = Assembly.GetCallingAssembly();
        List<MethodInfo> methodInfos = new List<MethodInfo>();
        System.Type[] types = assemblies.GetTypes();
        methodInfos.AddRange(types.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(m =>m.GetParameters().Length==0 && m.GetCustomAttributes(type, false).Length > 0)));
        methodInfos.OrderBy(item => ((RunOnGameInitializedAttribute)item.GetCustomAttributes(type, true).First()).order);

        for (int ii = 0; ii < methodInfos.Count; ii++)
        {
            ParameterInfo[] paramaters = methodInfos[ii].GetParameters();
            if (paramaters.Length == 0 && methodInfos[ii].IsStatic)
            {
                methodInfos[ii].Invoke(null, new object[] { });
            }
        }

        return methodInfos.ToArray();
    }
}