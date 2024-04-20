using Script.MVC.Module.Class;
using Script.MVC.Module.Collision;
using UnityEngine;
using Timer = Script.MVC.Other.Timer2.Timer;

namespace Script.MVC.Module.Ejector
{
    public class BulletClass : MonoBehaviour
    {
        //public GameObject obj;
        public Gun parent;
        public Biota owner;
        public BoxCollider2D boxCollider;
        public new Rigidbody2D rigidbody;
        public CollisionTrigger collisionTrigger;
        public float lifetime = 2;
        private Timer lifeTimer;
        //public BulletClass bulletClass;
        public Gun.Bullet Bullet;// = new(null,null);
        public bool released;//如果已经释放，则不执行后续释放操作
        public bool lockOrient;
        private void Awake()
        {
            //obj = gameObject;
            if(!boxCollider) boxCollider = gameObject.GetComponent<BoxCollider2D>();
            if(!rigidbody) rigidbody = gameObject.GetComponent<Rigidbody2D>();
            if(!collisionTrigger) collisionTrigger = gameObject.GetComponent<CollisionTrigger>();
            
        }

        // void Start()
        // {
        // }

        private void Update()
        {
            if (lockOrient)
            {
                // 获取盒子的速度向量
                Vector3 velocity = rigidbody.velocity;
                // 如果速度向量的长度大于最小速度阈值，则认为盒子正在移动
                if (velocity.magnitude > 0.1f)
                {
                    // 将盒子的朝向设置为速度向量的方向
                    gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward,velocity);
                }
            }
        }

        private void TimerStart_LifeTimer()
        {
            if (lifeTimer == null)
            {
                lifeTimer = Timer.Start(lifetime, (_) => {}, ReleaseBullet, 0.01f);
            }
            else
            {
                lifeTimer.ReStart();
            }
        }

        void OnEnable()
        {
            TimerStart_LifeTimer();
        }

        void OnDisable()
        {
            lifeTimer?.Pause();
        }
        void ReleaseBullet()
        {
            // 如果已经释放，则不执行后续释放操作
            //if (released)return;
            parent.BulletPool.Release(Bullet);
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
