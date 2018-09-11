using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameInitializer : MonoBehaviour {
    private static GameInitializer singleton;
    internal static System.Action OnUpdate;
    internal static System.Action OnInitialized;

    public List<UnityEngine.Object> Assets = new List<Object>();

    public static GameInitializer Singleton
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
    static void ConfigureEditorForInitialization()
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
                    if(OnInitialized != null)
                    {
                        OnInitialized();
                    }
                    RunOnGameInitializedAttribute.CallAllMethods();
                }
            };
        }
    }

    public static T GetAsset<T>() where T : UnityEngine.Object
    {
        return (T) Singleton.Assets.FirstOrDefault(x => x is T);
    }

    public static T[] GetAssets<T> ()where T : UnityEngine.Object
    {
        return  Singleton.Assets.Where(x => x is T).Select(x=>(T)x).ToArray();
    }

    public static UnityEngine.Object GetAsset(string typeName)
    {
        System.Type type = System.Type.GetType(typeName);
        return Singleton.Assets.FirstOrDefault(x => x.GetType() == type);
    }

    public static UnityEngine.Object[] GetAssets(string typeName)
    {
        System.Type type = System.Type.GetType(typeName);
        return Singleton.Assets.Where(x => x.GetType() == type).ToArray();
    }

    public static T GetAssetByName<T>(string AssetName) where T : UnityEngine.Object
    {
        return (T)Singleton.Assets.FirstOrDefault(x => (x is T) && x.name == AssetName);
    }

    public static UnityEngine.Object GetAssetByName(string AssetName)
    {
        return Singleton.Assets.FirstOrDefault(x => x.name == AssetName);
    }

    void Start () {
        DontDestroyOnLoad(this);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("GameInitializationScene");
    }
	
	// Update is called once per frame
	void Update () {
		if(OnUpdate != null)
        {
            OnUpdate();
        }
	}
}

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
        methodInfos.AddRange(types.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(m => m.GetCustomAttributes(type, false).Length > 0)));
        methodInfos.OrderBy(item => ((RunOnGameInitializedAttribute)item.GetCustomAttributes(type, true).First()).order);

        for (int ii = 0; ii < methodInfos.Count; ii++)
        {
            ParameterInfo[] paramaters = methodInfos[ii].GetParameters();
            if (paramaters.Length == 0 && methodInfos[ii].IsStatic)
            {
                methodInfos[ii].Invoke(null, new object[] { });
            }
            else
            {
                if (paramaters.Length != 0|| !methodInfos[ii].IsStatic)
                {
                    Debug.LogError(methodInfos[ii] + " does not follow the proper signature. Needs to be static void method()");
                }
            }
        }

        return methodInfos.ToArray();
    }
}