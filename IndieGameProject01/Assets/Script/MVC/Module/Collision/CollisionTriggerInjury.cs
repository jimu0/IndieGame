using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Module.Collision
{
    public class CollisionTriggerInjury : MonoBehaviour
    {
        public GameObject owner;
        public BoxCollider2D boxCollider;
        
        void Start()
        {
            if (!owner) owner = gameObject;
        }

        void Update()
        {
        
        }
        
        
        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="collision">被碰撞者</param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if(owner)
                //判断碰撞物标签不能是同类，并且判断层级为9:生物或10:可破坏物，才通过
                if (!collision.gameObject.CompareTag(owner.tag) && (collision.gameObject.layer == 7||collision.gameObject.layer == 9||collision.gameObject.layer == 10))
                {
                    collision.gameObject.GetComponent<Biota>().Be_Hit(owner, 1);
                    //GameplayInit.Instance.DicPawns[collision.gameObject].Be_Hit(owner, 1);
                }
        

        }
    }
}
