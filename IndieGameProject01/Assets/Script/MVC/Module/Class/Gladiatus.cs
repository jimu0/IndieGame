using System.Collections;
using Script.MVC.Module.Collision;
using Script.MVC.Module.Frame.ObjectPool;
using Script.MVC.Other.Timer2;
using UnityEngine;
namespace Script.MVC.Module.Class
{
    public class Gladiatus : Character
    {
        /// <summary>
        /// 行为状态
        /// </summary>
        public enum Behavior
        {
            /// <summary>
            /// 站立
            /// </summary>
            Stand,
            /// <summary>
            /// 防御
            /// </summary>
            Defend,
            /// <summary>
            /// 攻击
            /// </summary>
            Attack,
            /// <summary>
            /// 受击
            /// </summary>
            BeAttackde,
            /// <summary>
            /// 招架
            /// </summary>
            Parry,
            /// <summary>
            /// 僵直
            /// </summary>
            Rigidity,
            /// <summary>
            /// 死亡
            /// </summary>
            Die
        }
        /// <summary>
        /// 单位状态
        /// </summary>
        public enum Buff { }
        /// <summary>
        /// 目标
        /// </summary>
        public GameObject _targetObject = null;
        /// <summary>
        /// 当前行为状态
        /// </summary>
        public Behavior behavior = Behavior.Stand;

        //属性
        /// <summary>
        /// 生命值
        /// </summary>
        public float _HP;
        /// <summary>
        /// 体力值
        /// </summary>
        public float _SP;
        /// <summary>
        /// 力量
        /// </summary>
        public float strength;
        /// <summary>
        /// 护甲
        /// </summary>
        public float armor;
        /// <summary>
        /// //敏捷
        /// </summary>
        public float agility;
        /// <summary>
        /// 攻速
        /// </summary>
        public float attackSpeed = 0.1f;
        /// <summary>
        /// 移速
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// 动作反应速度
        /// </summary>
        public float reactionSpeed = 0.1f;
        /// <summary>
        /// 其他属性
        /// </summary>
        public struct Attr { }



        //角斗士当前行为状态

        public GameObject _Obj_ams;//临时：角色架势可视化
        public Transform _Tsf_ams;//临时模拟手臂的Transform
        public Vector3 _ams_pos = new Vector3(0.4f, 0, 0);//临时：模拟手臂动作的box默认pos值
        public SpriteRenderer _ams_SpR;//临时：角色防御可视化组件调用
        Color DefendForward_color = new Color(255, 255, 255, 1);//临时：角色防御可视化_颜色
        public int squatValue = 0;//架势ID
        public bool longPress_Defend = false;//长按防御检测
        private float squatYO;//squatUpdate中的单次判断
        public Timer timer_SquatUp;//架势抬起前摇时间
        public Timer timer_SquatDoun;//架势放下前摇时间
        public Timer timer_Squat;//架势返回后摇时间
        public Timer timer_AttackForward;//攻击前摇时间
        public Timer timer_AttackBack;//攻击后摇时间
        public Timer timer_DefendForward;//防御前摇时间

        public GameObject _Obj_ColTrigger;//碰撞触发器Obj
        public CollisionTrigger _CollisionTrigger;//攻击行为的碰撞触发器
        public bool Attack_Trigger = false;//是否开启攻击检测
        public float Move_SpeedAttenuation = 1;//移动速度衰减因子
        private int orient = 1;//当前朝向
        private int orient_Preset = 1;//当前朝向预设值
        private Vector3 OrientValue = new Vector3(1,1,1);//朝向值
        public bool OrientSwitch = false;//允许转向开关
        public bool TargetLocked = false;//目标锁定状态

        public float force = 5; //跳跃高度
        //private bool isGrounded = false;//是否着陆
        
        
        private void Awake()
        {
            //_Tsf_ams = _Obj_ams.transform;
            //_ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            //reactionSpeed = 0.1f;
            //attackSpeed = 0.2f;
            ////if (behavior == Behavior.stand) { }
        }

        void Start()
        {
            //ConstructionTimer();
        
        }




        

        void Update()
        {
            //Land();
            //_Tsf_ams.localPosition = _ams_pos;
            //if (Col_AttackTrigger) { }
        }




