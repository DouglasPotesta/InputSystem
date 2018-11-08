using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Potesta
{
    /// <summary>
    /// This class loads managers, settings, and other elements that last the lifetime of the application.
    /// </summary>
    public class GameInitializer : MonoBehaviour
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
    {
        public const string SCENE_NAME = "GameInitializationScene";
        protected static GameInitializer singleton;
        internal static System.Action OnUpdate;
        internal static System.Action OnFixedUpdate;
        internal static System.Action OnGUIUpdate;
        [SerializeField]
        protected List<UnityEngine.Object> Assets = new List<Object>();

        protected static List<UnityEngine.Object> assets (){ return Singleton.Assets; }
#pragma warning disable 414
        [SerializeField]
        private List<SceneReference> allScenes;
#pragma warning restore 414

        private static GameInitializer Singleton
        {
            get
            {
                if (singleton == null)
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




        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void InitializeGame()
        {
            if (singleton == null)
            {
                AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SCENE_NAME, UnityEngine.SceneManagement.LoadSceneMode.Additive);
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

        void Start()
        {
            DontDestroyOnLoad(this);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(SCENE_NAME);
        }

        void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }
        }
        void FixedUpdate()
        {
            if(OnFixedUpdate != null)
            {
                OnFixedUpdate();
            }
        }
        void OnGUI()
        {
            if (OnGUIUpdate != null)
            {
                OnGUIUpdate();
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
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            allScenes = EditorBuildSettings.scenes.Where(x => x.enabled).Select(x => new SceneReference() { sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(x.path) }).ToList();
        }

        public void OnAfterDeserialize()
        {

        }

        public void AddAsset(Object asset)
        {
            if (!Assets.Contains(asset))
            {
                Assets.Add(asset);
            }
        }
        [RunOnGameInitialized]
        public static void GrabAllAssets()
        {
            if (Application.isPlaying)
            {
                System.Type type = typeof(IncludeInStartUpAttribute);
                List<System.Type> types = new List<System.Type>();
                types.AddRange(Assembly.GetAssembly(typeof(GameInitializer)).GetTypes()
                  .Where(m => m.GetCustomAttributes(type, false).Length > 0));
                for (int i = 0; i < types.Count; i++)
                {
                    assets().AddRange(AssetDatabase.FindAssets("t:" + types[i].Name).Select(x => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(x), types[i])).Where(x => x != null && !GameInitializer.assets().Contains(x)).ToArray());
                }
            }
        }
#endif
    }

}