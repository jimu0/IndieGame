using Script.MVC.Module.Frame.ObjectPool;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Ejector
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;//子弹
        [SerializeField] private Transform muzzle;//投射器挂点
        [SerializeField] private bool auto;//自动触发
        [SerializeField] private bool loop;//循环
        private bool notLoopSingleUse = true;//循环时的单次出发许可

        [SerializeField] private bool pierce = true;//可穿透
        [SerializeField] private bool gravitation;//受引力影响
        [SerializeField] private float fireDeviationAngle;//发射角度偏移
        [SerializeField] private float fireForce = 20f;//发射时的施加力
        [SerializeField] private float fireRate = 2.0f; // 发射频率
        [SerializeField] private float nextFireTime; // 下一次发射时间
        private Timer fireTimer;

        public ObjectPool<Bullet> bulletPool;
        
        // Start is called before the first frame update
        void Start()
        {
            bulletPool = new ObjectPool<Bullet>(OnCreate, OnGet, OnRelease, OnDestory, true, 10, 40);
            
            fireTimer = Timer.Start(fireRate, (float timeUpdata) => {}, () => { GetBullet();}, 0.01f);// Debug.Log("架势：抬高"); 
        }

        // Update is called once per frame
        void Update()
        {
            //Fire();

        }
        
                
        Bullet OnCreate()
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            BulletClass bulletClass = bulletObj.GetComponent<BulletClass>();
            bulletClass.parent = this;
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
            bullet.gameObject.transform.position = muzzle.position;
            bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, muzzle.eulerAngles.z + fireDeviationAngle);
            bullet.bulletClass.boxCollider.size = Vector2.one;
            bullet.bulletClass.rigidbody.gravityScale = gravitation ? 1 : 0;
            bullet.bulletClass.rigidbody.AddForce(bullet.gameObject.transform.right * fireForce, ForceMode2D.Impulse);
            
        }

        void Fire()
        {
            if (!auto || fireTimer.currentTimerState != Timer.TimerState.Stop) return;
            if (!loop)
            {
                if (!notLoopSingleUse) return;
                fireTimer.ReStart();
                notLoopSingleUse = false;
                return;
            }
            fireTimer.ReStart();
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
