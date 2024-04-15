using System;
using UnityEngine;

namespace Script.MVC.Other.Timer2
{
    [AutoSingleton(true)]
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        public static T Instance
        {
            get
            {
                Type _type = typeof(T);
                if (_destroyed)
                {
                    Debug.LogWarningFormat("[Singleton]??{0}???????????????? Null??", _type.Name);
                    return (T)((object)null);
                }
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(_type);
                        if (FindObjectsOfType(_type).Length > 1)
                        {
                            Debug.LogErrorFormat("[Singleton]?????{0}???????????.", _type.Name);
                            return _instance;
                        }
                        if (_instance == null)
                        {
                            object[] customAttributes = _type.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                            AutoSingletonAttribute autoAttribute = (customAttributes.Length > 0) ? (AutoSingletonAttribute)customAttributes[0] : null;
                            if (null == autoAttribute || !autoAttribute.autoCreate)
                            {
                                Debug.LogWarningFormat("[Singleton]???????????{0}????????????????????????~", _type.Name);
                                return (T)((object)null);
                            }
                            GameObject go = null;
                            if (string.IsNullOrEmpty(autoAttribute.resPath))
                            {
                                go = new GameObject(_type.Name);
                                _instance = go.AddComponent<T>();
                            }
                            else
                            {
                                go = Resources.Load<GameObject>(autoAttribute.resPath);
                                if (null != go)
                                {
                                    go = GameObject.Instantiate(go);
                                }
                                else
                                {
                                    Debug.LogErrorFormat("[Singleton]?????{0}??ResPath??????????¡¤????{1}??", _type.Name, autoAttribute.resPath);
                                    return (T)((object)null);
                                }
                                _instance = go.GetComponent<T>();
                                if (null == _instance)
                                {
                                    Debug.LogErrorFormat("[Singleton]????????¦Ä?????????{0}????ResPath??{1}??", _type.Name, autoAttribute.resPath);
                                }
                            }
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Debug.Log("????????????‰Ç");
                if (Application.isPlaying)
                {
                    GameObject.Destroy(gameObject);
                }
                else
                {
                    GameObject.DestroyImmediate(gameObject);
                }
            }
            else
            {
                _instance = GetComponent<T>();
                if (!transform.parent) //Unity ?????????????? ???????????????
                {
                    DontDestroyOnLoad(gameObject);
                }
                OnInit();
            }
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                GameObject.Destroy(_instance.gameObject);
            }
            _destroyed = true;
            _instance = (T)((object)null);
        }

        /// <summary>
        /// ??? _destroyed ??
        /// </summary>
        public static void ClearDestroy()
        {
            DestroyInstance();
            _destroyed = false;
        }

        private static bool _destroyed = false;
        /// <summary>
        /// ???????????Unity ?????????????????
        /// ?????? gameObject ???????????????????????????????¦Á????????????
        /// ????????????????????????????????????? gameObject ?????????????????§³?
        /// ???????????§Þ??????????????????????
        /// </summary>
        public void OnDestroy()
        {
            if (_instance != null && _instance.gameObject == base.gameObject)
            {
                _instance = (T)((object)null);
                _destroyed = true;
            }
        }

        /// <summary>Awake ??????????? </summary>
        public virtual void OnInit()
        {
            Debug.Log("OnInit");
        }
        
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AutoSingletonAttribute : Attribute
    {
        public bool autoCreate; //??????????????
        public string resPath;  //????????????¡¤?????????

        public AutoSingletonAttribute(bool _autoCreate, string _resPath = "")
        {
            this.autoCreate = _autoCreate;
            this.resPath = _resPath;
        }
    }
}