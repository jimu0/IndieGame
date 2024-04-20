using Script.MVC.Module.Class;
using Script.MVC.Module.Frame.ObjectPool;
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
        [SerializeField] private bool pierce = true;//可穿透
        [SerializeField] private bool gravitation;//受引力影响
        [SerializeField] private bool lockOrient;//始终锁定朝向到移动方向
        [SerializeField] private float fireForce = 20f;//发射时的施加力

        public ObjectPool<Bullet> BulletPool;
        
        void Start()
        {
            BulletPool = new ObjectPool<Bullet>(BulletOnCreate, BulletOnGet, BulletOnRelease, BulletOnDestroy, true, 10, 40);
            
        }

        // void Update()
        // {
        // }

        public void SetGunPos(Vector2 posStart,Vector2 posEnd)
        {
            posGunStart = posStart;
            posGunEnd = posEnd;
        }

        private Bullet BulletOnCreate()
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            BulletClass bulletClass = bulletObj.GetComponent<BulletClass>();
            bulletClass.parent = this;
            bulletClass.owner = owner;
            bulletClass.collisionTrigger.owner = owner;
            Bullet bullet = new(bulletObj, bulletClass);
            bullet.BClass.Bullet = bullet;

            return bullet;
        }

        private void BulletOnGet(Bullet bullet)
        {
            bullet.GObj.SetActive(true);
        }

        private void BulletOnRelease(Bullet bullet)
        {
            bullet.GObj.SetActive(false);
        }

        private void BulletOnDestroy(Bullet bullet)
        {
            bullet.GObj.SetActive(false);
        }

        
        
        void GetBullet()
        {
            Bullet bullet = BulletPool.Get();
            bullet.BClass.owner = owner;
            bullet.BClass.collisionTrigger.owner = owner;
            bullet.GObj.transform.position = posGunStart;
            gunAim = posGunEnd - posGunStart;
            Quaternion rotation =  Quaternion.LookRotation(Vector3.forward,gunAim);
            bullet.GObj.transform.rotation = rotation;
            Vector3 bulletScale = bullet.GObj.transform.localScale;
            bulletScale.x *= gunAim.x / Mathf.Abs(gunAim.x);
            bullet.GObj.transform.localScale = bulletScale;
            bullet.BClass.boxCollider.size = Vector2.one;
            bullet.BClass.boxCollider.isTrigger = pierce;
            bullet.BClass.rigidbody.gravityScale = gravitation ? 1 : 0;
            
            bullet.BClass.rigidbody.AddForce(gunAim* fireForce, ForceMode2D.Impulse);
            bullet.BClass.lockOrient = lockOrient;
        }

        public void Fire()
        {
            GetBullet();
        }

        public class Bullet
        {
            public readonly GameObject GObj;
            public readonly BulletClass BClass;

            public Bullet(GameObject obj, BulletClass c)
            {
                GObj = obj;
                BClass = c;
            }
        }
        
        
    }
}
