using UnityEngine;

namespace Script.MVC.Module.Frame.ObjectPool
{
    public class PoolTest : MonoBehaviour
    {
        private ObjectPool<GameObject> pool;
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
            Debug.Log("OnGet");
        }
        void OnRelease(GameObject gameObj)
        {
            Debug.Log("OnRelease");
        }
        void OnDestory(GameObject gameObj)
        {
            Debug.Log("OnDestroy");
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                pool.Get();
            }
        }
    }
}
