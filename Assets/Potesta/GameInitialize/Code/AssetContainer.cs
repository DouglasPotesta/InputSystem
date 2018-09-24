using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Potesta
{
    public class AssetContainer : MonoBehaviour
    {

        public List<Object> Assets;


        [HideInInspector] public string scenePath;


        public SceneReference scene;

        /// <summary>
        /// Get first Initialization Asset of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            return (T)Assets.FirstOrDefault(x => x is T);
        }
        /// <summary>
        /// Get all Initialization Assets of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetAllAssets<T>() where T : UnityEngine.Object
        {
            return Assets.Where(x => x is T).Select(x => (T)x).ToArray();
        }
        /// <summary>
        /// Get Initialization Asset with type typeName
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public UnityEngine.Object GetAsset(string typeName)
        {
            System.Type type = System.Type.GetType(typeName);
            return Assets.FirstOrDefault(x => x.GetType() == type);
        }
        /// <summary>
        /// Get all Initialization Assets with type typeName
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public UnityEngine.Object[] GetAllAssets(string typeName)
        {
            System.Type type = System.Type.GetType(typeName);
            return Assets.Where(x => x.GetType() == type).ToArray();
        }
        /// <summary>
        /// Get first Initialization Assets of specified type with specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public T GetAssetByName<T>(string AssetName) where T : UnityEngine.Object
        {
            return (T)GetAllAssets<T>().FirstOrDefault(x => x.name == AssetName);
        }
        /// <summary>
        /// Get first Initialization Assets of specified type with specified name.
        /// </summary>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public UnityEngine.Object GetAssetByName(string AssetName, string typeName)
        {
            return GetAllAssets(typeName).FirstOrDefault(x => x.name == AssetName);
        }

        /// <summary>
        /// Get first Initialization Assets with specified name.
        /// </summary>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public UnityEngine.Object GetAssetByName(string AssetName)
        {
            return Assets.FirstOrDefault(x => x.name == AssetName);
        }
    }
}