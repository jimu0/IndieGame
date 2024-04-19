using Script.MVC.Module.Class;
using Script.MVC.Module.Frame.ObjectPool;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Ejector
{
    public class Gun : MonoBehaviour
    {
        public Biota owner;
        [SerializeField] private GameObject bulletPrefab;//子弹
        private Vector2 posGunStart;//子弹目标点
        private Vector2 posGunEnd;
        private Vector2 gunAim = Vector2.right;
        [SerializeField] private bool loop;//循环
        private bool notLoopSingleUse = true;//循环时的单次出发许可

        [SerializeField] private bool pierce = true;//可穿透
        [SerializeField] private bool gravitation;//受引力影响
        [SerializeField] private bool lockOrient;//始终锁定朝向到移动方向
        [SerializeField] private float fireForce = 20f;//发射时的施加力

        public ObjectPool<Bullet> bulletPool;
        
        void Start()
        {
            bulletPool = new ObjectPool<Bullet>(OnCreate, OnGet, OnRelease, OnDestory, true, 10, 40);
            
        }

        void Update()
        {
            //Fire();

        }

        public void SetGunPos(Vector2 posStart,Vector2 posEnd)
        {
            posGunStart = posStart;
            posGunEnd = posEnd;
        }

        Bullet OnCreate()
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            BulletClass bulletClass = bulletObj.GetComponent<BulletClass>();
            bulletClass.parent = this;
            bulletClass.owner = owner;
            bulletClass.collisionTrigger.owner = owner;
            Bullet bullet = new(bulletObj, bulletClass);
            bullet.bulletClass.Bullet = bullet;

            return bullet;
        }
        void OnGet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }
        void OnRelease(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
        void OnDestory(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        
        
        void GetBullet()
        {
            Bullet bullet = bulletPool.Get();
            bullet.bulletClass.owner = owner;
            bullet.bulletClass.collisionTrigger.owner = owner;
            bullet.gameObject.transform.position = posGunStart;
            gunAim = posGunEnd - posGunStart;
            Quaternion rotation =  Quaternion.LookRotation(Vector3.forward,gunAim);
            bullet.gameObject.transform.rotation = rotation;
            Vector3 bulletScale = bullet.gameObject.transform.localScale;
            bulletScale.x *= gunAim.x / Mathf.Abs(gunAim.x);
            bullet.gameObject.transform.localScale = bulletScale;
            bullet.bulletClass.boxCollider.size = Vector2.one;
            bullet.bulletClass.boxCollider.isTrigger = pierce;
            bullet.bulletClass.rigidbody.gravityScale = gravitation ? 1 : 0;
            
            bullet.bulletClass.rigidbody.AddForce(gunAim* fireForce, ForceMode2D.Impulse);
            bullet.bulletClass.lockOrient = lockOrient;
        }

        public void Fire()
        {
            //if (fireTimer.currentTimerState == Timer.TimerState.Timing)return;
            GetBullet();
            //fireTimer.ReStart();
        }

        public void Fire_Auto()
        {
            //if (!auto || fireTimer.currentTimerState == Timer.TimerState.Timing) return;
            if (!loop)
            {
                if (!notLoopSingleUse) return;
                //fireTimer.ReStart();
                notLoopSingleUse = false;
                return;
            }
            //fireTimer.ReStart();
        }
        
        public class Bullet
        {
            public GameObject gameObject;
            public BulletClass bulletClass;

            public Bullet(GameObject obj, BulletClass c)
            {
                this.gameObject = obj;
                this.bulletClass = c;
            }
        }
        
        
    }
}
