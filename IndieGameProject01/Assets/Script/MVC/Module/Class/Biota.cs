using System.Collections;
using Script.MVC.Module.Collision;
using Script.MVC.Module.Ejector;
using Script.MVC.Other.Timer2;
using UnityEngine;
namespace Script.MVC.Module.Class
{
    public class Biota : Character
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

        //public GameObject _Obj_ams;//临时：角色架势可视化
        //public Transform _Tsf_ams;//临时模拟手臂的Transform
        //public Vector3 _ams_pos = new Vector3(0.4f, 0, 0);//临时：模拟手臂动作的box默认pos值
        //public SpriteRenderer _ams_SpR;//临时：角色防御可视化组件调用
        //Color DefendForward_color = new Color(255, 255, 255, 1);//临时：角色防御可视化_颜色
        public int squatValue = 0;//架势ID
        public bool longPress_Defend = false;//长按防御检测
        private float squatYO;//squatUpdate中的单次判断
        public Timer timer_SquatUp;//架势抬起前摇时间
        public Timer timer_SquatDown;//架势放下前摇时间
        public Timer timer_Squat;//架势返回后摇时间
        public Timer timer_AttackForward;//攻击前摇时间
        public Timer timer_AttackBack;//攻击后摇时间
        public Timer timer_DefendForward;//防御前摇时间

        public GameObject _Obj_ColTrigger;//碰撞触发器Obj
        public CollisionTrigger _CollisionTrigger;//攻击行为的碰撞触发器
        public bool Attack_Trigger = false;//是否开启攻击检测
        public float Move_SpeedAttenuation = 1;//移动速度衰减因子
        private int orient = 1;//当前朝向
        public int orient_Preset = 1;//当前朝向预设值
        private Vector3 OrientValue = new Vector3(1,1,1);//朝向值
        public bool OrientSwitch = false;//允许转向开关
        public bool TargetLocked = false;//目标锁定状态

        public float force = 5; //跳跃高度
        //private bool isGrounded = false;//是否着陆
        public Gun gun;//枪
        public Vector2 posGunStart = Vector2.zero;
        public Vector2 posGunEnd = Vector2.one;
        public float posGunEnd0;
        public GameObject pos_gunStart;
        public GameObject pos_gunEnd;
        public Rigidbody2D rig;
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

