using System.Collections;
using Script.MVC.Module.Class;
using Script.MVC.Module.Frame.GameplayInit;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Collision
{
    public class CollisionTrigger : MonoBehaviour
    {
        [SerializeField] public GameObject owner;//?????????????????
        private Gladiatus gladiatus;//??????????

        protected Vector3 Col_home = new Vector3(0, -10000, 0);//????????��??
        //protected Vector3 Tsf_value = new Vector3();//?????????3?��??
        protected Vector2 Col_value = new Vector2();//?????????2?��??????????boxCollider.offset???boxCollider.size?
        BoxCollider2D boxCollider;//????????????
        public Timer timer_Fade;//可视化射线计时器
        private float DrawFade = 0.2f;//残留时间
        private Color DrawColor = Color.cyan;//可视化射线残留颜色
        private float offsetX, offsetY, sizeX, sizeY;
        private bool isDrawFade = false;
        private void Awake()
        {
            gladiatus = owner.GetComponent<Gladiatus>();
            gameObject.transform.localPosition = Col_home;
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        void Start()
        {
        }

        void Update()
        {
            if (isDrawFade)
            {
                DrawRect(offsetX,offsetY,sizeX,sizeY);
            }
        }

        //?????????????????????????????????????tag??layer???��???????????????????????????��
        /// <summary>
        /// ?????????
        /// </summary>
        /// <param name="collision">???????</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != owner.gameObject.tag && collision.gameObject.layer == 9)
            {
                //collision.gameObject.GetComponent<Gladiatus>().Be_Hit(owner, 1);
                GameplayInit.Instance.DicPawns[collision.gameObject].Be_Hit(owner, 1);
                Col_OFF();
            }
        

        }

        //----------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="oX">???X?</param>
        /// <param name="oY">???Y?</param>
        /// <param name="sX">???X?</param>
        /// <param name="sY">???Y?</param>
        protected void Col_SetValue(float oX, float oY, float sX, float sY)
        {
            offsetX = oX;
            offsetY = oY;
            sizeX = sX;
            sizeY = sY;
            gameObject.transform.localPosition = Vector3.zero;
            Col_value.x = oX; Col_value.y = oY;
            boxCollider.offset = Col_value;
            Col_value.x = sX; Col_value.y = sY;
            boxCollider.size = Col_value;
        }
        /// <summary>
        /// ???????????
        /// </summary>
        public void Col_OFF()
        {
            gameObject.transform.localPosition = Col_home;
            Col_value.x = 0; Col_value.y = 0;
            boxCollider.offset = Col_value;
            Col_value.x = 0.01f; Col_value.y = 0.01f;
            boxCollider.size = Col_value;
        
        }
        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="oX">???X?</param>
        /// <param name="oY">???Y?</param>
        /// <param name="sX">???X?</param>
        /// <param name="sY">???Y?</param>
        public void Col_ON(float oX, float oY, float sX, float sY) 
        {
            Col_SetValue(oX, oY, sX, sY);
            isDrawFade = true;
            StartCoroutine(DelayedExecution());

        }
        
        IEnumerator DelayedExecution()
        {
            yield return new WaitForSeconds(1.0f);
            isDrawFade = false;
        }
        
        // 绘制矩形的方法，接受矩形的偏移量、宽度和高度作为参数
        void DrawRect(float oX, float oY, float sX, float sY)
        {
            // 计算矩形的四个顶点
            Vector2 topLeft = new Vector2(transform.position.x + oX - sX / 2, transform.position.y + oY + sY / 2);
            Vector2 topRight = new Vector2(transform.position.x + oX + sX / 2, transform.position.y + oY + sY / 2);
            Vector2 bottomLeft = new Vector2(transform.position.x + oX - sX / 2, transform.position.y + oY - sY / 2);
            Vector2 bottomRight = new Vector2(transform.position.x + oX + sX / 2, transform.position.y + oY - sY / 2);
            // 绘制矩形的四条边
            Debug.DrawLine(topLeft, topRight, DrawColor);
            Debug.DrawLine(topRight, bottomRight, DrawColor);
            Debug.DrawLine(bottomRight, bottomLeft, DrawColor);
            Debug.DrawLine(bottomLeft, topLeft, DrawColor);
        }
    }
}
