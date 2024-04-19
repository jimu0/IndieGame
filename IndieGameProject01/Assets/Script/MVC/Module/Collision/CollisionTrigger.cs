using Script.MVC.Module.Class;
using Script.MVC.Module.Frame.GameplayInit;
using UnityEngine;

namespace Script.MVC.Module.Collision
{
    public class CollisionTrigger : MonoBehaviour
    {
        [SerializeField] public Biota owner;//伤害所有者
        //private Biota biota;
        private const int MaxCollisionSize = 300; //最大碰撞盒体积限制，以保证宽敞的排泄区
        private int id;//每个碰撞器应该具有唯一性
        private Vector3 colHome = new(0, -10000, -10000);//排泄区
        private Vector2 colValue;
        private BoxCollider2D boxCollider;
        //public Timer TimerFade;//可视化射线计时器
        //[SerializeField] private float drawFade = 0.2f;//残留时间
        private readonly Color drawColor = Color.cyan;//可视化射线残留颜色
        private float offsetX, offsetY, sizeX, sizeY;
        private bool isDrawFade;//辅助线绘制淡出
        private void Awake()
        {
            //biota = owner.GetComponent<Biota>();
            colHome.y = -10000 + id * MaxCollisionSize;
            gameObject.transform.localPosition = colHome;
            if(!boxCollider) boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        // void Start()
        // {
        // }

        void Update()
        {
            if (isDrawFade)
            {
                DrawRect(offsetX,offsetY,sizeX,sizeY);
            }
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="collision">被碰撞者</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(owner)
            //判断碰撞物标签不能是同类，并且判断层级为9:生物或10:可破坏物，才通过
            if (!collision.gameObject.CompareTag(owner.tag) && (collision.gameObject.layer == 9||collision.gameObject.layer == 10))
            {
                //collision.gameObject.GetComponent<Biota>().Be_Hit(owner, 1);
                GameplayInit.Instance.DicPawns[collision.gameObject].Be_Hit(owner, 1);
                Col_OFF();
            }
        

        }

        //----------------------------------------------------------------------------------------------------------------
        
        /// 构造碰撞盒
        /// <param name="oX">位置x</param>
        /// <param name="oY">位置y</param>
        /// <param name="sX">宽度</param>
        /// <param name="sY">高度</param>
        private void Col_SetValue(float oX, float oY, float sX, float sY)
        {
            offsetX = oX;
            offsetY = oY;
            sizeX = sX;
            sizeY = sY;
            gameObject.transform.localPosition = Vector3.zero;
            colValue.x = oX; colValue.y = oY;
            boxCollider.offset = colValue;
            colValue.x = sX; colValue.y = sY;
            boxCollider.size = colValue;
        }
        /// <summary>
        /// 关闭碰撞盒
        /// </summary>
        public void Col_OFF()
        {
            isDrawFade = false;
            gameObject.transform.localPosition = colHome;
            colValue.x = 0; colValue.y = 0;
            boxCollider.offset = colValue;
            colValue.x = 0.01f; colValue.y = 0.01f;
            boxCollider.size = colValue;
        
        }
        /// <summary>
        /// 打开碰撞盒
        /// </summary>
        /// <param name="oX">位置x</param>
        /// <param name="oY">位置y</param>
        /// <param name="sX">宽度</param>
        /// <param name="sY">高度</param>
        public void Col_ON(float oX, float oY, float sX, float sY) 
        {
            Col_SetValue(oX, oY, sX, sY);
            isDrawFade = true;
            //StartCoroutine(DelayedExecution());

        }
        // IEnumerator DelayedExecution()
        // {
        //     yield return new WaitForSeconds(1.0f);
        //     isDrawFade = false;
        // }
        
        // 绘制矩形的方法，接受矩形的偏移量、宽度和高度作为参数
        void DrawRect(float oX, float oY, float sX, float sY)
        {
            // 计算矩形的四个顶点
            Vector3 position = transform.position;
            Vector2 topLeft = new (position.x + oX - sX / 2, position.y + oY + sY / 2);
            Vector2 topRight = new (position.x + oX + sX / 2, position.y + oY + sY / 2);
            Vector2 bottomLeft = new (position.x + oX - sX / 2, position.y + oY - sY / 2);
            Vector2 bottomRight = new (position.x + oX + sX / 2, position.y + oY - sY / 2);
            // 绘制矩形的四条边
            Debug.DrawLine(topLeft, topRight, drawColor);
            Debug.DrawLine(topRight, bottomRight, drawColor);
            Debug.DrawLine(bottomRight, bottomLeft, drawColor);
            Debug.DrawLine(bottomLeft, topLeft, drawColor);
        }
    }
}