        private void TimerReStart_SquatUp()
        {
            if (timer_SquatUp == null)
            {
                timer_SquatUp = Timer.Start(reactionSpeed, (float timeUpdata) =>
                {
                    posGunEnd0 = timeUpdata / reactionSpeed;
                }, () => { squatValue = 1; }, 0.01f);
            }
            else
            {
                timer_SquatUp.ReStart();
            }
            // Debug.Log("架势：抬高"); 
        }
        private void TimerReStart_SquatDown() 
        {
            if (timer_SquatDown == null)
            {
                timer_SquatDown = Timer.Start(reactionSpeed, (float timeUpdata) => 
                    {
                        posGunEnd0 = -(timeUpdata / reactionSpeed);
                    }, () =>
                    {
                        squatValue = -1;
                    },
                    0.01f);
            }
            else
            {
                timer_SquatDown.ReStart();
            }
            // Debug.Log("架势：压低");
        }
        private void TimerReStart_Squat()
        {
            if (timer_Squat == null)
            {
                timer_Squat = Timer.Start(reactionSpeed, (float timeUpdata) =>
                {
                    posGunEnd0 =  squatValue * ((reactionSpeed-timeUpdata) / reactionSpeed);
                }, () =>
                {
                    squatValue = 0;
                    posGunEnd0 = 0;
                }, 0.01f);
            }
            else
            {
                timer_Squat.ReStart();
            }
            // Debug.Log("架势：平式"); 
        }
        private void TimerReStart_DefendForward()
        {
            if (timer_DefendForward == null)
            {
                timer_DefendForward = Timer.Start(attackSpeed, (float timeUpdata) => 
                    {
                    
                        //timer_SquatUp?.Pause(); timer_Squat?.Pause(); timer_SquatDown?.Pause();
                    },
                    () =>
                    {
                    
                    },
                    0.01f);
            }
            else
            {
                timer_DefendForward.ReStart();
            }
            // Debug.Log("防御：触发"); 
        }
        private void TimerReStart_AttackBack()
        {
            if (timer_AttackBack == null)
            {
                timer_AttackBack = Timer.Start(attackSpeed, (float timeUpdata) =>
                    {
                        timer_SquatUp?.Resume(); timer_Squat?.Resume(); timer_SquatDown?.Resume();
                    },
                    () =>
                    {
                        OrientSwitch = true;
                        Move_SpeedAttenuation = 1;
                        if (longPress_Defend) {  SetOrient(orient_Preset, false); ReadyDefend(); } else { behavior = Behavior.Stand; }
                    },
                    0.01f);
            }
            else
            {
                timer_AttackBack.ReStart();
            }
            //Debug.Log("状态返回：攻击"); 
        }
        private void TimerReStart_AttackForward()
        {
            if (timer_AttackForward == null)
            {
                timer_AttackForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                    {
                        Move_SpeedAttenuation = 1;
                        timer_SquatUp?.Pause(); timer_Squat?.Pause(); timer_SquatDown?.Pause();
                    },
                    () =>
                    {
                        TriggerAttack(gun, posGunStart, posGunEnd);
                        TimerReStart_AttackBack();
                    },
                    0.01f);
            }
            else
            {
                timer_AttackForward.ReStart();
            }
            // Debug.Log("攻击：触发"); 
        }
        
        
        
        
        public void Moving(float x, float jumpForce)
        {
            //if(isFlying) flyHw.x = x;
            float mSpeed = 4f;
            Vector2 pos = Vector2.zero;
            //transform.Translate(Vector3.forward * vertical * m_speed * Time.deltaTime);//?? ??
            if (behavior == Behavior.Attack)
            {
                if (isGround)
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation/3.6f)));//攻击时地面速度减慢
                    //pos.x = x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation / 3.6f);
                    //var b = transform.position.x + x*10;
                    //pos.x = b;
                }
                else
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation));//攻击时空中可以移动
                    //var b = transform.position.x + x*10;
                    //pos.x = b;

                }
            }
            else
            {
                if (jumpForce > 0)
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation/3.6f))); //????
                    //pos.x = x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation / 3.6f);

                }
                else
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation)); //????
                    //pos.x = x * mSpeed * Time.deltaTime * Move_SpeedAttenuation;

                }

            }
            SetOrient(x, TargetLocked);
            //Debug.Log(x.ToString());
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
                    //else { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }

                    
                    //GameObject muzzle = gameObject.transform.Find("mod/muzzle").gameObject;
                    //if(gunMuzzle) gunMuzzle.transform.localScale = OrientValue;
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
                    timer_Squat?.Pause();
                    timer_SquatDown?.Pause();
                    TimerReStart_SquatUp();
                    squatYO = y;
                }
            }
            else if (y <= -0.34)
            { //向下判断
                if (squatYO <= -0.34) { }
                else
                {
                    timer_SquatUp?.Pause();
                    timer_Squat?.Pause();
                    TimerReStart_SquatDown();
                    squatYO = y;
                }
            }
            else
            { //中间判断
                if (squatYO > -0.34 && squatYO < 0.34) { }
                else
                {
                    timer_SquatUp?.Pause();
                    timer_SquatDown?.Pause();
                    TimerReStart_Squat();
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
            //DefendForward_color.b = 255;
            //_ams_SpR.color = DefendForward_color;
            if (behavior == Behavior.Stand || behavior == Behavior.Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Attack;
                TimerReStart_AttackForward();
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
                TimerReStart_DefendForward();
            }
            if (behavior == Behavior.Attack && longPress_Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                TimerReStart_DefendForward();
            }
        }
        /// <summary>
        /// 取消防御
        /// </summary>
        public void CancelDefend()
        {

            if (behavior == Behavior.Defend)
            {
                timer_DefendForward?.Resume();
                OrientSwitch = true;
                behavior = Behavior.Stand;
                //DefendForward_color.b = 255;
                //_ams_SpR.color = DefendForward_color;
            }
            //Debug.Log("触发：取消防御");
        }

        /// <summary>
        /// 触发攻击开关
        /// </summary>
        /// <param name="weapon">武器/Gun</param>
        /// <param name="weaponStart">武器枪口位置</param>
        /// <param name="weaponEnd">武器瞄准位置</param>
        private void TriggerAttack(Gun weapon,Vector2 weaponStart,Vector2 weaponEnd)
        {
            if (!weapon) return;
            Attack_Trigger = true;
            weapon.SetGunPos(weaponStart,weaponEnd);
            weapon.Fire();
            _CollisionTrigger.Col_ON(2.1f*orient,0,4.2f,0.8f);
            StartCoroutine(CancelAttack(0.1f));
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
        /// <param name="source">伤害来源单位</param>
        /// <param name="damage">伤害值</param>
        public void Be_Hit(Biota source,float damage) 
        {
            if (_HP-damage <= 0)
            {
                _HP = 0;
                Die();
                return;
            }
            _HP -= damage;
            Debug.Log("受到来自【"+ source.name+"】的伤害，剩余生命值【"+_HP.ToString()+"】");
        }
        /// <summary>
        /// 触发死亡
        /// </summary>
        public void Die()
        {
            behavior = Behavior.Die;
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

        void FixedUpdate()
        {
            // 在FixedUpdate中进行物理计算
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!rig) return;
            // 获取碰撞的接触点信息
            ContactPoint2D[] contacts = collision.contacts;
            foreach (ContactPoint2D contact in contacts)
            {
                // 计算接触点的相对速度
                Vector2 relativeVelocity = contact.relativeVelocity;
                relativeVelocity.y *= 0.3f;
                // 施加给生物
                rig.AddForce(relativeVelocity, ForceMode2D.Impulse);
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

    }
}
