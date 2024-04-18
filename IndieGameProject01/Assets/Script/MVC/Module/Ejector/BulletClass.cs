using System;
using System.Collections;
using UnityEngine;
using Timer = Script.MVC.Other.Timer2.Timer;

namespace Script.MVC.Module.Ejector
{
    public class BulletClass : MonoBehaviour
    {
        //public GameObject obj;
        public Gun parent;
        public BoxCollider2D boxCollider;
        public Rigidbody2D rigidbody;
        public float lifetime = 2;
        private Timer lifeTimer;
        //public BulletClass bulletClass;
        public Gun.Bullet Bullet;// = new(null,null);
        public bool released;//如果已经释放，则不执行后续释放操作
        private void Awake()
        {
            //obj = gameObject;
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
            //bulletClass = this;
            lifeTimer = Timer.Start(lifetime, (float _) => {}, ReleaseBullet, 0.01f);

        }

        void Start()
        {
            //released = false;
        }
        
        void OnEnable()
        {
            lifeTimer.ReStart();
        }

        void OnDisable()
        {
            lifeTimer.Pause();
        }
        void ReleaseBullet()
        {
            // 如果已经释放，则不执行后续释放操作
            //if (released)return;
            parent.bulletPool.Release(Bullet);
        }
        // void OnEnable()
        // {
        //     StartCoroutine(DestroyAfterTime(lifetime));
        // }
        // IEnumerator DestroyAfterTime(float time)
        // {
        //     yield return new WaitForSeconds(time);
        //     // 如果已经释放，则不执行后续释放操作
        //     if (released)
        //     {
        //         yield break; // 终止协程的执行
        //     }
        //     //released = true;
        //     parent.bulletPool.Release(Bullet);
        //     // Destroy(gameObject);  // 如果不使用对象池，则直接销毁对象
        // }
    }
}
