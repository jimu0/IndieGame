using System.Collections.Generic;
using UnityEngine;

namespace Editor.AssetBundle
{
    public class QAssetBundleManager
    {
        static UnityEngine.AssetBundle assetbundle = null;

        static Dictionary<string, UnityEngine.AssetBundle> DicAssetBundle = new Dictionary<string, UnityEngine.AssetBundle>(); 

        public static T LoadResource<T>(string assetBundleName, string assetBundleGroupName) where T : Object
        {
            if (string.IsNullOrEmpty(assetBundleGroupName))
            {
                return default(T);
            }

            if (!DicAssetBundle.TryGetValue(assetBundleGroupName, out assetbundle))
            {
                assetbundle = UnityEngine.AssetBundle.LoadFromFile(GetStreamingAssetsPath() + assetBundleGroupName);//+ ".assetbundle"
                DicAssetBundle.Add(assetBundleGroupName, assetbundle);
            }
            object obj = assetbundle.LoadAsset(assetBundleName, typeof(T));
            var one = obj as T; 
            return one;
        }

        /// <summary>
        /// 卸载(只卸载内存镜像)
        /// </summary>
        /// <param name="assetBundleGroupName"></param>
        public static void UnLoadResource(string assetBundleGroupName)
        {
            if (DicAssetBundle.TryGetValue(assetBundleGroupName, out assetbundle))
            {
                assetbundle.Unload(false);
                if (assetbundle != null)
                {
                    assetbundle = null;
                }
                DicAssetBundle.Remove(assetBundleGroupName);
                Resources.UnloadUnusedAssets();
            }
        }
        /// <summary>
        /// 卸载(可选择卸载方式)
        /// </summary>
        /// <param name="assetBundleGroupName"></param>
        /// <param name="b">只卸载内存镜像(false)或卸载内存镜像以及Asset的内存实例(true)</param>
        public static void UnLoadResource(string assetBundleGroupName,bool b=false)
        {
            if (DicAssetBundle.TryGetValue(assetBundleGroupName, out assetbundle))
            {
                assetbundle.Unload(b);
                if (assetbundle != null)
                {
                    assetbundle = null;
                }
                DicAssetBundle.Remove(assetBundleGroupName);
                Resources.UnloadUnusedAssets();
            }
        }

        public static string GetStreamingAssetsPath()
        {
            string StreamingAssetsPath =
#if UNITY_EDITOR
                Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID
            "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
            Application.dataPath + "/Raw/";
#else
            string.Empty;
#endif
            return StreamingAssetsPath;
        }
    
    }
}