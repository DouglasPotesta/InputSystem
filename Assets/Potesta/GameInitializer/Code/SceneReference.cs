using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Potesta
{
    [System.Serializable]
    public struct SceneReference
#if UNITY_EDITOR
    : ISerializationCallbackReceiver
#endif
    {
#if UNITY_EDITOR
        public SceneAsset sceneAsset;
#endif

        public string Name
        {
            get
            {
#if UNITY_EDITOR
                if (sceneAsset == null) { return ""; }
                return sceneAsset.name;
#else
            return name;
#endif
            }
        }

#pragma warning disable 414
        [HideInInspector]
        [SerializeField]
        private string name;
#pragma warning restore 414

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (sceneAsset == null) { name = ""; }
            else { name = sceneAsset.name; }
        }

        public void OnAfterDeserialize()
        {
        }
#endif
    }
}