        /// <summary>
        /// 初始化计时器构造
        /// </summary>
        public void ConstructionTimer()
        {
            timer_SquatUp = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = 0.3f * (timeUpdata / reactionSpeed); }, () => { squatValue = 1; }, 0.01f);// Debug.Log("架势：抬高"); 

            timer_SquatDoun = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = -0.3f * (timeUpdata / reactionSpeed); }, () => { squatValue = -1; }, 0.01f);// Debug.Log("架势：压低"); 

            timer_Squat = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = 0.3f * squatValue * ((reactionSpeed - timeUpdata) / reactionSpeed); }, () => { squatValue = 0; }, 0.01f);// Debug.Log("架势：平式"); 

            timer_DefendForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    DefendForward_color.b = 255 - 255 * (timeUpdata / attackSpeed);
                    _ams_SpR.color = DefendForward_color;
                    //timer_SquatUp.Pause(); timer_Squat.Pause(); timer_SquatDoun.Pause();
                },
                () =>
                {
                    if (behavior == Behavior.Defend)
                    {
                        DefendForward_color.b = 0;
                        _ams_SpR.color = DefendForward_color;
                    }
                    else
                    {
                        DefendForward_color.b = 255;
                        _ams_SpR.color = DefendForward_color;
                    }
                    //timer_AttackBack.ReStart(attackSpeed);
                },
                0.01f);// Debug.Log("防御：触发"); 

            timer_AttackBack = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    _ams_pos.x = 0.4f + ((attackSpeed - timeUpdata) / attackSpeed);
                    timer_SquatUp.Resume(); timer_Squat.Resume(); timer_SquatDoun.Resume();
                },
                () =>
                {
                    OrientSwitch = true;
                    Move_SpeedAttenuation = 1;
                    if (longPress_Defend) {  SetOrient(orient_Preset, false); ReadyDefend(); } else { behavior = Behavior.Stand; }
                },
                0.01f);//Debug.Log("状态返回：攻击"); 

            timer_AttackForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    Move_SpeedAttenuation = 1;
                    _ams_pos.x = 4f * (timeUpdata / attackSpeed);
                    timer_SquatUp.Pause(); timer_Squat.Pause(); timer_SquatDoun.Pause();
                },
                () =>
                {
                    TriggerAttack(attackSpeed);
                    timer_AttackBack.ReStart(attackSpeed/10);
                },
                0.01f);// Debug.Log("攻击：触发"); 
            
            

        }
        /// <summary>
        /// 转向设置
        /// </summary>
        /// <param name="i">X轴值输入</param>
        /// <param name="TargetLocked">目标锁定状态</param>
        public void SetOrient(float i,bool TargetLocked)
        {

            if (TargetLocked) 
            {
                orient_Preset = orient; 
            } 
            else 
            {
                //预设值(只要防御输入没有响应就记录朝向预设,会在攻击后摇结束时，longPress_Defend条件为true时候设置此预设朝向为新朝向)
                if (!longPress_Defend)
                {
                    if (i > 0) { orient_Preset = 1; }
                    else if (i < 0) { orient_Preset = -1; }
                    else { orient_Preset = 1; }
                }


                if (OrientSwitch)
                {
                    //if (orient != orient_Preset) { orient = orient_Preset; }
                    if (i > 0)
                    { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    else if (i < 0)
                    { orient = -1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    else { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                }
            }

        }

        /// <summary>
        /// 准备架势判断
        /// </summary>
        /// <param name="y">y轴输入</param>
        public void judgeTheSquat(float y)
        {
            if (y >= 0.34)
            { //向上判断
                if (squatYO >= 0.34) { }
                else
                {
                    timer_Squat.Cancel();
                    timer_SquatDoun.Cancel();
                    timer_SquatUp.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            else if (y <= -0.34)
            { //向下判断
                if (squatYO <= -0.34) { }
                else
                {
                    timer_SquatUp.Cancel();
                    timer_Squat.Cancel();
                    timer_SquatDoun.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            else
            { //中间判断
                if (squatYO > -0.34 && squatYO < 0.34) { }
                else
                {
                    timer_SquatUp.Cancel();
                    timer_SquatDoun.Cancel();
                    timer_Squat.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            //Debug.Log(y.ToString());
        }

        /// <summary>
        /// 准备跳跃
        /// </summary>
        public void ReadyJump(Rigidbody2D rig, float jumpForce) 
        {
            //if (float.Parse(string.Format("{0:F1}", transform.position.y)) > 0.5f) return;
            if (behavior == Behavior.Stand) 
            {
                //Debug.Log("jump!");
                rig.velocity += Vector2.up * (force + jumpForce);
            }
        }


        /// <summary>
        /// 准备攻击
        /// </summary>
        public void ReadyAttack()
        {
            DefendForward_color.b = 255;
            _ams_SpR.color = DefendForward_color;
            if (behavior == Behavior.Stand || behavior == Behavior.Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Attack;
                timer_AttackForward.ReStart(attackSpeed);
                //Move_SpeedAttenuation = 0.01f;
            }
        }
        /// <summary>
        /// 准备防御
        /// </summary>
        public void ReadyDefend()
        {
        
            if (behavior == Behavior.Stand)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                timer_DefendForward.ReStart(attackSpeed);
            }
            if (behavior == Behavior.Attack && longPress_Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                timer_DefendForward.ReStart(attackSpeed);
            }
        }
        /// <summary>
        /// 取消防御
        /// </summary>
        public void CancelDefend()
        {

            if (behavior == Behavior.Defend)
            {
                timer_DefendForward.Cancel();
                OrientSwitch = true;
                behavior = Behavior.Stand;
                DefendForward_color.b = 255;
                _ams_SpR.color = DefendForward_color;
            }
            //Debug.Log("触发：取消防御");
        }
        /// <summary>
        /// 触发攻击开关
        /// </summary>
        /// <param name="time">触发时长/s</param>
        private void TriggerAttack(float time) 
        {
            Attack_Trigger = true;
            _CollisionTrigger.Col_ON(2.1f*orient,0,4.2f,0.8f);
            StartCoroutine(CancelAttack(time));
        }
        /// <summary>
        /// 通过协程，延迟停止攻击触发
        /// </summary>
        /// <param name="time">触发时长/s</param>
        /// <returns>延迟时间</returns>
        private IEnumerator CancelAttack(float time) 
        {
            yield return new WaitForSecondsRealtime(time);
            //yield return null;
            _CollisionTrigger.Col_OFF();
            Attack_Trigger = false;
        }
        /// <summary>
        /// 攻击碰撞检测响应
        /// </summary>
        /// <param name="collision">碰撞属性</param>
        public void Attack_Hit(Collider2D collision)
        {
        
            if (Attack_Trigger)//Attack_Trigger
            {
                //_targetObject = collision.gameObject;//根据判断条件得到目标单位
                //Debug.Log("击");
                Attack_Trigger = false;

            }
        }
        /// <summary>
        /// 受击碰撞检测响应
        /// </summary>
        /// <param name="Source">伤害来源单位</param>
        /// <param name="Damage">伤害值</param>
        public void Be_Hit(GameObject Source,float Damage) 
        {
            if (_HP-Damage <= 0)
            {
                _HP = 0;
                Die();
                return;
            }
            _HP -= Damage;
            Debug.Log("受到来自【"+ Source.name+"】的伤害，剩余生命值【"+_HP.ToString()+"】");
        }
        /// <summary>
        /// 触发死亡
        /// </summary>
        public void Die()
        {
            Debug.Log(this.name+"死亡");
        }

        

        void OnTriggerEnter2D(Collider2D other)
        {
            // 如果触发到的碰撞器是地面的碰撞器
            if (other.CompareTag("Ground"))
            {
                isGround = true;
                Debug.Log("OnTriggerExit2D:Grounded");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // 如果离开了地面的碰撞器
            if (other.CompareTag("Ground"))
            {
                isGround = false;
                Debug.Log("OnTriggerExit2D:Floating");
            }
        }

        public LayerMask groundLayer; // 将1左移7位，表示只检测第7图层
        public void Land()
        {
            Vector2 pos = Vector2.down;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, 0.6f);
            isGround = false;
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == 6)
                {
                    isGround = true; // 标记碰撞到Layer 6
                    break; // 如果已找到Layer 6的物体，无需进一步检查
                }
            }
            Color rayColor = isGround ? Color.red : Color.green;
            Debug.DrawRay(pos, Vector2.down * 0.6f, rayColor);
        }
        
        public void Fire(Timer fireTime,bool sticking,Rigidbody2D fireBulletRig)
        {
            //bulletObj = bulletPool.Get();
            //bulletBox = fireBulletBox;
            //bulletRig = fireBulletRig;
            
            fireTime.ReStart();
        }

    }
}
