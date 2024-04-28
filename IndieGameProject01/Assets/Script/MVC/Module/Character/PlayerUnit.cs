using Gamekit2D;
using Script.MVC.Module.Class;
using Script.MVC.Module.Ejector;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Character
{
    
    public class PlayerUnit : Biota,I_PlayerUnit
    {

        //private Rigidbody2D rig;
        private Timer timerJump;
        //private Vector2 flyHw = Vector2.zero;
        //public float flySpeed = 5f; // 推进器推力大小
        private float jumpForce;
        private float jumpDamping=3.6f;//移动衰减，用于起跳前蓄力时
        private bool isJumpD;
        private bool crouch;//是否蹲下
        private bool crouchDive;//允许下挑穿过单向平台
        //public bool isFlying;
        public float ux;
        private void Awake()
        {
            
            rig = GetComponent<Rigidbody2D>();
            gun = GetComponent<Gun>();
            gun.owner = gameObject;
            //_Tsf_ams = _Obj_ams.transform;
            //_ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            reactionSpeed = 0.1f;
            attackSpeed = 0.05f;
            //if (behavior == Behavior.stand) { }
            pos_gunStart = transform.Find("mod/pos_gunStart").gameObject;
            pos_gunEnd = transform.Find("mod/pos_gunEnd").gameObject;

        }

        // void Start()
        // {
        //
        //     
        // }
        
        void Update()
        {
            //_Tsf_ams.localPosition = _ams_pos;
            Vector3 position = gameObject.transform.position;
            posGunStart = position;
            posGunEnd.x = position.x + (orient_Preset * (1 - Mathf.Abs(posGunEnd0))) + (ux * Mathf.Abs(posGunEnd0));
            posGunEnd.y = position.y + posGunEnd0;
            pos_gunStart.transform.SetPositionAndRotation(posGunStart,pos_gunStart.transform.rotation);
            pos_gunEnd.transform.SetPositionAndRotation(posGunEnd,pos_gunEnd.transform.rotation);

            // Land();
            // if (behavior == Behavior.Die)
            // {
            //     TriggerResurrection();
            // }
        }
        
        // public CharacterController2D m_CharacterController2D;
        // public Vector2 m_MoveVector;
        // void FixedUpdate()
        // { 
        //     m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);
        //     //m_Animator.SetFloat(m_HashHorizontalSpeedPara, m_MoveVector.x);
        //     //m_Animator.SetFloat(m_HashVerticalSpeedPara, m_MoveVector.y);
        //     //UpdateBulletSpawnPointPositions();
        //     //UpdateCameraFollowTargetPosition();
        // }
        
        private void TimerReStart_SquatUp()
        {
            // 跳跃蓄力时间
            if (timerJump == null)
            {
                timerJump = Timer.Start(1f, (update) => 
                { 
                    jumpForce = 6f * update;
                    jumpDamping = 1.2f - update;
                }, () =>
                {
                    jumpDamping = 0.2f;
                }, 0.01f);
            }
            else
            {
                timerJump.ReStart();
            }
        }

        public void Move(float x)
        {
            ux = x;
            Moving(x,jumpForce,jumpDamping,3.6f);
        }

        public void Squat(float y)
        {
            //if(isFlying) flyHw.y = y;
            crouchDive = y < 0.1f && isPlatform;
            judgeTheSquat(y);
        }

        public void Defend()
        {
            ReadyDefend();
            longPress_Defend = true;
        }
        public void Defend_Cancel()
        {
            CancelDefend();
            longPress_Defend = false;
        }
        public void Attack()
        {
            ReadyAttack();
        }
        public void Attack_Cancel()
        {
            //取消攻击

            //timer_AttackBack.ReStart(attackSpeed);
        }

        public void Flash()
        {
            //Debug.Log("闪现");
        }

        public void JumpD()
        {
            isJumpD = true;
            if (isGround)
            {
                TimerReStart_SquatUp();
            }
        }
        public void Jump()
        {
            if (isJumpD && isGround)
            {
                isJumpD = false;
                if (crouchDive)
                {
                    timerJump.Pause();
                    jumpForce = 0;
                    if (boxCollider2D)
                    {
                        PlatformEffector2D platform = boxCollider2D.GetComponent<PlatformEffector2D>();
                        int playerLayer = LayerMask.NameToLayer("player"); // 获取 player 图层的索引
                        int layerMask = platform.colliderMask; // 获取当前的碰撞器掩码值
                        layerMask &= ~(1 << playerLayer); // 将 Player 图层对应的位设为 0，表示排除该图层
                        platform.colliderMask = layerMask; // 将更新后的掩码值应用到碰撞器
                        isPlatform = false;
                        //crouchDive = false;
                    }
                }
                else
                {
                    TimerReStart_SquatUp();
                }
            }
        }
        public void JumpU()
        {
            isJumpD = false;
            timerJump?.Pause();
            if (isGround && !crouchDive) ReadyJump(rig, jumpForce);
            jumpForce = 0;
            
        }

        public void Skill1(bool kd)
        {
            //handgun
        }

        public void Skill2()
        {
            
        }

        /// <summary>
        /// 飞行
        /// </summary>
        public void Skill3()
        {
            //Debug.Log("飞行");//forceMagnitude
            //isFlying = true; 
            rig.velocity = Vector2.zero;
            //timer_fly.ReStart(0.4f);
            Vector2 thrustDirection = new Vector2(1, 1); // 推力方向
            rig.AddForce(thrustDirection.normalized * 20, ForceMode2D.Impulse);
        }

        public void Skill4()
        {
            
        }

        //-------------------------------------------------------------------------------------------------

        public void CC_isGround() 
        {
            isGround = true;
        }
        public void CC_isNotGround() 
        {
            isGround = false;
        }
        // /// <summary>
        // /// 触发死亡
        // /// </summary>
        // public void TriggerDie()
        // {
        //     timerJump?.Pause();
        //     Die();
        // }
        // /// <summary>
        // /// 触发复活
        // /// </summary>
        // private void TriggerResurrection()
        // {
        //     Resurrection();
        // }
        


    }
}
