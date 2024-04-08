using UnityEngine;

namespace Script.MVC.Module.Frame.ObjectPool
{
    public class PoolTest : MonoBehaviour
    {
        private ObjectPool<GameObject> pool;
        public GameObject obj;
        void Start()
        {
            pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestory,
                true, 10, 1000);
        }
        GameObject OnCreate()
        {
            return new GameObject("Create");
        }
        void OnGet(GameObject gameObj)
        {
            gameObj.SetActive(true);
            Debug.Log("OnGet");
        }
        void OnRelease(GameObject gameObj)
        {
            gameObj.SetActive(false);
            Debug.Log("OnRelease");
        }
        void OnDestory(GameObject gameObj)
        {
            gameObj.SetActive(false);
            Debug.Log("OnDestroy");
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                pool.Get();
            }

            if (Input.GetMouseButtonDown(1))
            {
                pool.Release(obj);
            }
        }
    }
}
